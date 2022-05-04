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
    public partial class AddCity : Form
    {
        private readonly String connectString;
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;
        private DataTable;

        public AddCity(String connString)
        {
            InitializeComponent();
            connectString = connString;
        }

        private async void AddCity_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);

            try
            {
                conn.Open();

                cmd = new NpgsqlCommand($"SELECT country_name FROM Countries", conn);

                DataTable dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());
                listBox1.DataSource = dt;
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
                MessageBox.Show("Длина названия города должна быть больше 0");
                return;
            }
            if (listBox1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите страну, в которой находится данный город\nЕсли данной страны нет - создайте ее, нажав на кнопку\n\"Добавить новую страну\"");
                return;
            }

            try
            {
                conn.Open();

                cmd = new NpgsqlCommand($"SELECT country_id FROM Countries WHERE Countries.country_name = '{listBox1.Text}'", conn);

                int id = (int) await cmd.ExecuteScalarAsync();

                cmd = new NpgsqlCommand($"INSERT INTO Cities (country_id, city_name) VALUES ({id}, '{textBox1.Text}');", conn);
                await cmd.ExecuteNonQueryAsync();
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("Город успешно добавлен");
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
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (new AddCountry(connectString).ShowDialog() == DialogResult.OK)
            {
                try
                {
                    conn.Open();

                    cmd = new NpgsqlCommand($"SELECT country_name FROM Countries", conn);

                    DataTable dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    listBox1.DataSource = dt;
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
