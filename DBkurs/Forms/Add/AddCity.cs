using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Forms.Add
{
    public partial class AddCity : Form
    {
        private readonly string connectString;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private DataTable dt;
        private readonly DataGridViewRow row;

        public AddCity(string connString, DataGridViewRow row = null)
        {
            this.InitializeComponent();
            connectString = connString;
            this.row = row;
        }

        private async void AddCity_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);

            try
            {
                conn.Open();

                cmd = new NpgsqlCommand($"SELECT * FROM Countries", conn);

                dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());

                listBox1.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    listBox1.Items.Add(dt.Rows[i][1] + " - " + dt.Rows[i][0]);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                conn.Close();
            }

            if (row != null)
            {
                listBox1.SelectedIndex = listBox1.FindString(row.Cells["Страна"].Value.ToString() + " - ");
                textBox1.Text = row.Cells["Город"].Value.ToString();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Длина названия города должна быть больше 0");
                return;
            }
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите страну, в которой находится данный город\nЕсли данной страны нет - создайте ее, нажав на кнопку\n\"Добавить новую страну\"");
                return;
            }

            try
            {
                conn.Open();
                string[] temp = listBox1.SelectedItem.ToString().Split('-');
                int id = Int32.Parse(temp.Last());
                if (row == null)
                {
                    cmd = new NpgsqlCommand($"INSERT INTO Cities (country_id, city_name) VALUES ({id}, '{textBox1.Text}');", conn);
                }
                else
                {
                    int mId = (int)row.Cells["id"].Value;
                    cmd = new NpgsqlCommand($"UPDATE Cities SET country_id = {id}, city_name = '{textBox1.Text}' WHERE city_id = {mId}", conn);
                }

                await cmd.ExecuteNonQueryAsync();
                DialogResult = DialogResult.OK;
                if (row == null)
                    MessageBox.Show("Город успешно добавлен");
                else
                    MessageBox.Show("Город успешно изменен");
                this.Close();
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

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (new AddCountry(connectString).ShowDialog() == DialogResult.OK)
            {
                try
                {
                    conn.Open();

                    cmd = new NpgsqlCommand($"SELECT * FROM Countries", conn);

                    dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());

                    listBox1.Items.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        listBox1.Items.Add(dt.Rows[i][1] + " - " + dt.Rows[i][0]);
                    }
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
        }
    }
}
