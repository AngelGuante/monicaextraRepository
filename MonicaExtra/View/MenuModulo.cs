using MonicaExtra.Conrtroller;
using System.Windows.Forms;

namespace MonicaExtra.View
{
    public partial class MenuModulo : Form
    {
        public MenuModulo()
        {
            InitializeComponent();
            new MenuModuloController(this);
            CenterToScreen();
        }
    }
}
