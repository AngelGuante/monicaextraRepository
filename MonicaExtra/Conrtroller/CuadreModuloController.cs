using MonicaExtra.Model.monicaextra;
using MonicaExtra.Model.Reportes;
using MonicaExtra.Utils.DB;
using MonicaExtra.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                LlenarReporteCaja(true);
            });

            _view.btnAtras.Click += new EventHandler((object sender, EventArgs e) =>
            {
                _cajaModuloView.Visible = true;
                _view.Hide();
            });
            #endregion

            _view.Load += new EventHandler((object sender, EventArgs e) =>
            {
                LlenarReporteCaja(false);
            });

            _view.FormClosing += new FormClosingEventHandler((object sender, FormClosingEventArgs e) => { Dispose(); });
        }

        /// <summary>
        /// Llena el report de los cierres de caja.
        /// </summary>
        private void LlenarReporteCaja(bool buscadoPorFecha)
        {
            List<CierreCajaModel> objCierreCaja = new List<CierreCajaModel>();

            if (buscadoPorFecha)
            {

                StringBuilder _query = new StringBuilder();
                var FechaHastaMasUno = _view.dtpFechaHasta.Value.AddDays(1);
                _query.Append("SELECT * " +
                              "FROM [monicaextra].[cierrecaja] " +
                             $"WHERE FechaProceso >= '{_view.dtpFechaDesde.Value.ToString("yyy-MM-dd")}' AND FechaProceso <= '{FechaHastaMasUno.ToString("yyy-MM-dd")}' " +
                             $"ORDER BY NumeroCierre DESC ");

                ((List<CierreCajaModel>)new Connection().ExecuteQuery(_query, "monica10", "cierrecaja")).Select(s => new { s.NumeroCierre, s.FechaProceso, s.SaldoFinal, s.Comentario }).ToList().ForEach(cierre =>
                     {
                         objCierreCaja.Add(new CierreCajaModel
                         {
                             NumeroCierre = cierre.NumeroCierre,
                             FechaProceso = cierre.FechaProceso,
                             SaldoFinal = (int)cierre.SaldoFinal,
                             Comentario = cierre.Comentario
                         });
                     });
            }
            else
            {
                ((List<CierreCajaModel>)new Connection().ExecuteQuery(new StringBuilder("SELECT TOP (50) * FROM monicaextra.CierreCaja ORDER BY NumeroCierre DESC"), "monica10", "cierrecaja")).Select(s => new { s.NumeroCierre, s.FechaProceso, s.SaldoFinal, s.Comentario }).ToList().ForEach(cierre =>
                {
                    objCierreCaja.Add(new CierreCajaModel
                    {
                        NumeroCierre = cierre.NumeroCierre,
                        FechaProceso = cierre.FechaProceso,
                        SaldoFinal = (int)cierre.SaldoFinal,
                        Comentario = cierre.Comentario
                    });
                });
            }

            _view.reportViewer1.LocalReport.DataSources.Clear();
            _view.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("CierreCajaDataSet", objCierreCaja));
            _view.reportViewer1.RefreshReport();

        }
    }
}
