using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Requests
{
    public partial class Get_length_count : Form
    {
        public int Length { get; private set; }
        public int Count { get; private set; }

        public Get_length_count()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Length = Decimal.ToInt32(numericUpDown2.Value);
            Count = Decimal.ToInt32(numericUpDown1.Value);
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
