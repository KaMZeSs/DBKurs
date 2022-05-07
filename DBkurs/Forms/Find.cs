using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace DBKurs.Forms
{
    public partial class Find : Form
    {
        Tables CurrentTable
        {
            set
            {
                comboBox1.Items.Clear();

                String[] items;
                switch (value)
                {
                    case Tables.ProductRanges:
                        items = new String[] { "Альбом", "Магазин", "Дата поступления", "Количество единиц" };
                        break;
                    case Tables.Albums:
                        items = new String[] { "Альбом", "Магазин", "Дата поступления", "Количество единиц" };
                        break;
                    case Tables.Shops:
                        items = new String[] { "Альбом", "Магазин", "Дата поступления", "Количество единиц" };
                        break;
                    case Tables.Cities:
                        items = new String[] { "Альбом", "Магазин", "Дата поступления", "Количество единиц" };
                        break;
                    case Tables.Countries:
                        items = new String[] { "Альбом", "Магазин", "Дата поступления", "Количество единиц" };
                        break;
                    case Tables.Executors:
                        items = new String[] { "Альбом", "Магазин", "Дата поступления", "Количество единиц" };
                        break;
                    case Tables.Genres:
                        items = new String[] { "Альбом", "Магазин", "Дата поступления", "Количество единиц" };
                        break;
                    case Tables.Languages:
                        items = new String[] { "Альбом", "Магазин", "Дата поступления", "Количество единиц" };
                        break;
                    case Tables.RecordFirms:
                        items = new String[] { "Альбом", "Магазин", "Дата поступления", "Количество единиц" };
                        break;
                    case Tables.RecordTypes:
                        items = new String[] { "Альбом", "Магазин", "Дата поступления", "Количество единиц" };
                        break;
                    case Tables.Districts:
                        items = new String[] { "Альбом", "Магазин", "Дата поступления", "Количество единиц" };
                        break;
                    case Tables.Owners:
                        items = new String[] { "Альбом", "Магазин", "Дата поступления", "Количество единиц" };
                        break;
                    case Tables.PropertyTypes:
                        items = new String[] { "Альбом", "Магазин", "Дата поступления", "Количество единиц" };
                        break;
                    default:
                        items = null;
                        break;
                }
                comboBox1.Items.AddRange(items);
            }
        }
        public Find()
        {
            InitializeComponent();
        }

        private void Find_Load(object sender, EventArgs e)
        {
            var owner = this.Owner as MainForm;
            owner.onTable_Updated += Owner_onTable_Updated;
            CurrentTable = owner.CurrentTable;
        }

        private void Owner_onTable_Updated(object sender, MainForm.TableEventArgs e)
        {
            CurrentTable = e.NewTable;
        }
    }
}
