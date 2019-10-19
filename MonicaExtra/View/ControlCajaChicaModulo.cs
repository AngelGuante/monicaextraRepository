using MonicaExtra.Conrtroller;
using System.Windows.Forms;

namespace MonicaExtra.View
{
    public partial class ControlCajaChicaModulo : Form
    {
        public ControlCajaChicaModulo(MenuModulo VentanaAnterior)
        {
            InitializeComponent();
            CenterToScreen();
            new ControlCajaChicaModuloController(this, VentanaAnterior);
        }
    }
}
