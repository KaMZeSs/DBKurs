using System.Drawing;
using System.Windows.Forms;

namespace DBKurs.Styles
{
    public class TestColorTable : ProfessionalColorTable
    {
        public override Color MenuBorder => Color.FromArgb(208, 208, 208);
        public override Color MenuItemBorder => Color.FromArgb(208, 208, 208);
        public override Color MenuItemSelected => Color.FromArgb(218, 218, 218);
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(184, 184, 184);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(184, 184, 184);
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(184, 184, 184);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(184, 184, 184);
    }

}
