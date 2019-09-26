using MonicaExtra.Model.monica_global;
using MonicaExtra.Utils;
using MonicaExtra.View;
using System.Linq;
using System.Windows.Forms;

namespace MonicaExtra.Conrtroller
{
    class SeleccionEmpresaModuloController
    {
        #region VARIABLES
        private readonly SeleccionEmpresaModulo _view;
        private readonly monica10_globalEntities _dbContext;
        #endregion

        public SeleccionEmpresaModuloController(SeleccionEmpresaModulo view)
        {
            _view = view;
            _dbContext = new monica10_globalEntities();

            LlenarComponentesConDB();
            AplicarEventosAVista();
        }

        /// <summary>
        /// Llenar los componentes con los datos correspondientes, con data desde la base de datos y demas.
        /// </summary>
        private void LlenarComponentesConDB()
        {
            #region DataGridViews
            var empresas = _dbContext.empresas.Select(s => new { ID = s.empresa_id, Empresa = s.Nombre_empresa.Trim().ToUpper() })
                                      .ToList();
            _view.dgvSelectEmpresa.DataSource = empresas;
            #endregion
        }

        /// <summary>
        /// Colocar a los componentes de el VIEW los eventos correspondientes.
        /// </summary>
        private void AplicarEventosAVista()
        {
            #region DataGridViews
            _view.dgvSelectEmpresa.CellClick += new DataGridViewCellEventHandler((object sender, DataGridViewCellEventArgs e) =>
            {
                if (e.RowIndex < 0) return;
                var idEmpresa = (int)_view.dgvSelectEmpresa.Rows[e.RowIndex].Cells[0].Value;
                GlobalVariables.empresaSeleccionada = _dbContext.empresas.FirstOrDefault(f => f.empresa_id == idEmpresa);
                new MenuModulo().Show();
                _view.Hide();
            });
            #endregion
        }
    }
}
