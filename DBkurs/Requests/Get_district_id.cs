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
    public partial class Get_district_id : Form
    {
        private readonly String connectString;
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;
        private DataTable dtDistricts;

        public int District_id
        {
            get
            {
                return district_id;
            }
        }
        int district_id;
        public Get_district_id(String conn)
        {
            InitializeComponent();
            this.connectString = conn;
        }

        private async void Get_district_id_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);
            try
            {
                await conn.OpenAsync();
                await InitializeDistricts();
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

        private async Task InitializeDistricts()
        {
            cmd = new NpgsqlCommand($"SELECT * FROM Districts", conn);

            dtDistricts = new DataTable();
            dtDistricts.Load(await cmd.ExecuteReaderAsync());

            listBox1.Items.Clear();
            for (int i = 0; i < dtDistricts.Rows.Count; i++)
            {
                listBox1.Items.Add(dtDistricts.Rows[i][1] + " - " + dtDistricts.Rows[i][0]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            district_id = Int32.Parse(listBox1.SelectedItem.ToString().Split('-').Last());
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
