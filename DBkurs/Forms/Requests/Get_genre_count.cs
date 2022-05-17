using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Requests
{
    public partial class Get_genre_count : Form
    {
        private readonly string connectString;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private DataTable dtGenres;

        public int Genre_id { get; private set; }
        public int Count { get; private set; }

        public Get_genre_count(string conn)
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
                await this.InitializeGenres();
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

        private async Task InitializeGenres()
        {
            cmd = new NpgsqlCommand($"SELECT * FROM Genres", conn);

            dtGenres = new DataTable();
            dtGenres.Load(await cmd.ExecuteReaderAsync());

            listBox1.Items.Clear();
            for (int i = 0; i < dtGenres.Rows.Count; i++)
            {
                listBox1.Items.Add(dtGenres.Rows[i][1] + " - " + dtGenres.Rows[i][0]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Genre_id = Int32.Parse(listBox1.SelectedItem.ToString().Split('-').Last());
            Count = Decimal.ToInt32(numericUpDown1.Value);
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
