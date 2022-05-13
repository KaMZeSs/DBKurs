using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Forms
{
    public partial class CompoundForm : Form
    {
        private readonly string connectString;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private DataTable dtShops, dtAlbums;
        private int current;

        public CompoundForm(string connString)
        {
            this.InitializeComponent();
            connectString = connString;
            current = 0;
        }

        private async void CompoundForm_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);
            await this.Load_all_shops();
            await this.Load_Next_Shop();
        }

        private async Task Load_all_shops()
        {
            try
            {
                conn.Open();

                cmd = new NpgsqlCommand($"SELECT * FROM Get_All_Shops()", conn);

                dtShops = new DataTable();
                dtShops.Load(await cmd.ExecuteReaderAsync());
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private async Task Load_Next_Shop()
        {
            if (current >= dtShops.Rows.Count | current < 0)
                return;

            //load shop data
            Shop_name.Text = dtShops.Rows[current]["Название магазина"].ToString();
            district.Text = dtShops.Rows[current]["Район города"].ToString();
            adres.Text = dtShops.Rows[current]["Адрес"].ToString();
            property_type.Text = dtShops.Rows[current]["Тип собственности"].ToString();
            license.Text = dtShops.Rows[current]["Лицензия"].ToString();
            license_expiration.Text = ((DateTime)dtShops.Rows[current]["Дата окончания лицензии"]).ToShortDateString();
            owner_name.Text = dtShops.Rows[current]["Владелец"].ToString();
            year_opened.Text = dtShops.Rows[current]["Год открытия"].ToString();

            //load albums from that shop
            try
            {
                conn.Open();

                int alb_id = (int)dtShops.Rows[current]["id"];

                cmd = new NpgsqlCommand($"SELECT * FROM Get_All_Albums_From_Shop({alb_id})", conn);
                dtAlbums = new DataTable();
                dtAlbums.Load(await cmd.ExecuteReaderAsync());

                DataTable dtCloned = dtAlbums.Clone();
                dtCloned.BeginLoadData();
                dtCloned.Columns[dtCloned.Columns.Count - 2].DataType = typeof(Image);

                foreach (DataRow dataRow in dtAlbums.Rows)
                {
                    DataRow row = dtCloned.NewRow();
                    for (int i = 0; i < row.ItemArray.Length - 2; i++)
                    {
                        row[i] = dataRow[i];
                    }

                    string base64String = Encoding.Default.GetString((byte[])dataRow[14]);
                    byte[] byteArray = Convert.FromBase64String(base64String);

                    using (var ms = new MemoryStream(byteArray))
                    {
                        var img = Image.FromStream(ms);
                        row[14] = img;
                    }
                    row[row.ItemArray.Length - 1] = dataRow[row.ItemArray.Length - 1];

                    dtCloned.Rows.Add(row);
                }
                dataGridView1.RowTemplate.Height = 50;

                dataGridView1.DataSource = null; //очистка таблицы

                dataGridView1.DataSource = dtCloned;
                dataGridView1.Columns[13].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        #region Styles

        readonly Color mouseOver = Color.FromArgb(230, 230, 230);
        readonly Color mouseOverSelected = Color.FromArgb(102, 145, 178);
        readonly Color mouseLeftSelected = Color.FromArgb(153, 181, 204);

        int mouseOverCell_index = 0;

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                if (e.ColumnIndex == dataGridView1.ColumnCount - 1)
                    return;
                dataGridView1.Columns[e.ColumnIndex].HeaderCell.Style.SelectionBackColor =
                dataGridView1.Columns[e.ColumnIndex].HeaderCell.Style.BackColor = mouseOver;
            }
            else
            {
                mouseOverCell_index = e.RowIndex;
                if (dataGridView1.Rows[e.RowIndex].Selected)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = mouseOverSelected;
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = mouseOver;
                }
            }
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                if (e.ColumnIndex == dataGridView1.ColumnCount - 1)
                    return;
                dataGridView1.Columns[e.ColumnIndex].HeaderCell.Style.SelectionBackColor =
                dataGridView1.Columns[e.ColumnIndex].HeaderCell.Style.BackColor = Color.White;
            }
            else
            {
                if (dataGridView1.Rows[e.RowIndex].Selected)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = mouseLeftSelected;
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0 | mouseOverCell_index == -2)
                return;
            if (dataGridView1.Rows[mouseOverCell_index].DefaultCellStyle.BackColor == mouseOver)
            {
                dataGridView1.RowHeadersDefaultCellStyle.SelectionBackColor =
                dataGridView1.Rows[mouseOverCell_index].DefaultCellStyle.SelectionBackColor = mouseOverSelected;
                dataGridView1.Rows[mouseOverCell_index].DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource is null)
                return;
            var column = new DataGridViewColumn
            {
                DefaultCellStyle = dataGridView1.DefaultCellStyle
            };
            column.HeaderCell.Style.BackColor = column.HeaderCell.Style.SelectionBackColor = dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            column.ReadOnly = true;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(column);
        }

        #endregion

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.SplitterDistance > 167)
                splitContainer1.SplitterDistance = 167;
        }

        private async void direction_button_Click(object sender, EventArgs e)
        {
            if ((sender as Button) == right_button)
            {
                current++;
                if (current >= dtShops.Rows.Count)
                    current = dtShops.Rows.Count - 1;
            }
            else
            {
                current--;
                if (current < 0)
                    current = 0;
            }

            await this.Load_Next_Shop();
        }
    }
}
