using System.Windows.Forms;

namespace DBKurs.Forms
{
    class DoubleBufferedDataGridView : DataGridView
    {
        protected override bool DoubleBuffered { get => true; }
    }

}
