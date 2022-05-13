using System;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Forms.Add
{
    public partial class AddPropertyType : Form
    {
        private readonly string connectString;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private readonly DataGridViewRow row;

        public AddPropertyType(string connString, DataGridViewRow row = null)
        {
            this.InitializeComponent();
            connectString = connString;
            this.row = row;
        }

        private void AddPropertyType_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);
            if (row != null)
            {
                textBox1.Text = row.Cells["Тип собственности"].Value.ToString();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Длина названия типа собственности должна быть больше 0");
                return;
            }

            try
            {
                conn.Open();

                if (row == null)
                {
                    cmd = new NpgsqlCommand($"INSERT INTO PropertyTypes (propertyType_name) VALUES ('{textBox1.Text}');", conn);
                }
                else
                {
                    int mId = (int)row.Cells["id"].Value;
                    cmd = new NpgsqlCommand($"UPDATE PropertyTypes SET propertyType_name = '{textBox1.Text}' WHERE propertytype_id = {mId}", conn);
                }
                if (row == null)
                    MessageBox.Show("Тип собственности успешно добавлен");
                else
                    MessageBox.Show("Тип собственности успешно изменен");
                await cmd.ExecuteNonQueryAsync();
                DialogResult = DialogResult.OK;

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
    }
}
