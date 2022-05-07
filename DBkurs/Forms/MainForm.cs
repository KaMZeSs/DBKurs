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

    public enum Tables { ProductRanges, Albums, Shops, Cities, Countries, Executors, Genres, Languages, RecordFirms, RecordTypes, Districts, Owners, PropertyTypes }

    public partial class MainForm : Form
    {
        UpdateTable updator, updator_continue;

        private readonly static String connectString = "Host=localhost;Port=5432;User Id=postgres;Password=1310;Database=Kurs";
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;
        private DataTable dt;

        private Tables currentTable;

        #region Properties

        public Tables CurrentTable 
        {
            get
            {
                return currentTable;
            }
        }

        #endregion

        #region Events
        public class TableEventArgs
        {
            public Tables NewTable { get; }

            public TableEventArgs(Tables newTable)
            {
                this.NewTable = newTable;
            }
        }
        public delegate void On_Table_Updated(object sender, TableEventArgs e);

        public event On_Table_Updated onTable_Updated;

        #endregion

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
            if (conn.State == ConnectionState.Open)
            {
                MessageBox.Show("Дождитесь завершения операции");
                return;
            }
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

#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
            Task.Factory.StartNew(() =>
            {
                onTable_Updated?.Invoke(this, new TableEventArgs(currentTable));
            });
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен

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

            String selected = "'{" + String.Join(", ", ids) + "}'";

            sql = "SELECT * FROM ";

            switch (currentTable)
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

            if (currentTable == Tables.ProductRanges)
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

            String message = "Вы уверены, что хотите удалить данные строки?\nБудет удалено строк:\n";

            switch (currentTable)
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
                switch (currentTable)
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

            switch (currentTable)
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

        private void поискToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Find().Show(this);
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

        private void составнаяФормаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CompoundForm(connectString).ShowDialog(this);
        }
    }
}
