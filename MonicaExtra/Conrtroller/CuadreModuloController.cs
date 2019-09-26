using MonicaExtra.Model.Reportes;
using MonicaExtra.View;
using System;

namespace MonicaExtra.Conrtroller
{
    class CuadreModuloController
    {
        #region VARIABLES
        private readonly CuadreModulo _view;
        private readonly monicaextraDataSet _dataSet;
        private readonly CajaModulo _cajaModuloView;
        #endregion

        public CuadreModuloController(CuadreModulo view, monicaextraDataSet ds, CajaModulo caja)
        {
            _view = view;
            _dataSet = ds;
            _cajaModuloView = caja;

            AplicarEventosAVista();
        }

        /// <summary>
        /// Colocar a los componentes de el VIEW los eventos correspondientes.
        /// </summary>
        private void AplicarEventosAVista()
        {
            _view.Load += new EventHandler((object sender, EventArgs e) =>
            {
                LlenarReporteCaja();
            });

            _view.btnBuscar.Click += new EventHandler((object sender, EventArgs e) =>
            {
                LlenarReporteCaja();
            });

            _view.btnAtras.Click += new EventHandler((object sender, EventArgs e) =>
            {
                _cajaModuloView.Visible = true;
                _view.Close();
            });
        }

        /// <summary>
        /// Llena el report de los cierres de caja
        /// </summary>
        private void LlenarReporteCaja()
        {
            var FechaHastaMasUno = _view.dtpFechaHasta.Value.AddDays(1);
            Model.Reportes.monicaextraDataSetTableAdapters.cierrecajaTableAdapter cierrecajaTableAdapter = new Model.Reportes.monicaextraDataSetTableAdapters.cierrecajaTableAdapter();
            cierrecajaTableAdapter.CierresEntreFechas(_dataSet.cierrecaja, _view.dtpFechaDesde.Value.ToString("yyy-MM-dd"), FechaHastaMasUno.ToString("yyy-MM-dd"));
            _view.reportViewer1.RefreshReport();
        }
    }
}
