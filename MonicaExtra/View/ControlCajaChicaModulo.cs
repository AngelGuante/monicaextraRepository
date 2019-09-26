using MonicaExtra.Conrtroller;
using System.Windows.Forms;

namespace MonicaExtra.View
{
    public partial class ControlCajaChicaModulo : Form
    {
        public ControlCajaChicaModulo()
        {
            InitializeComponent();
            CenterToScreen();
            new ControlCajaChicaModuloController(this);
        }
    }
}
