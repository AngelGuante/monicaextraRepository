using MonicaExtra.Conrtroller;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MonicaExtra.View
{
    public partial class MovimientoSeleccionado : Form
    {
        public MovimientoSeleccionado(List<Model.Reportes.MovimientoSeleccionado> Movimiento)
        {
            InitializeComponent();
            CenterToScreen();
            new MovimientoSeleccionadoController(this, Movimiento);
        }
    }
}
