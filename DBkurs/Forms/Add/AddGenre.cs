using System;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Forms.Add
{
    public partial class AddGenre : Form
    {
        private readonly string connectString;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private readonly DataGridViewRow row;

        public AddGenre(string connString, DataGridViewRow row = null)
        {
            this.InitializeComponent();
            connectString = connString;
            this.row = row;
        }

        private void AddGenre_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);

            if (row != null)
            {
                textBox1.Text = row.Cells["Жанр"].Value.ToString();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Длина названия жанра должна быть больше 0");
                return;
            }

            try
            {
                conn.Open();

                if (row == null)
                {
                    cmd = new NpgsqlCommand($"INSERT INTO Genres (genre_name) VALUES ('{textBox1.Text}');", conn);
                }
                else
                {
                    int mId = (int)row.Cells["id"].Value;
                    cmd = new NpgsqlCommand($"UPDATE Genres SET genre_name = '{textBox1.Text}' WHERE genre_id = {mId}", conn);
                }


                await cmd.ExecuteNonQueryAsync();
                DialogResult = DialogResult.OK;

                if (row == null)
                    MessageBox.Show("Жанр успешн добавлен");
                else
                    MessageBox.Show("Жанр успешно изменен");


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
