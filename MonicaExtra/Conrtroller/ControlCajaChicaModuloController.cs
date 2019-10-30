using MonicaExtra.Model.monicaextra;
using MonicaExtra.Utils.DB;
using MonicaExtra.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MonicaExtra.Conrtroller
{
    class ControlCajaChicaModuloController : ControllersBase
    {
        #region VARIABLES
        private readonly ControlCajaChicaModulo _view;
        private readonly MenuModulo _ventanaAnterior;
        private readonly Connection _conn;
        #endregion

        public ControlCajaChicaModuloController(ControlCajaChicaModulo view, MenuModulo VentanaAnterior)
        {
            _view = view;
            _ventanaAnterior = VentanaAnterior;
            _conn = new Connection();

            LlenarComponentesConDB();
            AplicarEventosAVista();

            VentanaAnterior.Hide();
        }

        /// <summary>
        /// Llenar los componentes con los datos correspondientes, con data desde la base de datos y demas.
        /// </summary>
        private void LlenarComponentesConDB() =>
            LlenarTabla();

        /// <summary>
        /// Colocar a los componentes de el VIEW los eventos correspondientes.
        /// </summary>
        private void AplicarEventosAVista()
        {
            #region BUTTONS
            _view.btnMovimientos.Click += new EventHandler((object sender, EventArgs e) =>
            {
                new CajaModulo(_view).Show();
            });

            _view.btnAtras.Click += new EventHandler((object sender, EventArgs e) =>
            {
                _ventanaAnterior.Show();
                _view.Dispose();
            });
            #endregion

            _view.FormClosing += new FormClosingEventHandler((object sender, FormClosingEventArgs e) => { Dispose(); });
        }

        /// <summary>
        /// Llena la tabla que muestra todos los movimientos. Lo coloco en un metodo aparte porque se va a llamar desde diferentes lugares dentro de la vista.
        /// </summary>
        private void LlenarTabla() =>
            _view.dataGridView1.DataSource = ((List<MovimientoCajaModel>)_conn.ExecuteQuery(new System.Text.StringBuilder("SELECT TOP 50 NumeroTransacion, Beneficiario, Concepto, Rnc, Ncf, Monto, Fecha  FROM monicaextra.movimientocaja WHERE Estatus = 1 ORDER BY NumeroTransacion DESC"), "monica10", "movimientocaja"))
                                                                                            .Select(s => new { s.NumeroTransacion, s.Beneficiario, s.Concepto, s.Monto, s.Fecha })
                                                                                            .ToList();
    }
}
