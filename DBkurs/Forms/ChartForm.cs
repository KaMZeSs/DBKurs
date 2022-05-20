using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBKurs.Forms
{
    public partial class ChartForm : Form
    {
        public DataTable dt
        {
            set
            {
                if (value.Columns.Count >= 3 && !value.Columns.Contains("id") 
                    || value.Columns.Count == 0)
                    throw new ArgumentException("Невозможно отобразить график");
                this.UpdateChart(value);
            }
        }

        public ChartForm()
        {
            InitializeComponent();
            chart1.Series[0]["PieLabelStyle"] = "Disabled";
        }

        private void ChartForm_Load(object sender, EventArgs e)
        {
            (this.Owner as MainForm).onRequest_Asked_To_Show += onAsked_To_Update;
            this.столбцыToolStripMenuItem.PerformClick();
        }

        private void onAsked_To_Update(object sender, MainForm.DataTableEventArgs e)
        {
            dt = e.data;
        }

        public struct Data
        {
            public double Value { get; set; }
            public String Label { get; set; }

            public Data(int value, string label)
            {
                Value = value;
                Label = label;
            }
        }

        private void UpdateChart(DataTable data)
        {
            chart1.Series[0].Points.Clear();

            int xAxis_index = data.Columns.Contains("id") ? 1 : 0;
            int yAxis_index = xAxis_index + 1;

            var values = (from DataRow row in data.AsEnumerable().AsParallel() 
                          select new Data(Int32.Parse(row[yAxis_index].ToString()), 
                          row[xAxis_index].ToString()))
                          .AsParallel()
                          .AsOrdered<Data>()
                          .ToArray<Data>();

            for (int i = 0; i < values.Length; i++)
            {
                var point = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                point.AxisLabel = values[i].Label;
                point.SetValueY(values[i].Value);
                chart1.Series.First().Points.Add(point);
            }
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch ((sender as ToolStripMenuItem).Text)
            {
                case "Круговая":
                    chart1.Legends.First().Enabled = true;
                    chart1.Series.First().ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
                    break;
                case "Столбчатая":
                    chart1.Legends.First().Enabled = false;
                    chart1.Series.First().Sort(System.Windows.Forms.DataVisualization.Charting.PointSortOrder.Descending);
                    chart1.Series.First().ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    break;
                case "Пончик":
                    chart1.Legends.First().Enabled = true;
                    chart1.Series.First().ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
                    break;
                case "Радар":
                    chart1.Legends.First().Enabled = false;
                    chart1.Series.First().ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
                    break;
                case "Сохранить как":
                    var save = new SaveFileDialog()
                    {
                        Filter = "PNG Image | *.png",
                        RestoreDirectory = true
                    };
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        chart1.SaveImage(save.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                    }
                    break;
            }
        }

        private void ChartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            (this.Owner as MainForm).onRequest_Asked_To_Show -= onAsked_To_Update;
        }
    }
}
