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
    public partial class AddOwner : Form
    {
        private readonly String connectString;
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;

        public AddOwner(String connString)
        {
            InitializeComponent();
            connectString = connString;
        }

        private void AddOwner_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);
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

                cmd = new NpgsqlCommand($"INSERT INTO Owners (owner_name) VALUES ('{textBox1.Text}');", conn);
                await cmd.ExecuteNonQueryAsync();
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("Владелец успешно добавлен");
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
