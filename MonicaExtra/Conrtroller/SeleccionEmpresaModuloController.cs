using MonicaExtra.Model.monica_global;
using MonicaExtra.Utils;
using MonicaExtra.Utils.DB;
using MonicaExtra.View;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MonicaExtra.Conrtroller
{
    class SeleccionEmpresaModuloController
    {
        #region VARIABLES
        private readonly SeleccionEmpresaModulo _view;
        private Connection _conn;
        private List<EmpresaModel> _empresas;
        #endregion

        public SeleccionEmpresaModuloController(SeleccionEmpresaModulo view)
        {
            _view = view;
            _conn = new Connection();

            LlenarComponentesConDB();
            AplicarEventosAVista();
        }

        /// <summary>
        /// Llenar los componentes con los datos correspondientes, con data desde la base de datos y demas.
        /// </summary>
        private void LlenarComponentesConDB()
        {
            _empresas = (List<EmpresaModel>)_conn.ExecuteQuery(new System.Text.StringBuilder("SELECT empresa_id, Nombre_empresa FROM empresas"), "monica10_global", "empresa");

            #region DataGridViews
            var empresas = _empresas.Select(s => new { ID = s.empresa_id, Empresa = s.Nombre_empresa.Trim().ToUpper() })
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
                GlobalVariables._empresaSeleccionada = _empresas.FirstOrDefault(f => f.empresa_id == idEmpresa);
                new MenuModulo().Show();
                _view.Hide();
            });
            #endregion
        }
    }
}
