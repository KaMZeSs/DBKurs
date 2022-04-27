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
using Npgsql;

namespace DBKurs.Forms
{
    delegate void UpdateTable();

    public partial class MainForm : Form
    {
        UpdateTable updator;

        private readonly static String connectString = "Host=localhost;Port=5432;User Id=postgres;Password=1310;Database=Kurs";
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;
        private DataTable dt;

        public MainForm()
        {
            InitializeComponent();
            updator = Update_Albums;
            conn = new NpgsqlConnection(connectString);
        }

        #region Updaters

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String name = ((ToolStripMenuItem)sender).Text;

            switch (name)
            {
                case "Ассортимент":
                    updator = Update_ProductRanges;
                    break;
                case "Альбом":
                    updator = Update_Albums;
                    break;
                case "Магазин":
                    updator = Update_Shops;
                    break;
                case "Страна":
                    updator = Update_Countries;
                    break;
                case "Город":
                    updator = Update_Cities;
                    break;
                case "Фирма звукозаписи":
                    updator = Update_RecordFirms;
                    break;
                case "Жанр исполнения":
                    updator = Update_Genres;
                    break;
                case "Тип записи":
                    updator = Update_RecordTypes;
                    break;
                case "Язык исполнения":
                    updator = Update_Languages;
                    break;
                case "Исполнитель":
                    updator = Update_Executors;
                    break;
                case "Владелец":
                    updator = Update_Owners;
                    break;
                case "Тип собственности":
                    updator = Update_ProperyTypes;
                    break;
                case "Район города":
                    updator = Update_Districts;
                    break;
                default:
                    MessageBox.Show($"Забыл/перепутал {name}");
                    break;
            }

            updator.Invoke();
        }

        private async void Update_Cities()
        {
            try
            {
                conn.Open();
                sql = @"select * from Cities";
                cmd = new NpgsqlCommand(sql, conn);

                dt = new DataTable();

                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null; //очистка таблицы
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private async void Update_Countries()
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private async void Update_Executors()
        {
            try
            {
                conn.Open();
                sql = @"select * from Executors";
                cmd = new NpgsqlCommand(sql, conn);

                dt = new DataTable();

                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null; //очистка таблицы
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private async void Update_Genres()
        {
            try
            {
                conn.Open();
                sql = @"select * from Genres";
                cmd = new NpgsqlCommand(sql, conn);

                dt = new DataTable();

                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null; //очистка таблицы
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private async void Update_Languages()
        {
            try
            {
                conn.Open();
                sql = @"select * from Languages";
                cmd = new NpgsqlCommand(sql, conn);

                dt = new DataTable();

                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null; //очистка таблицы
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private async void Update_RecordFirms()
        {
            try
            {
                conn.Open();
                sql = @"select * from RecordFirms";
                cmd = new NpgsqlCommand(sql, conn);

                dt = new DataTable();

                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null; //очистка таблицы
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private async void Update_RecordTypes()
        {
            try
            {
                conn.Open();
                sql = @"select * from RecordTypes";
                cmd = new NpgsqlCommand(sql, conn);

                dt = new DataTable();

                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null; //очистка таблицы
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private async void Update_Districts()
        {
            try
            {
                conn.Open();
                sql = @"select * from Districts";
                cmd = new NpgsqlCommand(sql, conn);

                dt = new DataTable();

                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null; //очистка таблицы
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private async void Update_Owners()
        {
            try
            {
                conn.Open();
                sql = @"select * from Owners";
                cmd = new NpgsqlCommand(sql, conn);

                dt = new DataTable();

                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null; //очистка таблицы
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private async void Update_ProperyTypes()
        {
            try
            {
                conn.Open();
                sql = @"select * from ProperyTypes";
                cmd = new NpgsqlCommand(sql, conn);

                dt = new DataTable();

                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null; //очистка таблицы
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private async void Update_Albums()
        {
            try
            {
                conn.Open();
                sql = @"select * from Albums";
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private async void Update_ProductRanges()
        {
            try
            {
                conn.Open();
                sql = @"select * from ProductRanges";
                cmd = new NpgsqlCommand(sql, conn);

                dt = new DataTable();

                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null; //очистка таблицы
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private async void Update_Shops()
        {
            try
            {
                conn.Open();
                sql = @"select * from Shops";
                cmd = new NpgsqlCommand(sql, conn);

                dt = new DataTable();

                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null; //очистка таблицы
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updator.Invoke();
        }
    }
}
