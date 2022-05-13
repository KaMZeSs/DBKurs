using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Generator
{
    public delegate void SetInfo(DateTime time, string info);
    public partial class Form1 : Form
    {
        SetInfo info;
        public Form1()
        {
            this.InitializeComponent();
            info = new SetInfo((DateTime time, string str) =>
            {
                richTextBox1.Invoke(new SetInfo((time, str) =>
                {
                    int temp = richTextBox1.Text.Length;
                    richTextBox1.Text += $"[{time.ToString("H:mm:ss:fffffff")}]";
                    richTextBox1.SelectionStart = temp;
                    richTextBox1.SelectionLength = richTextBox1.Text.Length;
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.Text += $" {str} {Environment.NewLine}";
                }), time, str);
            });
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ForeColor = !Int32.TryParse(textBox1.Text, out int value) ? Color.Red : Color.Black;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            int count = 10000;
            int unic = 10;
            if (textBox1.Text.Length > 0 & textBox1.ForeColor.ToArgb() == Color.Black.ToArgb())
            {
                count = Int32.Parse(textBox1.Text);
            }
            if (textBox2.Text.Length > 0 & textBox2.ForeColor.ToArgb() == Color.Black.ToArgb())
            {
                unic = Int32.Parse(textBox2.Text);
            }

            button1.Enabled = false;
            DateTime start = DateTime.Now;
            await Task.Factory.StartNew(() => { new Randomize.Generate(info).Generation(len: count, minNew: unic).Send(); });
            richTextBox1.Text += $"Затрачено времени: {(DateTime.Now - start).TotalSeconds} с";
            button1.Enabled = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox1.ForeColor = !Int32.TryParse(textBox1.Text, out int value) ? Color.Red : Color.Black;
        }
    }
}
