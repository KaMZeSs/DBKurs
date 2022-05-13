using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Forms.Add
{
    public partial class AddRecordFirm : Form
    {
        private readonly string connectString;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private DataTable dt;
        private DataGridViewRow row;

        public AddRecordFirm(string connString, DataGridViewRow row = null)
        {
            this.InitializeComponent();
            connectString = connString;
            this.row = row;
        }

        private async void AddRecordFirm_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);

            try
            {
                conn.Open();

                cmd = new NpgsqlCommand($"SELECT * FROM Cities", conn);

                dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());

                listBox1.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    listBox1.Items.Add(dt.Rows[i][2] + " - " + dt.Rows[i][0]);
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
                listBox1.SelectedIndex = listBox1.FindString(row.Cells["Город"].Value.ToString() + " - ");
                textBox1.Text = row.Cells["Фирма звукозаписи"].Value.ToString();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Длина названия фирмы должна быть больше 0");
                return;
            }
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите город, в котором находится данная фирма\nЕсли данного города нет - создайте его, нажав на кнопку\n\"Добавить новый город\"");
                return;
            }

            try
            {
                conn.Open();

                string[] temp = listBox1.SelectedItem.ToString().Split('-');
                int id = Int32.Parse(temp.Last());

                if (row == null)
                {
                    cmd = new NpgsqlCommand($"INSERT INTO RecordFirms (city_id, recordFirm_name) VALUES ({id}, '{textBox1.Text}');", conn);
                }
                else
                {
                    int mId = (int)row.Cells["id"].Value;
                    cmd = new NpgsqlCommand($"UPDATE RecordFirms SET city_id = {id}, recordFirm_name = '{textBox1.Text}' WHERE recordFirm_id = {mId}", conn);
                }

                await cmd.ExecuteNonQueryAsync();
                DialogResult = DialogResult.OK;
                if (row == null)
                    MessageBox.Show("Фирма звукозаписи успешно добавлена");
                else
                    MessageBox.Show("Фирма звукозаписи успешно изменена");
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
            this.Close();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (new AddCountry(connectString).ShowDialog() == DialogResult.OK)
            {
                try
                {
                    conn.Open();

                    cmd = new NpgsqlCommand($"SELECT * FROM Cities", conn);

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
