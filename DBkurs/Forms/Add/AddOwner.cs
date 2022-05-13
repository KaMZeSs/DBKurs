using System;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Forms.Add
{
    public partial class AddOwner : Form
    {
        private readonly string connectString;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private readonly DataGridViewRow row;

        public AddOwner(string connString, DataGridViewRow row = null)
        {
            this.InitializeComponent();
            connectString = connString;
            this.row = row;
        }

        private void AddOwner_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);
            if (row != null)
            {
                textBox1.Text = row.Cells["Владелец"].Value.ToString();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Длина ФИО владельца должна быть больше 0");
                return;
            }

            if (textBox1.Text.Split(' ').Length < 3)
            {
                MessageBox.Show("ФИО владельца должно содержать по крайней мере 3 слова");
                return;
            }

            try
            {
                conn.Open();

                if (row == null)
                {
                    cmd = new NpgsqlCommand($"INSERT INTO Owners (owner_name) VALUES ('{textBox1.Text}');", conn);
                }
                else
                {
                    int mId = (int)row.Cells["id"].Value;
                    cmd = new NpgsqlCommand($"UPDATE Owners SET owner_name = '{textBox1.Text}' WHERE owner_id = {mId}", conn);
                }

                await cmd.ExecuteNonQueryAsync();
                DialogResult = DialogResult.OK;
                if (row == null)
                    MessageBox.Show("Владелец успешн добавлен");
                else
                    MessageBox.Show("Владелец успешно изменен");
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
