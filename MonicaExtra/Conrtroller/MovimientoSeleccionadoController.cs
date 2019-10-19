using System.Collections.Generic;

namespace MonicaExtra.Conrtroller
{
    class MovimientoSeleccionadoController
    {
        #region VARIABLES
        private View.MovimientoSeleccionado _view;
        #endregion

        public MovimientoSeleccionadoController(View.MovimientoSeleccionado view, List<Model.Reportes.MovimientoSeleccionado> Movimiento)
        {
            _view = view;

            LlenarReporteCaja(Movimiento);
        }

        // <summary>
        // Llena el report con el movimiento seleccionado
        // </summary>
        private void LlenarReporteCaja(List<Model.Reportes.MovimientoSeleccionado> Movimiento)
        {
            _view.reportViewer1.LocalReport.DataSources.Clear();
            _view.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("MovimientoSeleccionadoDataSet", Movimiento));
            _view.reportViewer1.RefreshReport();
        }

    }
}
