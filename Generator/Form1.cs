using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Generator
{
    public delegate void SetInfo(DateTime time, String info);
    public partial class Form1 : Form
    {
        SetInfo info;
        public Form1()
        {
            InitializeComponent();
            info = new SetInfo((DateTime time, String str) =>
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
            if (!Int32.TryParse(textBox1.Text, out int value))
            {
                textBox1.ForeColor = Color.Red;
            }
            else
            {
                textBox1.ForeColor = Color.Black;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 & textBox1.ForeColor == Color.Black)
            {
                this.button1.Enabled = false;
                DateTime start = DateTime.Now;
                await Task.Factory.StartNew(() => { new Randomize.Generate(info).Generation(int.Parse(textBox1.Text)).Send(); });
                richTextBox1.Text += $"Затрачено времени: {(DateTime.Now - start).TotalSeconds} с";
                this.button1.Enabled = true;
            }
        }
    }
}
