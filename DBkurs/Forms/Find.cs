using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Forms
{
    public partial class Find : Form
    {
        private readonly string connectString;
        private readonly NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private DataTable dt;

        Tables CurrentTable
        {
            get
            {
                return CurrentTable;
            }
            set
            {
                comboBox1.Items.Clear();
                end_of_comand = null;

                string[] items;
                switch (value)
                {
                    case Tables.ProductRanges:
                        items = new string[] { "Альбом", "Магазин", "Дата поступления", "Количество единиц" };
                        start_of_comand = "SELECT * FROM get_all_productranges()";
                        break;
                    case Tables.Albums:
                        items = new string[] { "Название альбома", "Фирма звукозаписи", "Дата выпуска альбома",
                            "Тираж альбома", "Количество песен", "Тип записи", "Исполнитель", "Жанр",
                            "Язык исполнения", "Время звучания" };
                        start_of_comand = "SELECT * FROM get_all_albums()";
                        break;
                    case Tables.Shops:
                        items = new string[] { "Название магазина", "Район города", "Адрес", "Тип собственности",
                            "Лицензия", "Дата окончания лицензии", "Владелец", "Год открытия" };
                        start_of_comand = "SELECT * FROM get_all_shops()";
                        break;
                    case Tables.Cities:
                        items = new string[] { "Страна", "Город" };
                        start_of_comand = "SELECT * FROM get_all_cities()";
                        break;
                    case Tables.Countries:
                        items = new string[] { "Страна" };
                        start_of_comand = "SELECT * FROM get_all_countries()";
                        break;
                    case Tables.Executors:
                        items = new string[] { "Исполнитель" };
                        start_of_comand = "SELECT * FROM get_all_executors()";
                        break;
                    case Tables.Genres:
                        items = new string[] { "Жанр" };
                        start_of_comand = "SELECT * FROM get_all_genres()";
                        break;
                    case Tables.Languages:
                        items = new string[] { "Язык" };
                        start_of_comand = "SELECT * FROM get_all_languages()";
                        break;
                    case Tables.RecordFirms:
                        items = new string[] { "Город", "Фирма звукозаписи" };
                        start_of_comand = "SELECT * FROM get_all_recordfirms()";
                        break;
                    case Tables.RecordTypes:
                        items = new string[] { "Тип записи" };
                        start_of_comand = "SELECT * FROM get_all_recordtypes()";
                        break;
                    case Tables.Districts:
                        items = new string[] { "Район" };
                        start_of_comand = "SELECT * FROM get_all_districts()";
                        break;
                    case Tables.Owners:
                        items = new string[] { "Владелец" };
                        start_of_comand = "SELECT * FROM get_all_owners()";
                        break;
                    case Tables.PropertyTypes:
                        items = new string[] { "Тип собственности" };
                        start_of_comand = "SELECT * FROM get_all_propertytypes()";
                        break;
                    default:
                        items = null;
                        break;
                }
                comboBox1.Items.AddRange(items);
                comboBox1.SelectedIndex = 0;
            }
        }

        string start_of_comand;
        string end_of_comand;

        public Find(string connectString)
        {
            this.InitializeComponent();
            this.connectString = connectString;
            conn = new NpgsqlConnection(connectString);
        }

        private void Find_Load(object sender, EventArgs e)
        {
            var owner = Owner as MainForm;
            owner.onTable_Updated += this.Owner_onTable_Updated;
            CurrentTable = owner.CurrentTable;
        }

        private void Owner_onTable_Updated(object sender, MainForm.TableEventArgs e)
        {
            CurrentTable = e.NewTable;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString().ToLower().Contains("дата"))
            {
                textBox1.Visible = false;
                dateTimePicker1.Visible =
                dateTimePicker2.Visible =
                label2.Visible =
                label3.Visible = true;
            }
            else
            {
                textBox1.Visible = true;
                dateTimePicker1.Visible =
                dateTimePicker2.Visible =
                label2.Visible =
                label3.Visible = false;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!(Owner as MainForm).AbleToFind)
                return;

            if (textBox1.Visible & textBox1.Text.Length == 0)
            {
                MessageBox.Show("Введите данные, по которым необходимо провести поиск");
            }

            if (end_of_comand == null)
            {
                if (textBox1.Visible)
                {
                    if (comboBox1.SelectedItem.ToString() == "Исполнитель")
                    {
                        end_of_comand = $"WHERE (lower(\"{comboBox1.SelectedItem}\") LIKE '%{textBox1.Text.Replace("'", "''").ToLower()}%' OR lower(\"Информация\") LIKE '%{textBox1.Text.Replace("'", "''").ToLower()}%')";
                    }
                    else
                    {
                        end_of_comand = $"WHERE lower(\"{comboBox1.SelectedItem}\") LIKE '%{textBox1.Text.Replace("'", "''").ToLower()}%'";
                    }
                }
                else
                {
                    string timeStart = dateTimePicker1.Value.ToString("dd-MM-yyyy");
                    string timeEnd = dateTimePicker2.Value.ToString("dd-MM-yyyy");
                    end_of_comand = $"WHERE (\"{comboBox1.SelectedItem}\" >= '{timeStart}' AND \"{comboBox1.SelectedItem}\" <= '{timeEnd}')";
                }
            }
            else
            {
                DialogResult res = MessageBox.Show("Продолжить работу с предыдущим результатом?", "Сохранить результат?", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Yes)
                {
                    if (start_of_comand.Contains("WHERE"))
                        end_of_comand = end_of_comand.Replace("WHERE", "AND");

                    start_of_comand = $"{start_of_comand} {end_of_comand}";
                }
                if (res == DialogResult.Cancel)
                {
                    return;
                }
                if (res == DialogResult.No)
                {
                    if (textBox1.Visible)
                    {
                        if (comboBox1.SelectedItem.ToString() == "Исполнитель")
                        {
                            end_of_comand = $"WHERE (lower(\"{comboBox1.SelectedItem}\") LIKE '%{textBox1.Text.Replace("'", "''").ToLower()}%' OR lower(\"Информация\") LIKE '%{textBox1.Text.Replace("'", "''").ToLower()}%')";
                        }
                        else
                        {
                            end_of_comand = $"WHERE lower(\"{comboBox1.SelectedItem}\") LIKE '%{textBox1.Text.Replace("'", "''").ToLower()}%'";
                        }
                    }
                    else
                    {
                        string timeStart = dateTimePicker1.Value.ToString("dd-MM-yyyy");
                        string timeEnd = dateTimePicker2.Value.ToString("dd-MM-yyyy");
                        end_of_comand = $"WHERE (\"{comboBox1.SelectedItem}\" >= '{timeStart}' AND \"{comboBox1.SelectedItem}\" <= '{timeEnd}')";
                    }
                }
                else
                {
                    if (textBox1.Visible)
                    {
                        if (comboBox1.SelectedItem.ToString() == "Исполнитель")
                        {
                            end_of_comand = $"AND (lower(\"{comboBox1.SelectedItem}\") LIKE '%{textBox1.Text.Replace("'", "''").ToLower()}%' OR lower(\"Информация\") LIKE '%{textBox1.Text.Replace("'", "''").ToLower()}%')";
                        }
                        else
                        {
                            end_of_comand = $"AND lower(\"{comboBox1.SelectedItem}\") LIKE '%{textBox1.Text.Replace("'", "''").ToLower()}%'";
                        }
                    }
                    else
                    {
                        string timeStart = dateTimePicker1.Value.ToString("dd-MM-yyyy");
                        string timeEnd = dateTimePicker2.Value.ToString("dd-MM-yyyy");
                        end_of_comand = $"AND (\"{comboBox1.SelectedItem}\" >= '{timeStart}' AND \"{comboBox1.SelectedItem}\" <= '{timeEnd}')";
                    }
                }
            }

            dt = new DataTable();

            try
            {
                await conn.OpenAsync();
                cmd = new NpgsqlCommand($"{start_of_comand} {end_of_comand}", conn);
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

            (Owner as MainForm).ViewSearchResult(ref dt);

        }
    }
}
