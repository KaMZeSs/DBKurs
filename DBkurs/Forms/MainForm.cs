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
    delegate void UpdateTable();

    public partial class MainForm : Form
    {
        UpdateTable updator;

        public MainForm()
        {
            InitializeComponent();
            updator = Update_Albums;
        }

        #region Updaters

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String name = ((ToolStripMenuItem)sender).Text;

            switch (name)
            {
                case "Ассортимент":
                    updator = Update_ProductRanges;
                    break;
                case "Альбом":
                    updator = Update_Albums;
                    break;
                case "Магазин":
                    updator = Update_Shops;
                    break;
                case "Страна":
                    updator = Update_Countries;
                    break;
                case "Город":
                    updator = Update_Cities;
                    break;
                case "Фирма звукозаписи":
                    updator = Update_RecordFirms;
                    break;
                case "Жанр исполнения":
                    updator = Update_Genres;
                    break;
                case "Тип записи":
                    updator = Update_RecordTypes;
                    break;
                case "Язык исполнения":
                    updator = Update_Languages;
                    break;
                case "Исполнитель":
                    updator = Update_Executors;
                    break;
                case "Владелец":
                    updator = Update_Owners;
                    break;
                case "Тип собственности":
                    updator = Update_ProperyTypes;
                    break;
                case "Район города":
                    updator = Update_Districts;
                    break;
                default:
                    MessageBox.Show($"Забыл/перепутал {name}");
                    break;
            }
        }

        private void Update_Cities()
        {

        }
        private void Update_Countries()
        {

        }
        private void Update_Executors()
        {

        }
        private void Update_Genres()
        {

        }
        private void Update_Languages()
        {

        }
        private void Update_RecordFirms()
        {

        }
        private void Update_RecordTypes()
        {

        }

        private void Update_Districts()
        {

        }
        private void Update_Owners()
        {

        }
        private void Update_ProperyTypes()
        {

        }

        private void Update_Albums()
        {

        }
        private void Update_ProductRanges()
        {

        }
        private void Update_Shops()
        {

        }

        #endregion
    }
}
