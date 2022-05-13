﻿using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Requests
{
    public partial class Get_owner_id : Form
    {
        private readonly string connectString;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private DataTable dtShops;

        public int Owner_id { get; private set; }

        public Get_owner_id(string conn)
        {
            this.InitializeComponent();
            connectString = conn;
        }

        private async void Get_shop_id_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);
            try
            {
                await conn.OpenAsync();
                await this.InitializeOwners();
                listBox1.SelectedIndex = 0;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        private async Task InitializeOwners()
        {
            cmd = new NpgsqlCommand($"SELECT owner_id, owner_name FROM Owners", conn);

            dtShops = new DataTable();
            dtShops.Load(await cmd.ExecuteReaderAsync());

            listBox1.Items.Clear();
            for (int i = 0; i < dtShops.Rows.Count; i++)
            {
                listBox1.Items.Add(dtShops.Rows[i][1] + " - " + dtShops.Rows[i][0]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Owner_id = Int32.Parse(listBox1.SelectedItem.ToString().Split('-').Last());
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
