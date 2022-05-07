using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Forms.Add
{
    public partial class AddRecordFirm : Form
    {
        private readonly String connectString;
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;
        private DataTable dt;

        public AddRecordFirm(String connString)
        {
            InitializeComponent();
            connectString = connString;
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

                String[] temp = listBox1.SelectedItem.ToString().Split('-');
                int id = Int32.Parse(temp.Last());

                cmd = new NpgsqlCommand($"INSERT INTO RecordFirms (city_id, recordFirm_name) VALUES ({id}, '{textBox1.Text}');", conn);
                await cmd.ExecuteNonQueryAsync();
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("Фирма успешно добавлена");
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
            this.DialogResult = DialogResult.Cancel;
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
