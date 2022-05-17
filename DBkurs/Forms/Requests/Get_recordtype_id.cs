using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Requests
{
    public partial class Get_recordtype_id : Form
    {
        private readonly string connectString;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private DataTable dtCountries;

        public int RecordType_id { get; private set; }

        public Get_recordtype_id(string conn)
        {
            this.InitializeComponent();
            connectString = conn;
        }

        private async void Get_recordtype_id_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);
            try
            {
                await conn.OpenAsync();
                await this.InitializeRecordtypes();
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

        private async Task InitializeRecordtypes()
        {
            cmd = new NpgsqlCommand($"SELECT recordtype_id, recordtype_name FROM RecordTypes", conn);

            dtCountries = new DataTable();
            dtCountries.Load(await cmd.ExecuteReaderAsync());

            listBox1.Items.Clear();
            for (int i = 0; i < dtCountries.Rows.Count; i++)
            {
                listBox1.Items.Add(dtCountries.Rows[i][1] + " - " + dtCountries.Rows[i][0]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RecordType_id = Int32.Parse(listBox1.SelectedItem.ToString().Split('-').Last());
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
