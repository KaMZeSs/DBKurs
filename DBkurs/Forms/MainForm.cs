using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using DBKurs.Forms.Add;
using DBKurs.Styles;
using Npgsql;
using ClosedXML.Excel;
using System.Collections.Generic;

namespace DBKurs.Forms
{
    delegate void UpdateTable();

    public enum Tables
    {
        ProductRanges,
        Albums,
        Shops,
        Cities,
        Countries,
        Executors,
        Genres,
        Languages,
        RecordFirms,
        RecordTypes,
        Districts,
        Owners,
        PropertyTypes
    }

    public partial class MainForm : Form
    {
        UpdateTable updator, updator_continue;

        private readonly static string connectString = "Host=localhost;Port=5432;User Id=postgres;Password=1310;Database=Kurs";
        private NpgsqlConnection conn;
        private string sql;
        private NpgsqlCommand cmd;
        private DataTable dt;

        public MainForm()
        {
            this.InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new TestColorTable());

            conn = new NpgsqlConnection(connectString);

            updator_continue = () =>
            {
                var task = Task.Factory.StartNew((x) =>
                {
                    return GetWidths(x as DataTable, dataGridView1.DefaultCellStyle.Font);
                }, (dataGridView1.DataSource as DataTable).Copy());

                task.ContinueWith(this.SetColumnWidths);

                //task.ContinueWith();


                //int[] widths = new int[dataGridView1.ColumnCount - 1];

                //for (int i = 0; i < widths.Length; i++)
                //{
                //    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                //    widths[i] = dataGridView1.Columns[i].Width;
                //    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                //    dataGridView1.Columns[i].Width = widths[i];
                //}
            };

            городToolStripMenuItem.PerformClick();
        }

        #region Properties

        public Tables CurrentTable { get; private set; }

        #endregion

        #region Events
        public class TableEventArgs
        {
            public Tables NewTable { get; }

            public TableEventArgs(Tables newTable)
            {
                NewTable = newTable;
            }
        }
        public delegate void On_Table_Updated(object sender, TableEventArgs e);

        public event On_Table_Updated onTable_Updated;

        #endregion

        #region Updaters

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (conn.State == ConnectionState.Open)
            {
                MessageBox.Show("Дождитесь завершения операции");
                return;
            }
            string name = ((ToolStripMenuItem)sender).Text;
            dataGridView1.RowTemplate.Height = 33;

            updator = () => { mouseOverCell_index = -2; dataGridView1.Columns.Clear(); };
            поискToolStripMenuItem.Enabled = true;
            обновитьToolStripMenuItem.Enabled = true;
            данныеToolStripMenuItem.Enabled = true;
            switch (name)
            {
                case "Ассортимент":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Show_productranges";
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
                    CurrentTable = Tables.ProductRanges;
                    break;
                case "Альбом":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Show_albums";
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

                                string base64String = Encoding.Default.GetString((byte[])dataRow[12]);
                                byte[] byteArray = Convert.FromBase64String(base64String);

                                using (var ms = new MemoryStream(byteArray))
                                {
                                    var img = Image.FromStream(ms);
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
                    CurrentTable = Tables.Albums;
                    break;
                case "Магазин":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Show_shops";
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
                    CurrentTable = Tables.Shops;
                    break;
                case "Страна":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = "select * from Show_countries";
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
                    CurrentTable = Tables.Countries;
                    break;
                case "Город":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = "SELECT * FROM Show_cities";
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
                    CurrentTable = Tables.Cities;
                    break;
                case "Фирма звукозаписи":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Show_recordFirms";
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
                    CurrentTable = Tables.RecordFirms;
                    break;
                case "Жанр исполнения":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Show_genres";
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
                    CurrentTable = Tables.Genres;
                    break;
                case "Тип записи":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Show_recordTypes";
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
                    CurrentTable = Tables.RecordTypes;
                    break;
                case "Язык исполнения":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Show_languages";
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
                    CurrentTable = Tables.Languages;
                    break;
                case "Исполнитель":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Show_executors";
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
                    CurrentTable = Tables.Executors;
                    break;
                case "Владелец":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Show_owners";
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
                    CurrentTable = Tables.Owners;
                    break;
                case "Тип собственности":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Show_propertyTypes";
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
                    CurrentTable = Tables.PropertyTypes;
                    break;
                case "Район города":
                    updator += async () =>
                    {
                        try
                        {
                            conn.Open();
                            sql = @"select * from Show_districts";
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
                    CurrentTable = Tables.Districts;
                    break;
            }
            updator.Invoke();

            Task.Factory.StartNew(() =>
            {
                onTable_Updated?.Invoke(this, new TableEventArgs(CurrentTable));
            });
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e) => updator.Invoke();

        #endregion

        #region Add/Delete/Update

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;

            switch (CurrentTable)
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

            string selected = "'{" + String.Join(", ", ids) + "}'";

            sql = "SELECT * FROM ";

            switch (CurrentTable)
            {
                case Tables.ProductRanges:
                    break;
                case Tables.Albums:
                    sql += $"CountCascadeAlbums({selected})";
                    break;
                case Tables.Shops:
                    sql += $"CountCascadeShops({selected})";
                    break;
                case Tables.Cities:
                    sql += $"CountCascadeCities({selected})";
                    break;
                case Tables.Countries:
                    sql += $"CountCascadeCountries({selected})";
                    break;
                case Tables.Executors:
                    sql += $"CountCascadeExecutors({selected})";
                    break;
                case Tables.Genres:
                    sql += $"CountCascadeGenres({selected})";
                    break;
                case Tables.Languages:
                    sql += $"CountCascadeLanguages({selected})";
                    break;
                case Tables.RecordFirms:
                    sql += $"CountCascadeRecordFirms({selected})";
                    break;
                case Tables.RecordTypes:
                    sql += $"CountCascadeRecordTypes({selected})";
                    break;
                case Tables.Districts:
                    sql += $"CountCascadeDistricts({selected})";
                    break;
                case Tables.Owners:
                    sql += $"CountCascadeOwners({selected})";
                    break;
                case Tables.PropertyTypes:
                    sql += $"CountCascadePropertyTypes({selected})";
                    break;
                default:
                    break;
            }

            int[] willBeDeleted;

            if (CurrentTable == Tables.ProductRanges)
            {
                willBeDeleted = new int[] { dataGridView1.SelectedRows.Count };
            }
            else
            {
                dt = new DataTable();

                try
                {
                    await conn.OpenAsync();
                    cmd = new NpgsqlCommand(sql, conn);
                    dt.Load(await cmd.ExecuteReaderAsync());
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }

                willBeDeleted = (int[])dt.Rows[0][0];
            }

            string message = "Вы уверены, что хотите удалить данные строки?\nБудет удалено строк:\n";

            switch (CurrentTable)
            {
                case Tables.ProductRanges:
                    message += $"\tАссортимент: {willBeDeleted[0]}";
                    break;
                case Tables.Albums:
                    message += $"\tАльбомы: {willBeDeleted[0]}\n\tАссортимент: {willBeDeleted[1]}";
                    break;
                case Tables.Shops:
                    message += $"\tМагазины: {willBeDeleted[0]}\n\tАссортимент: {willBeDeleted[1]}";
                    break;
                case Tables.Cities:
                    message += $"\tГорода: {willBeDeleted[0]}\n\tФирмы звукозаписи: {willBeDeleted[1]}\n\tАльбомы: {willBeDeleted[2]}\n\tАссортимент: {willBeDeleted[3]}";
                    break;
                case Tables.Countries:
                    message += $"\tСтраны: {willBeDeleted[0]}\n\tГорода: {willBeDeleted[1]}\n\tФирмы звукозаписи: {willBeDeleted[2]}\n\tАльбомы: {willBeDeleted[3]}\n\tАссортимент: {willBeDeleted[4]}";
                    break;
                case Tables.Executors:
                    message += $"\tИсполнители: {willBeDeleted[0]}\n\tАльбомы: {willBeDeleted[1]}\n\tАссортимент: {willBeDeleted[2]}";
                    break;
                case Tables.Genres:
                    message += $"\tЖанры: {willBeDeleted[0]}\n\tАльбомы: {willBeDeleted[1]}\n\tАссортимент: {willBeDeleted[2]}";
                    break;
                case Tables.Languages:
                    message += $"\tЯзыки: {willBeDeleted[0]}\n\tАльбомы: {willBeDeleted[1]}\n\tАссортимент: {willBeDeleted[2]}";
                    break;
                case Tables.RecordFirms:
                    message += $"\tФирмы звукозаписи: {willBeDeleted[0]}\n\tАльбомы: {willBeDeleted[1]}\n\tАссортимент: {willBeDeleted[2]}";
                    break;
                case Tables.RecordTypes:
                    message += $"\tТипы записи: {willBeDeleted[0]}\n\tАльбомы: {willBeDeleted[1]}\n\tАссортимент: {willBeDeleted[2]}";
                    break;
                case Tables.Districts:
                    message += $"\tРайоны: {willBeDeleted[0]}\n\tМагазины: {willBeDeleted[1]}\n\tАссортимент: {willBeDeleted[2]}";
                    break;
                case Tables.Owners:
                    message += $"\tВладельцы: {willBeDeleted[0]}\n\tМагазины: {willBeDeleted[1]}\n\tАссортимент: {willBeDeleted[2]}";
                    break;
                case Tables.PropertyTypes:
                    message += $"\tТипы собственности: {willBeDeleted[0]}\n\tМагазины: {willBeDeleted[1]}\n\tАссортимент: {willBeDeleted[2]}";
                    break;
            }

            if (MessageBox.Show(message, "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                sql = "DELETE FROM ";
                switch (CurrentTable)
                {
                    case Tables.ProductRanges:
                        sql += $"ProductRanges WHERE productRange_id = ANY({selected})";
                        break;
                    case Tables.Albums:
                        sql += $"Albums WHERE album_id = ANY({selected})";
                        break;
                    case Tables.Shops:
                        sql += $"Shops WHERE shop_id = ANY({selected})";
                        break;
                    case Tables.Cities:
                        sql += $"Cities WHERE city_id = ANY({selected})";
                        break;
                    case Tables.Countries:
                        sql += $"Countries WHERE country_id = ANY({selected})";
                        break;
                    case Tables.Executors:
                        sql += $"Executors WHERE executor_id = ANY({selected})";
                        break;
                    case Tables.Genres:
                        sql += $"Genres WHERE genre_id = ANY({selected})";
                        break;
                    case Tables.Languages:
                        sql += $"Languages WHERE language_id = ANY({selected})";
                        break;
                    case Tables.RecordFirms:
                        sql += $"RecordFirms WHERE recordfirm_id = ANY({selected})";
                        break;
                    case Tables.RecordTypes:
                        sql += $"RecordTypes WHERE recordtype_id = ANY({selected})";
                        break;
                    case Tables.Districts:
                        sql += $"Districts WHERE district_id = ANY({selected})";
                        break;
                    case Tables.Owners:
                        sql += $"Owners WHERE owner_id = ANY({selected})";
                        break;
                    case Tables.PropertyTypes:
                        sql += $"PropertyTypes WHERE propertytype_id = ANY({selected})";
                        break;
                }

                try
                {
                    await conn.OpenAsync();
                    cmd = new NpgsqlCommand(sql, conn);
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
                updator.Invoke();
            }
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageBox.Show("Выделите только одну строку");
                return;
            }

            DialogResult dialogResult;

            switch (CurrentTable)
            {
                case Tables.ProductRanges:
                    dialogResult = new Add.AddProductRange(connectString, dataGridView1.SelectedRows[0]).ShowDialog(this);
                    break;
                case Tables.Albums:
                    dialogResult = new Add.AddAlbum(connectString, dataGridView1.SelectedRows[0]).ShowDialog(this);
                    break;
                case Tables.Shops:
                    dialogResult = new Add.AddShop(connectString, dataGridView1.SelectedRows[0]).ShowDialog(this);
                    break;
                case Tables.Cities:
                    dialogResult = new Add.AddCity(connectString, dataGridView1.SelectedRows[0]).ShowDialog(this);
                    break;
                case Tables.Countries:
                    dialogResult = new Add.AddCountry(connectString, dataGridView1.SelectedRows[0]).ShowDialog(this);
                    break;
                case Tables.Executors:
                    dialogResult = new Add.AddExecutor(connectString, dataGridView1.SelectedRows[0]).ShowDialog(this);
                    break;
                case Tables.Genres:
                    dialogResult = new Add.AddGenre(connectString, dataGridView1.SelectedRows[0]).ShowDialog(this);
                    break;
                case Tables.Languages:
                    dialogResult = new Add.AddLanguage(connectString, dataGridView1.SelectedRows[0]).ShowDialog(this);
                    break;
                case Tables.RecordFirms:
                    dialogResult = new Add.AddRecordFirm(connectString, dataGridView1.SelectedRows[0]).ShowDialog(this);
                    break;
                case Tables.RecordTypes:
                    dialogResult = new Add.AddRecordType(connectString, dataGridView1.SelectedRows[0]).ShowDialog(this);
                    break;
                case Tables.Districts:
                    dialogResult = new Add.AddDistrict(connectString, dataGridView1.SelectedRows[0]).ShowDialog(this);
                    break;
                case Tables.Owners:
                    dialogResult = new Add.AddOwner(connectString, dataGridView1.SelectedRows[0]).ShowDialog(this);
                    break;
                case Tables.PropertyTypes:
                    dialogResult = new Add.AddPropertyType(connectString, dataGridView1.SelectedRows[0]).ShowDialog(this);
                    break;
                default:
                    return;
            }

            if (dialogResult == DialogResult.OK)
                updator.Invoke();
        }

        #endregion

        #region Find
        public bool AbleToFind
        {
            get
            {
                return обновитьToolStripMenuItem.Enabled;
            }
        }
        private void поискToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in OwnedForms)
            {
                if (form.GetType() == typeof(Find))
                {
                    form.Close();
                    return;
                }
            }
            new Find(connectString).Show(this);
        }

        public void ViewSearchResult(ref DataTable dt)
        {
            dataGridView1.DataSource = null; //очистка таблицы
            dataGridView1.Columns.Clear();
            if (CurrentTable == Tables.Albums)
            {
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

                    string base64String = Encoding.Default.GetString((byte[])dataRow[12]);
                    byte[] byteArray = Convert.FromBase64String(base64String);

                    using (var ms = new MemoryStream(byteArray))
                    {
                        var img = Image.FromStream(ms);
                        row[12] = img;
                    }
                    row[row.ItemArray.Length - 1] = dataRow[row.ItemArray.Length - 1];

                    dtCloned.Rows.Add(row);
                }
                dataGridView1.RowTemplate.Height = 50;

                dataGridView1.DataSource = null; //очистка таблицы

                dataGridView1.DataSource = dtCloned;
                dataGridView1.Columns[11].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            else
            {
                dataGridView1.DataSource = dt;
            }
            updator_continue.Invoke();
        }

        #endregion

        #region Styles

        readonly Color mouseOver = Color.FromArgb(230, 230, 230);
        readonly Color mouseOverSelected = Color.FromArgb(102, 145, 178);
        readonly Color mouseLeftSelected = Color.FromArgb(153, 181, 204);

        int mouseOverCell_index = 0;

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
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
            catch (Exception)
            {

            }

        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
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
            catch (Exception)
            {

            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows != null & dataGridView1.SelectedRows.Count > 0)
                toolStripStatusLabel4.Text = (dataGridView1.SelectedRows[0].Index + 1).ToString();
            toolStripStatusLabel6.Text = dataGridView1.SelectedRows.Count.ToString();
            try
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
            catch (Exception)
            {

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
            toolStripStatusLabel2.Text = dataGridView1.RowCount.ToString();
        }

        private int[] GetWidths(DataTable dt, Font font)
        {
            int[] widths = new int[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                widths[i] = (from DataRow row in dt.AsEnumerable().AsParallel()
                             select TextRenderer.MeasureText(row[i].ToString(), font).Width
                             ).Max();
                var header = TextRenderer.MeasureText(dt.Columns[i].ColumnName, font).Width;
                widths[i] = widths[i] > header ? widths[i] : header;
                widths[i] = dt.Columns[i].ColumnName == "id" ? widths[i] + 1 : widths[i];

                //widths[i] = (from DataRow row in dt.AsEnumerable().AsParallel()
                //             select row[i].ToString().Length).Max();
            }
            return widths;
        }

        private delegate void toSetWidths_delegate(int[] a);
        private void SetColumnWidths(Task<int[]> x)
        {
            dataGridView1.Invoke(new toSetWidths_delegate((array) =>
            {
                for (int i = 0; i < array.Length; i++)
                {
                    dataGridView1.Columns[i].Width = array[i];
                }
            }), x.Result);
        }

        #endregion

        #region Requests

        private async void суммарныйДоходИсполнителяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Requests.Get_time_period("Введите период");
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                поискToolStripMenuItem.Enabled = false;
                обновитьToolStripMenuItem.Enabled = false;
                данныеToolStripMenuItem.Enabled = false;
                string vs1 = form.timePeriod.Start.ToString("dd-MM-yyyy");
                string vs2 = form.timePeriod.End.ToString("dd-MM-yyyy");

                try
                {
                    await conn.OpenAsync();

                    cmd = new NpgsqlCommand("SELECT executors.executor_id AS \"id\", " +
                        "executors.executor_name AS \"Исполнитель\", " +
                        "sum(productranges.amount) AS \"Прибыль\", " +
                        "count(distinct(albums.album_name)) AS \"Количеств альбомов\" FROM executors " +
                        "JOIN albums ON albums.executor_id = executors.executor_id " +
                        "OR albums.albumInfo LIKE '%' || executors.executor_name || '%' " +
                        "JOIN productranges ON productranges.album_id = albums.album_id " +
                        $"WHERE(productranges.dateofreceipt > '{vs1}' " +
                        $"AND productranges.dateofreceipt < '{vs2}') " +
                        "GROUP BY executors.executor_id ORDER BY \"Прибыль\" DESC", conn);
                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());

                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                    updator_continue.Invoke();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
        private async void вОпределенномРайонеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Requests.Get_district_id(connectString);

            if (form.ShowDialog() == DialogResult.OK)
            {
                поискToolStripMenuItem.Enabled = false;
                обновитьToolStripMenuItem.Enabled = false;
                данныеToolStripMenuItem.Enabled = false;
                try
                {
                    await conn.OpenAsync();
                    cmd = new NpgsqlCommand(
                        "SELECT genres.genre_name AS \"Жанр\", " +
                        "count(albums.album_id) AS \"Количество альбомов на район\" " +
                        "FROM shops " +
                        "JOIN districts ON shops.district_id = districts.district_id " +
                        "JOIN productranges ON shops.shop_id = productranges.shop_id " +
                        "JOIN albums ON albums.album_id = productranges.album_id " +
                        "JOIN genres ON genres.genre_id = albums.genre_id " +
                        $"WHERE districts.district_id = {form.District_id} " +
                        "GROUP BY genres.genre_id " +
                        "ORDER BY genres.genre_name; ",
                        conn);
                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    updator_continue.Invoke();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
        private async void поВсемМагазинамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            поискToolStripMenuItem.Enabled = false;
            обновитьToolStripMenuItem.Enabled = false;
            данныеToolStripMenuItem.Enabled = false;
            try
            {
                await conn.OpenAsync();

                cmd = new NpgsqlCommand("SELECT * FROM count_genres", conn);

                dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;
                updator_continue.Invoke();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        private async void поВсемМагазинамToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            поискToolStripMenuItem.Enabled = false;
            обновитьToolStripMenuItem.Enabled = false;
            данныеToolStripMenuItem.Enabled = false;
            try
            {
                await conn.OpenAsync();

                cmd = new NpgsqlCommand("SELECT * FROM Count_Top3_Genres LIMIT 3", conn);

                dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;
                updator_continue.Invoke();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        private async void вОпределенномМагазинеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Requests.Get_shop_id(connectString);

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                поискToolStripMenuItem.Enabled = false;
                обновитьToolStripMenuItem.Enabled = false;
                данныеToolStripMenuItem.Enabled = false;
                try
                {
                    await conn.OpenAsync();

                    cmd = new NpgsqlCommand(
                        "SELECT genres.genre_name AS \"Жанр\", " +
                        "sum(ProductRanges.amount) AS \"Количество проданных копий\" " +
                        "FROM shops " +
                        "JOIN districts ON shops.district_id = districts.district_id " +
                        "JOIN productranges ON shops.shop_id = productranges.shop_id " +
                        "JOIN albums ON albums.album_id = productranges.album_id " +
                        "JOIN genres ON genres.genre_id = albums.genre_id " +
                        $"WHERE shops.shop_id = {form.Shop_id} " +
                        "GROUP BY genres.genre_id " +
                        "ORDER BY \"Количество проданных копий\" DESC " +
                        "LIMIT 3; ",
                        conn);

                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    updator_continue.Invoke();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
        private async void списокАльбомовКоторыеПродаютсяУДанногоВладельцаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Requests.Get_owner_id(connectString);

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                поискToolStripMenuItem.Enabled = false;
                обновитьToolStripMenuItem.Enabled = false;
                данныеToolStripMenuItem.Enabled = false;
                try
                {
                    await conn.OpenAsync();

                    cmd = new NpgsqlCommand(
                        "SELECT distinct(albums.album_id) AS \"id\", " +
                        "albums.album_name AS \"Альбом\" " +
                        "FROM owners " +
                        "JOIN shops ON shops.owner_id = owners.owner_id " +
                        "JOIN productranges ON productranges.shop_id = shops.shop_id " +
                        "JOIN albums ON albums.album_id = productranges.album_id " +
                        $"WHERE owners.owner_id = {form.Owner_id}",
                        conn);

                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    updator_continue.Invoke();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
        private async void всеАльбомыКоторыеВыпущеныВДаннойСтранеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Requests.Get_country_id(connectString);

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                поискToolStripMenuItem.Enabled = false;
                обновитьToolStripMenuItem.Enabled = false;
                данныеToolStripMenuItem.Enabled = false;
                try
                {
                    await conn.OpenAsync();

                    cmd = new NpgsqlCommand(
                        "SELECT albums.album_id AS \"id\", " +
                        "albums.album_name AS \"Альбом\" " +
                        "FROM countries " +
                        "JOIN cities USING(country_id) " +
                        "JOIN recordfirms USING(city_id) " +
                        "JOIN albums USING(recordfirm_id) " +
                        $"WHERE country_id = {form.Country_id}",
                        conn);

                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    updator_continue.Invoke();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
        private async void всеАльбомыКоторыеПоставленыВОпределенныйМагазинПослеОпределеннойДатыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Requests.Get_time("Выбрать дату");
            var shop = new Requests.Get_shop_id(connectString);

            if (form.ShowDialog(this) == DialogResult.OK && shop.ShowDialog(this) == DialogResult.OK)
            {
                поискToolStripMenuItem.Enabled = false;
                обновитьToolStripMenuItem.Enabled = false;
                данныеToolStripMenuItem.Enabled = false;
                try
                {
                    await conn.OpenAsync();
                    var vs = form.Time.ToString("dd-MM-yyyy");
                    cmd = new NpgsqlCommand(
                        "SELECT album_id AS \"id\", " +
                        "album_name AS \"Альбом\", " +
                        "dateOfReceipt AS \"Дата поступления\" " +
                        "FROM shops " +
                        "JOIN productranges USING(shop_id) " +
                        "JOIN albums USING(album_id) " +
                        $"WHERE shop_id = {shop.Shop_id} " +
                        $"AND dateOfReceipt > '{vs}'" +
                        "ORDER BY dateOfReceipt DESC",
                        conn);

                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    updator_continue.Invoke();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
        private async void датаПоследнейПоставкиВоВсеМагазиныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            поискToolStripMenuItem.Enabled = false;
            обновитьToolStripMenuItem.Enabled = false;
            данныеToolStripMenuItem.Enabled = false;
            try
            {
                await conn.OpenAsync();

                cmd = new NpgsqlCommand("SELECT * FROM Last_delivery", conn);

                dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;
                updator_continue.Invoke();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        private async void вывестиСписокФирмЗвукозаписиСоСтранамиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            поискToolStripMenuItem.Enabled = false;
            обновитьToolStripMenuItem.Enabled = false;
            данныеToolStripMenuItem.Enabled = false;
            try
            {
                await conn.OpenAsync();

                cmd = new NpgsqlCommand("SELECT * FROM Recordfirm_Country", conn);

                dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;
                updator_continue.Invoke();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        private async void вывестиСписокАльбомовСЖанрамиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            поискToolStripMenuItem.Enabled = false;
            обновитьToolStripMenuItem.Enabled = false;
            данныеToolStripMenuItem.Enabled = false;
            try
            {
                await conn.OpenAsync();

                cmd = new NpgsqlCommand("SELECT * FROM Albums_Genres", conn);

                dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;
                updator_continue.Invoke();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        private async void вывестиСписокМагазиновСТипамиСобственностиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            поискToolStripMenuItem.Enabled = false;
            обновитьToolStripMenuItem.Enabled = false;
            данныеToolStripMenuItem.Enabled = false;
            try
            {
                await conn.OpenAsync();

                cmd = new NpgsqlCommand("SELECT * FROM Shops_PropertyTypes", conn);

                dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;
                updator_continue.Invoke();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        private async void списокСтранБезГородовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            поискToolStripMenuItem.Enabled = false;
            обновитьToolStripMenuItem.Enabled = false;
            данныеToolStripMenuItem.Enabled = false;
            try
            {
                await conn.OpenAsync();

                cmd = new NpgsqlCommand("SELECT * FROM CountriesWithoutCities", conn);

                dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;
                updator_continue.Invoke();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        private async void списокАльбомовУКоторыхОдинИсполнительToolStripMenuItem_Click(object sender, EventArgs e)
        {
            поискToolStripMenuItem.Enabled = false;
            обновитьToolStripMenuItem.Enabled = false;
            данныеToolStripMenuItem.Enabled = false;
            try
            {
                await conn.OpenAsync();

                cmd = new NpgsqlCommand("SELECT * FROM Albums_one_executor", conn);

                dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;
                updator_continue.Invoke();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        private async void количествоАльбомовВКаждомМагазинеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            поискToolStripMenuItem.Enabled = false;
            обновитьToolStripMenuItem.Enabled = false;
            данныеToolStripMenuItem.Enabled = false;
            try
            {
                await conn.OpenAsync();

                cmd = new NpgsqlCommand("SELECT * FROM shop_album_count", conn);

                dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;
                updator_continue.Invoke();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        private async void среднееКоличетвоАльбомовНаМагазиныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            поискToolStripMenuItem.Enabled = false;
            обновитьToolStripMenuItem.Enabled = false;
            данныеToolStripMenuItem.Enabled = false;
            try
            {
                await conn.OpenAsync();

                cmd = new NpgsqlCommand("SELECT * FROM avg_album_count", conn);

                dt = new DataTable();
                dt.Columns.Add("Среднее количество альбомов");
                dt.Rows.Add(Decimal.ToDouble((decimal)await cmd.ExecuteScalarAsync()));

                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();

                dataGridView1.DataSource = dt;
                updator_continue.Invoke();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        private async void всегоАльбомовВТомЧислеВыпущенныеТиражомБолее10ккToolStripMenuItem_Click(object sender, EventArgs e)
        {
            поискToolStripMenuItem.Enabled = false;
            обновитьToolStripMenuItem.Enabled = false;
            данныеToolStripMenuItem.Enabled = false;
            try
            {
                await conn.OpenAsync();

                cmd = new NpgsqlCommand("SELECT * FROM albums_count_more_10kk", conn);

                dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;
                updator_continue.Invoke();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        private async void количествоАльбомовПоГородамВВыбраннойСтранеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Requests.Get_country_id(connectString);

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                поискToolStripMenuItem.Enabled = false;
                обновитьToolStripMenuItem.Enabled = false;
                данныеToolStripMenuItem.Enabled = false;
                try
                {
                    await conn.OpenAsync();

                    cmd = new NpgsqlCommand(
                        "SELECT city_name AS \"Город\", " +
                        "count(albums.*) AS \"Количество\" " +
                        "FROM countries " +
                        "JOIN cities USING(country_id) " +
                        "JOIN recordfirms USING(city_id) " +
                        "JOIN albums USING(recordfirm_id) " +
                        $"WHERE country_id = {form.Country_id} " +
                        "GROUP BY city_id " +
                        "ORDER BY \"Количество\" DESC",
                        conn);

                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    updator_continue.Invoke();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
        private async void количествоФирмЗвукозаписиВГородахКоторыеНачинаютсяНаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Requests.Get_substring_id();

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                поискToolStripMenuItem.Enabled = false;
                обновитьToolStripMenuItem.Enabled = false;
                данныеToolStripMenuItem.Enabled = false;
                try
                {
                    await conn.OpenAsync();

                    cmd = new NpgsqlCommand(
                        "SELECT city_name AS \"Город\", " +
                        "count(recordfirms.*) AS \"Количество\" " +
                        "FROM cities " +
                        "JOIN recordfirms USING(city_id) " +
                        $"WHERE lower(city_name) LIKE '{form.Result}%' " +
                        "GROUP BY city_id " +
                        "ORDER BY \"Количество\" DESC",
                        conn);

                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    updator_continue.Invoke();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
        private async void списокМагазиновВКоторыхПродаетсяБольшеЧемЖанраToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Requests.Get_genre_count(connectString);

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                поискToolStripMenuItem.Enabled = false;
                обновитьToolStripMenuItem.Enabled = false;
                данныеToolStripMenuItem.Enabled = false;
                try
                {
                    await conn.OpenAsync();

                    cmd = new NpgsqlCommand(
                        "SELECT shop_name AS \"Магазин\", " +
                        "count(distinct(album_id)) AS \"Количество альбомов этого жанра\" " +
                        "FROM shops " +
                        "JOIN productranges USING(shop_id) " +
                        "JOIN albums USING(album_id) " +
                        $"WHERE genre_id = {form.Genre_id} " +
                        "GROUP BY shop_id " +
                        $"HAVING count(*) > {form.Count}",
                        conn);

                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    updator_continue.Invoke();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
        private async void списокЖанровВКоторыхЧислоАльбомовСНазваниемДлиннееБолееToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Requests.Get_length_count();

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                поискToolStripMenuItem.Enabled = false;
                обновитьToolStripMenuItem.Enabled = false;
                данныеToolStripMenuItem.Enabled = false;
                try
                {
                    await conn.OpenAsync();

                    cmd = new NpgsqlCommand(
                        "SELECT genre_name AS \"Жанр\", " +
                        "count(album_id) AS \"Количество альбомов\" " +
                        "FROM genres " +
                        "JOIN albums USING(genre_id) " +
                        $"WHERE char_length(album_name) >= {form.Length} " +
                        "GROUP BY genre_name " +
                        $"HAVING count(*) >= {form.Count}",
                        conn);

                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    updator_continue.Invoke();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
        private async void количествоАльбомовПоЯзыкамНаписанныеВОпределеннойСтранеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Requests.Get_country_id(connectString);

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                поискToolStripMenuItem.Enabled = false;
                обновитьToolStripMenuItem.Enabled = false;
                данныеToolStripMenuItem.Enabled = false;
                try
                {
                    await conn.OpenAsync();

                    cmd = new NpgsqlCommand(
                        "SELECT language_name AS \"Язык\", " +
                        "count(album_id) AS \"Количество альбомов\" " +
                        "FROM languages " +
                        "JOIN albums USING(language_id) " +
                        "WHERE album_id IN( " +
                        "    SELECT album_id FROM countries " +
                        "    JOIN cities USING(country_id) " +
                        "    JOIN recordfirms USING(city_id) " +
                        "    JOIN albums USING(recordfirm_id) " +
                        $"    WHERE country_id = {form.Country_id} " +
                        ") " +
                        "GROUP BY language_name " +
                        "ORDER BY \"Количество альбомов\" DESC",
                        conn);

                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    updator_continue.Invoke();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
        private async void получитьКоличествоЗаписейВТаблицахToolStripMenuItem_Click(object sender, EventArgs e)
        {
            поискToolStripMenuItem.Enabled = false;
            обновитьToolStripMenuItem.Enabled = false;
            данныеToolStripMenuItem.Enabled = false;
            try
            {
                await conn.OpenAsync();

                cmd = new NpgsqlCommand("SELECT * FROM get_all_count", conn);

                dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;
                updator_continue.Invoke();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        private async void всеМагазиныКоторыеПродаютАльбомыСЭтимТипомЗаписиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Requests.Get_recordtype_id(connectString);

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                поискToolStripMenuItem.Enabled = false;
                обновитьToolStripMenuItem.Enabled = false;
                данныеToolStripMenuItem.Enabled = false;
                try
                {
                    await conn.OpenAsync();

                    cmd = new NpgsqlCommand(
                        "SELECT DISTINCT(shop_id) AS \"id\", " +
                        "shop_name AS \"Магазин\" " +
                        "FROM shops " +
                        "JOIN productranges USING(shop_id) " +
                        "WHERE album_id IN( " +
                        "    SELECT album_id FROM albums " +
                        $"    WHERE recordtype_id = {form.RecordType_id} " +
                        ") " +
                        "ORDER BY shop_id",
                        conn);

                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    updator_continue.Invoke();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
        private async void списокЯзыковКоторыеНеПродаютсяВЭтомМагазинеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Requests.Get_shop_id(connectString);

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                поискToolStripMenuItem.Enabled = false;
                обновитьToolStripMenuItem.Enabled = false;
                данныеToolStripMenuItem.Enabled = false;
                try
                {
                    await conn.OpenAsync();

                    cmd = new NpgsqlCommand(
                        "SELECT language_id AS \"id\", " +
                        "language_name AS \"Язык\" " +
                        "FROM languages " +
                        "WHERE language_id NOT IN( " +
                        "    SELECT DISTINCT(language_id) " +
                        "    FROM productranges " +
                        "    JOIN albums USING(album_id) " +
                        $"    WHERE shop_id = {form.Shop_id} " +
                        ") " +
                        "ORDER BY language_id",
                        conn);

                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    updator_continue.Invoke();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
        private async void магазиныСКоличествомАльбомовВСравненииСЧисломToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Requests.Get_number_to_compare();

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                поискToolStripMenuItem.Enabled = false;
                обновитьToolStripMenuItem.Enabled = false;
                данныеToolStripMenuItem.Enabled = false;
                try
                {
                    await conn.OpenAsync();

                    cmd = new NpgsqlCommand(
                        "SELECT shop_id AS \"id\", " +
                        "shop_name AS \"Магазин\", " +
                        "CASE " +
                        $"    WHEN count(*) > {form.Value} THEN 'больше {form.Value}' " +
                        $"    WHEN count(*) < {form.Value} THEN 'меньше {form.Value}' " +
                        $"    ELSE 'равно {form.Value}' " +
                        "END AS \"Количество альбомов\" " +
                        "FROM shops " +
                        "JOIN productranges USING(shop_id) " +
                        "GROUP BY shop_id " +
                        "ORDER BY shop_id",
                        conn);

                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dt;
                    updator_continue.Invoke();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
        private async void осталосьСвободныхАльбомовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            поискToolStripMenuItem.Enabled = false;
            обновитьToolStripMenuItem.Enabled = false;
            данныеToolStripMenuItem.Enabled = false;
            try
            {
                await conn.OpenAsync();

                cmd = new NpgsqlCommand("SELECT * FROM ostalos_albomov", conn);

                dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;
                updator_continue.Invoke();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        #endregion

        #region SaveToExcel

        Task savingToExcel_task;
        private void сохранитьВExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sf = new SaveFileDialog()
            {
                Filter = "xslx files (*.xlsx)|*.xlsx",
                OverwritePrompt = true,
                RestoreDirectory = true
            };
            if (sf.ShowDialog() == DialogResult.OK)
            {
                var data = (dataGridView1.DataSource as DataTable).Copy();

                bool isImage = false;

                for (int i = 0; i < data.Columns.Count; i++)
                {
                    if (data.Columns[i].DataType == typeof(Image))
                    {
                        data.Columns.RemoveAt(i);
                        isImage = true;
                    }
                }

                if (isImage &&
                    MessageBox.Show(
                        "Невозможно сохранить колонку с изображением\n" +
                        "Продолжить без сохранения столбца с изображением?",
                        "Содержится изображение",
                        MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                savingToExcel_task = Task.Factory.StartNew((ds) =>
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var vs = workbook.Worksheets.Add(ds as DataTable, "sheet");

                        vs.Columns().AdjustToContents();


                        workbook.SaveAs(sf.FileName);
                    }
                }, data);
            }
        }

        #endregion

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (savingToExcel_task != null &&
                savingToExcel_task.Status == TaskStatus.Running)
                await savingToExcel_task;
        }

        private void составнаяФормаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in OwnedForms)
            {
                if (form.GetType() == typeof(CompoundForm))
                {
                    form.Close();
                    return;
                }
            }
            new CompoundForm(connectString).Show(this);
        }
    }
}