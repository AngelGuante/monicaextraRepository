using MonicaExtra.Model.Reportes;
using MonicaExtra.View;
using System;
using System.Windows.Forms;

namespace MonicaExtra.Conrtroller
{
    class CuadreModuloController : ControllersBase
    {
        #region VARIABLES
        private readonly CuadreModulo _view;
        private readonly CajaModulo _cajaModuloView;
        #endregion

        public CuadreModuloController(CuadreModulo view, CajaModulo caja)
        {
            _view = view;
            _cajaModuloView = caja;

            AplicarEventosAVista();
        }

        /// <summary>
        /// Colocar a los componentes de el VIEW los eventos correspondientes.
        /// </summary>
        private void AplicarEventosAVista()
        {
            #region BUTTONS
            _view.btnBuscar.Click += new EventHandler((object sender, EventArgs e) =>
    {
        LlenarReporteCaja();
    });

            _view.btnAtras.Click += new EventHandler((object sender, EventArgs e) =>
            {
                _cajaModuloView.Visible = true;
                _view.Close();
            });
            #endregion

            _view.Load += new EventHandler((object sender, EventArgs e) =>
            {
                LlenarReporteCaja();
            });

            _view.FormClosing += new FormClosingEventHandler((object sender, FormClosingEventArgs e) => { Dispose(); });
        }

        /// <summary>
        /// Llena el report de los cierres de caja
        /// </summary>
        private void LlenarReporteCaja()
        {
            //var FechaHastaMasUno = _view.dtpFechaHasta.Value.AddDays(1);
            //Model.Reportes.monicaextraDataSetTableAdapters.cierrecajaTableAdapter cierrecajaTableAdapter = new Model.Reportes.monicaextraDataSetTableAdapters.cierrecajaTableAdapter();
            //cierrecajaTableAdapter.CierresEntreFechas(_dataSet.cierrecaja, _view.dtpFechaDesde.Value.ToString("yyy-MM-dd"), FechaHastaMasUno.ToString("yyy-MM-dd"));
            //_view.reportViewer1.RefreshReport();
        }
    }
}
