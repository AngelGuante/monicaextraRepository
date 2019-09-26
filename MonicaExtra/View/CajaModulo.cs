using MonicaExtra.Conrtroller;
using System.Windows.Forms;

namespace MonicaExtra.View
{
    public partial class CajaModulo : Form
    {
        public CajaModulo()
        {
            InitializeComponent();
            CenterToScreen();
            new CajaModuloController(this);
        }
    }
}
