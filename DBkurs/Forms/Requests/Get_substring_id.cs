using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Requests
{
    public partial class Get_substring_id : Form
    {
        public String Result { get; private set; }
        private bool isTolower;

        public Get_substring_id(bool isTolower = true)
        {
            this.InitializeComponent();
            this.isTolower = isTolower;
        }
        public Get_substring_id(String title, bool isTolower = true)
        {
            this.InitializeComponent();
            this.Text = label1.Text = title;
            this.isTolower = isTolower;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Result = isTolower ? textBox1.Text.ToLower() : textBox1.Text;
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
