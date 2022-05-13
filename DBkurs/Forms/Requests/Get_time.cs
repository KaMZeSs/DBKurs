using System;
using System.Windows.Forms;

namespace DBKurs.Requests
{
    public partial class Get_time : Form
    {
        public DateTime Time
        {
            get { return dateTimePicker1.Value; }
        }

        public Get_time(string title)
        {
            this.InitializeComponent();
            Text = title;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
