using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtTOxml();
        }

        void txtTOxml()
        {
            using (var reader = new StreamReader("E:/Repos/DBKurs/Generator/Data/запись.txt"))
            {
                String[] r = reader.ReadToEnd().Split("\r\n");
                Array.Sort(r);
                var serializer = new XmlSerializer(typeof(string[]));
                using (var writer = new StreamWriter("PropertyTypes.xml"))
                {
                    serializer.Serialize(writer, r);
                }
            }
        }

    }
}
