using MonicaExtra.Conrtroller;
using System.Windows.Forms;

namespace MonicaExtra.View
{
    public partial class CuadreModulo : Form
    {
        public CuadreModulo(CajaModulo caja)
        {
            InitializeComponent();
            CenterToScreen();
            new CuadreModuloController(this, monicaextraDataSet, caja);
        }
    }
}
