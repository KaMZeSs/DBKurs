using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using DBKurs.Styles;
using Npgsql;


namespace DBKurs.Forms
{
    delegate void UpdateTable();

    enum Tables { ProductRanges, Albums, Shops, Cities, Countries, Executors, Genres, Languages, RecordFirms, RecordTypes, Districts, Owners, PropertyTypes }

    public partial class MainForm : Form
    {
        UpdateTable updator, updator_continue;

        public readonly static String connectString = "Host=localhost;Port=5432;User Id=postgres;Password=1310;Database=Kurs";
        public NpgsqlConnection conn;
        public String sql;
        public NpgsqlCommand cmd;
        public DataTable dt;

        private Tables currentTable;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new TestColorTable());
            

            conn = new NpgsqlConnection(connectString);

            updator_continue = () =>
            {
                int[] widths = new int[dataGridView1.ColumnCount - 1];

                for (int i = 0; i < widths.Length; i++)
                {
                    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    widths[i] = dataGridView1.Columns[i].Width;
                    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dataGridView1.Columns[i].Width = widths[i];
                }
            };

            городToolStripMenuItem.PerformClick();
        }

        #region Updaters

        private async void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String name = ((ToolStripMenuItem)sender).Text;
            dataGridView1.RowTemplate.Height = 33;

            updator = () => { mouseOverCell_index = -2; dataGridView1.Columns.Clear(); };

            switch (name)
            {
                case "Ассортимент":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Get_All_ProductRanges()";
                            cmd = new NpgsqlCommand(sql, conn);

                            dt = new DataTable();

                            dt.Load(await cmd.ExecuteReaderAsync());
                            dataGridView1.DataSource = null; //очистка таблицы
                            dataGridView1.DataSource = dt;
                            updator_continue.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    };
                    currentTable = Tables.ProductRanges;
                    break;
                case "Альбом":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Get_All_Albums()";
                            cmd = new NpgsqlCommand(sql, conn);

                            dt = new DataTable();

                            dt.Load(await cmd.ExecuteReaderAsync());

                            DataTable dtCloned = dt.Clone();
                            dtCloned.BeginLoadData();
                            dtCloned.Columns[dtCloned.Columns.Count - 2].DataType = typeof(Image);

                            foreach (DataRow dataRow in dt.Rows)
                            {
                                DataRow row = dtCloned.NewRow();
                                for (int i = 0; i < row.ItemArray.Length - 2; i++)
                                {
                                    row[i] = dataRow[i];
                                }

                                var base64String = Encoding.Default.GetString((byte[])dataRow[12]);
                                byte[] byteArray = Convert.FromBase64String(base64String);

                                using (var ms = new MemoryStream(byteArray))
                                {
                                    Image img = Image.FromStream(ms);
                                    row[12] = img;
                                }
                                row[row.ItemArray.Length - 1] = dataRow[row.ItemArray.Length - 1];

                                dtCloned.Rows.Add(row);
                            }
                            dataGridView1.RowTemplate.Height = 50;

                            dataGridView1.DataSource = null; //очистка таблицы

                            dataGridView1.DataSource = dtCloned;
                            dataGridView1.Columns[11].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                            
                            updator_continue.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    };
                    currentTable = Tables.Albums;
                    break;
                case "Магазин":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Get_All_Shops()";
                            cmd = new NpgsqlCommand(sql, conn);

                            dt = new DataTable();

                            dt.Load(await cmd.ExecuteReaderAsync());
                            dataGridView1.DataSource = null; //очистка таблицы
                            dataGridView1.DataSource = dt;
                            updator_continue.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    };
                    currentTable = Tables.Shops;
                    break;
                case "Страна":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = "select * from Get_All_Countries()";
                            cmd = new NpgsqlCommand(sql, conn);

                            dt = new DataTable();

                            dt.Load(await cmd.ExecuteReaderAsync());
                            dataGridView1.DataSource = null; //очистка таблицы
                            dataGridView1.DataSource = dt;
                            updator_continue.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    };
                    currentTable = Tables.Countries;
                    break;
                case "Город":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = "SELECT * FROM Get_All_Cities()";
                            cmd = new NpgsqlCommand(sql, conn);

                            dt = new DataTable();

                            dt.Load(await cmd.ExecuteReaderAsync());
                            dataGridView1.DataSource = null; //очистка таблицы
                            dataGridView1.DataSource = dt;
                            updator_continue.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    };
                    currentTable = Tables.Cities;
                    break;
                case "Фирма звукозаписи":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Get_All_RecordFirms()";
                            cmd = new NpgsqlCommand(sql, conn);

                            dt = new DataTable();

                            dt.Load(await cmd.ExecuteReaderAsync());
                            dataGridView1.DataSource = null; //очистка таблицы
                            dataGridView1.DataSource = dt;
                            updator_continue.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    };
                    currentTable = Tables.RecordFirms;
                    break;
                case "Жанр исполнения":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Get_All_Genres()";
                            cmd = new NpgsqlCommand(sql, conn);

                            dt = new DataTable();

                            dt.Load(await cmd.ExecuteReaderAsync());
                            dataGridView1.DataSource = null; //очистка таблицы
                            dataGridView1.DataSource = dt;
                            updator_continue.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    };
                    currentTable = Tables.Genres;
                    break;
                case "Тип записи":
                    updator += async () =>
                    {
                        try
                        {
                            conn.OpenAsync();
                            sql = @"select * from Get_All_RecordTypes()";
                            cmd = new NpgsqlCommand(sql, conn);

                            dt = new DataTable();

                            dt.Load(await cmd.ExecuteReaderAsync());
                            dataGridView1.DataSource = null; //очистка таблицы
                            dataGridView1.DataSource = dt;
                            updator_continue.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }
                        finally
                        {
                            conn.CloseAsync();
                        }
                    };
                    currentTable = Tables.RecordTypes;
                    break;
                case "Язык исполнения":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Get_All_Languages()";
                            cmd = new NpgsqlCommand(sql, conn);

                            dt = new DataTable();

                            dt.Load(await cmd.ExecuteReaderAsync());
                            dataGridView1.DataSource = null; //очистка таблицы
                            dataGridView1.DataSource = dt;
                            updator_continue.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    };
                    currentTable = Tables.Languages;
                    break;
                case "Исполнитель":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Get_All_Executors()";
                            cmd = new NpgsqlCommand(sql, conn);

                            dt = new DataTable();

                            dt.Load(await cmd.ExecuteReaderAsync());
                            dataGridView1.DataSource = null; //очистка таблицы
                            dataGridView1.DataSource = dt;
                            updator_continue.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    };
                    currentTable = Tables.Executors;
                    break;
                case "Владелец":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Get_All_Owners()";
                            cmd = new NpgsqlCommand(sql, conn);

                            dt = new DataTable();

                            dt.Load(await cmd.ExecuteReaderAsync());
                            dataGridView1.DataSource = null; //очистка таблицы
                            dataGridView1.DataSource = dt;
                            updator_continue.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    };
                    currentTable = Tables.Owners;
                    break;
                case "Тип собственности":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Get_All_PropertyTypes()";
                            cmd = new NpgsqlCommand(sql, conn);

                            dt = new DataTable();

                            dt.Load(await cmd.ExecuteReaderAsync());
                            dataGridView1.DataSource = null; //очистка таблицы
                            dataGridView1.DataSource = dt;
                            updator_continue.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    };
                    currentTable = Tables.PropertyTypes;
                    break;
                case "Район города":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Get_All_Districts()";
                            cmd = new NpgsqlCommand(sql, conn);

                            dt = new DataTable();

                            dt.Load(await cmd.ExecuteReaderAsync());
                            dataGridView1.DataSource = null; //очистка таблицы
                            dataGridView1.DataSource = dt;
                            updator_continue.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    };
                    currentTable = Tables.Districts;
                    break;
            }
            updator.Invoke();
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            updator.Invoke();
        }

        #endregion

        #region Add/Delete/Update

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;

            switch (currentTable)
            {
                case Tables.ProductRanges:
                    dialogResult = new Add.AddProductRange(connectString).ShowDialog(this);
                    break;
                case Tables.Albums:
                    dialogResult = new Add.AddAlbum(connectString).ShowDialog(this);
                    break;
                case Tables.Shops:
                    dialogResult = new Add.AddShop(connectString).ShowDialog(this);
                    break;
                case Tables.Cities:
                    dialogResult = new Add.AddCity(connectString).ShowDialog(this);
                    break;
                case Tables.Countries:
                    dialogResult = new Add.AddCountry(connectString).ShowDialog(this);
                    break;
                case Tables.Executors:
                    dialogResult = new Add.AddExecutor(connectString).ShowDialog(this);
                    break;
                case Tables.Genres:
                    dialogResult = new Add.AddGenre(connectString).ShowDialog(this);
                    break;
                case Tables.Languages:
                    dialogResult = new Add.AddLanguage(connectString).ShowDialog(this);
                    break;
                case Tables.RecordFirms:
                    dialogResult = new Add.AddRecordFirm(connectString).ShowDialog(this);
                    break;
                case Tables.RecordTypes:
                    dialogResult = new Add.AddRecordType(connectString).ShowDialog(this);
                    break;
                case Tables.Districts:
                    dialogResult = new Add.AddDistrict(connectString).ShowDialog(this);
                    break;
                case Tables.Owners:
                    dialogResult = new Add.AddOwner(connectString).ShowDialog(this);
                    break;
                case Tables.PropertyTypes:
                    dialogResult = new Add.AddPropertyType(connectString).ShowDialog(this);
                    break;
                default:
                    return;
            }

            if (dialogResult == DialogResult.OK)
                updator.Invoke();
        }

        private async void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выделите как минимум одну строку для удаления");
                return;
            }

            int[] ids = new int[dataGridView1.SelectedRows.Count];

            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = (int)dataGridView1.SelectedRows[i].Cells["id"].Value;
            }

            try
            {
                await conn.OpenAsync();
            }
            catch (Exception exc)
            {
                switch (currentTable)
                {
                    case Tables.ProductRanges:
                        break;
                    case Tables.Albums:
                        break;
                    case Tables.Shops:
                        break;
                    case Tables.Cities:
                        sql = $"SELECT";
                        break;
                    case Tables.Countries:
                        break;
                    case Tables.Executors:
                        break;
                    case Tables.Genres:
                        break;
                    case Tables.Languages:
                        break;
                    case Tables.RecordFirms:
                        break;
                    case Tables.RecordTypes:
                        break;
                    case Tables.Districts:
                        break;
                    case Tables.Owners:
                        break;
                    case Tables.PropertyTypes:
                        break;
                    default:
                        break;
                }
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        #endregion

        #region Styles

        Color mouseOver = Color.FromArgb(230, 230, 230);
        Color mouseOverSelected = Color.FromArgb(102, 145, 178);
        Color mouseLeftSelected = Color.FromArgb(153, 181, 204);

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

        private void составнаяФормаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CompoundForm(connectString).ShowDialog(this);
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource is null)
                return;
            DataGridViewColumn column = new DataGridViewColumn();
            column.DefaultCellStyle = dataGridView1.DefaultCellStyle;
            column.HeaderCell.Style.BackColor = column.HeaderCell.Style.SelectionBackColor = dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            column.ReadOnly = true;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(column);
        }

        #endregion
    }
}
