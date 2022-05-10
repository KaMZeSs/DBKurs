﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBKurs.Requests
{
    public partial class Get_time_period : Form
    {
        public struct TimePeriod
        {
            public DateTime Start { get; }
            public DateTime End { get; }
            public TimePeriod(DateTime vs1, DateTime vs2)
            {
                Start = vs1;
                End = vs2;
            }
        }

        public TimePeriod timePeriod { get; set; }

        public Get_time_period(String title)
        {
            InitializeComponent();
            this.Text = title;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("Начальная дата не может быть больше конечной");
                return;
            }

            timePeriod = new TimePeriod(dateTimePicker1.Value, dateTimePicker2.Value);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
