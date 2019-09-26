using MonicaExtra.Conrtroller;
using System.Windows.Forms;

namespace MonicaExtra.View
{
    public partial class SeleccionEmpresaModulo : Form
    {
        public SeleccionEmpresaModulo()
        {
            InitializeComponent();
            CenterToScreen();
            new SeleccionEmpresaModuloController(this);
        }
    }
}
