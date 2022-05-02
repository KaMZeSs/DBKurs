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

            updator = () => dataGridView1.Columns.Clear();

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
                            conn.Open();
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
                            conn.Close();
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

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (currentTable)
            {
                case Tables.ProductRanges:
                    new Add.AddProductRange().ShowDialog(this);
                    break;
                case Tables.Albums:
                    new Add.AddAlbum().ShowDialog(this);
                    break;
                case Tables.Shops:
                    new Add.AddShop().ShowDialog(this);
                    break;
                case Tables.Cities:
                    new Add.AddCity().ShowDialog(this);
                    break;
                case Tables.Countries:
                    new Add.AddCountry().ShowDialog(this);
                    break;
                case Tables.Executors:
                    new Add.AddExecutor().ShowDialog(this);
                    break;
                case Tables.Genres:
                    new Add.AddGenre().ShowDialog(this);
                    break;
                case Tables.Languages:
                    new Add.AddLanguage().ShowDialog(this);
                    break;
                case Tables.RecordFirms:
                    new Add.AddRecordFirm().ShowDialog(this);
                    break;
                case Tables.RecordTypes:
                    new Add.AddRecordType().ShowDialog(this);
                    break;
                case Tables.Districts:
                    new Add.AddDistrict().ShowDialog(this);
                    break;
                case Tables.Owners:
                    new Add.AddOwner().ShowDialog(this);
                    break;
                case Tables.PropertyTypes:
                    new Add.AddPropertyType().ShowDialog(this);
                    break;
            }
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            else
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230);
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            else
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
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
            //column.Resizable = DataGridViewTriState.False;
            column.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(column);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column.DisplayIndex == dataGridView1.ColumnCount - 1)
            {
                
            }
        }
    }
}
