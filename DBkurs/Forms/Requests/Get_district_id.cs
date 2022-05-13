using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Requests
{
    public partial class Get_district_id : Form
    {
        private readonly string connectString;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private DataTable dtDistricts;

        public int District_id { get; private set; }

        public Get_district_id(string conn)
        {
            this.InitializeComponent();
            connectString = conn;
        }

        private async void Get_district_id_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);
            try
            {
                await conn.OpenAsync();
                await this.InitializeDistricts();
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
            District_id = Int32.Parse(listBox1.SelectedItem.ToString().Split('-').Last());
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
