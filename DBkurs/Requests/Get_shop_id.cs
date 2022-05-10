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

namespace DBKurs.Requests
{
    public partial class Get_shop_id : Form
    {
        private readonly String connectString;
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;
        private DataTable dtShops;

        public int Shop_id
        {
            get
            {
                return shop_id;
            }
        }
        int shop_id;
        public Get_shop_id(String conn)
        {
            InitializeComponent();
            this.connectString = conn;
        }

        private async void Get_shop_id_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);
            try
            {
                await conn.OpenAsync();
                await InitializeShops();
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

        private async Task InitializeShops()
        {
            cmd = new NpgsqlCommand($"SELECT shop_id, shop_name FROM Shops", conn);

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
            shop_id = Int32.Parse(listBox1.SelectedItem.ToString().Split('-').Last());
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
