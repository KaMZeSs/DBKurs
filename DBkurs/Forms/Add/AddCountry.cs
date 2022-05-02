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
    public partial class AddCountry : Form
    {
        public readonly static String connectString = "Host=localhost;Port=5432;User Id=postgres;Password=1310;Database=Kurs";
        public NpgsqlConnection conn;
        public String sql;
        public NpgsqlCommand cmd;
        public DataTable dt;

        public bool isChanged { get; set; }

        public AddCountry()
        {
            InitializeComponent();
        }

        private void AddCountry_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);
            isChanged = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        
    }
}
