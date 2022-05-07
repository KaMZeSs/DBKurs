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
    public partial class AddDistrict : Form
    {
        private readonly String connectString;
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;
        private DataGridViewRow row;

        public AddDistrict(String connString, DataGridViewRow row = null)
        {
            InitializeComponent();
            connectString = connString;
            this.row = row;

        }

        private void AddDistrict_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);
            if (row != null)
            {
                textBox1.Text = row.Cells["Район"].Value.ToString();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Длина названия района должна быть больше 0");
                return;
            }

            try
            {
                conn.Open();
                if (row == null)
                {
                    cmd = new NpgsqlCommand($"INSERT INTO Districts (district_name) VALUES ('{textBox1.Text}');", conn);
                }
                else
                {
                    int mId = (int)row.Cells["id"].Value;
                    cmd = new NpgsqlCommand($"Update Districts SET district_name = '{textBox1.Text}' WHERE district_id = {mId};", conn);
                }
                
                await cmd.ExecuteNonQueryAsync();
                this.DialogResult = DialogResult.OK;
                if (row == null)
                {
                    MessageBox.Show("Район успешно добавлен");
                }
                else
                {
                    MessageBox.Show("Район успешно изменен");
                }
                
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
