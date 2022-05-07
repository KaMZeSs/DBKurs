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
    public partial class AddGenre : Form
    {
        private readonly String connectString;
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;
        private DataGridViewRow row;

        public AddGenre(String connString, DataGridViewRow row = null)
        {
            InitializeComponent();
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
                this.DialogResult = DialogResult.OK;

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
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
