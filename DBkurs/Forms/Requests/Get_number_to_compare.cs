using System;
using System.Windows.Forms;

namespace DBKurs.Requests
{
    public partial class Get_number_to_compare : Form
    {
        public int Value { get; private set; }

        public Get_number_to_compare()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Value = Decimal.ToInt32(numericUpDown1.Value);
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
