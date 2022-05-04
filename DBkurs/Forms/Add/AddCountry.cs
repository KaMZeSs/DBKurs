﻿using System;
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
    public partial class AddCountry : Form
    {
        private readonly String connectString;
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;

        public AddCountry(String connString)
        {
            InitializeComponent();
            connectString = connString;
        }

        private void AddCountry_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Длина названия страны должна быть больше 0");
                return;
            }

            try
            {
                conn.Open();

                cmd = new NpgsqlCommand($"INSERT INTO Countries (country_name) VALUES ('{textBox1.Text}');", conn);
                await cmd.ExecuteNonQueryAsync();
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("Страна успешно добавлена");
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
