using MonicaExtra.Model.monicaextra;
using MonicaExtra.View;
using System;
using System.Linq;

namespace MonicaExtra.Conrtroller
{
    class ControlCajaChicaModuloController
    {
        #region VARIABLES
        private readonly ControlCajaChicaModulo _view;
        private readonly monicaextraEntities _dbContext;

        //  VARIABLES PARA EL PAGINATOR DE LA TABLA QUEMUESTRA TODOS LOS MOVIMIENTOS
        private int paginaActiual = 0;
        private int totalRegistrosAMostrar = 50;
        private int totalPaginas = 0;
        #endregion

        public ControlCajaChicaModuloController(ControlCajaChicaModulo view)
        {
            _view = view;
            _dbContext = new monicaextraEntities();

            LlenarComponentesConDB();
            AplicarEventosAVista();
        }


        /// <summary>
        /// Llenar los componentes con los datos correspondientes, con data desde la base de datos y demas.
        /// </summary>
        private void LlenarComponentesConDB()
        {
            #region OBTENER EL NUMERO TOTAL DE PAGINAS
            var registros = _dbContext.movimientocajas.Where(e => e.Estatus == 1)
                                          .Count();
            totalPaginas = registros / totalRegistrosAMostrar;
            if (registros % totalRegistrosAMostrar != 0)
                totalPaginas++;
            #endregion

            LlenarTabla();
        }

        /// <summary>
        /// Colocar a los componentes de el VIEW los eventos correspondientes.
        /// </summary>
        private void AplicarEventosAVista()
        {
            #region BUTTONS
            _view.btnMovimientos.Click += new EventHandler((object sender, EventArgs e) =>
            {
                new CajaModulo().Show();
                _view.Hide();
            });

            #region BOTONES PARA EL PAGINATOR
            _view.btnPrimero.Click += new EventHandler((object sender, EventArgs e) =>
            {
                paginaActiual = 0;
                LlenarTabla();
            });

            _view.btnAnterior.Click += new EventHandler((object sender, EventArgs e) =>
            {
                --paginaActiual;
                LlenarTabla();
            });

            _view.btnSiguiente.Click += new EventHandler((object sender, EventArgs e) =>
            {
                ++paginaActiual;
                LlenarTabla();
            });

            _view.btnUltimo.Click += new EventHandler((object sender, EventArgs e) =>
            {
                paginaActiual = totalPaginas - 1;
                LlenarTabla();
            }); 
            #endregion
            #endregion
        }

        /// <summary>
        /// Llena la tabla que muestra todos los movimientos. Lo coloco en un metodo aparte porque se va a llamar desde diferentes lugares dentro de la vista.
        /// </summary>
        private void LlenarTabla()
        {
            _view.dataGridView1.DataSource = _dbContext.movimientocajas.Where(e => e.Estatus == 1)
                                                           .Select(s => new { s.NumeroTransacion, s.Beneficiario, s.Concepto, s.Monto, s.Fecha })
                                                           .OrderByDescending(o => o.NumeroTransacion)
                                                           .Skip(paginaActiual * totalRegistrosAMostrar)
                                                           .Take(totalRegistrosAMostrar)
                                                           .ToList();

            _view.lblPages.Text = $"{paginaActiual + 1} - {totalPaginas}";

            #region HABILITAR/DESHABILITAR LOS BOTONES DEL PAGINATOR SEGUN SE AMERITE
            if (paginaActiual == 0)
            {
                _view.btnAnterior.Enabled = false;
                _view.btnPrimero.Enabled = false;
            }
            else
            {
                _view.btnAnterior.Enabled = true;
                _view.btnPrimero.Enabled = true;
            }

            if (paginaActiual == (totalPaginas - 1))
            {
                _view.btnSiguiente.Enabled = false;
                _view.btnUltimo.Enabled = false;
            }
            else
            {
                _view.btnSiguiente.Enabled = true;
                _view.btnUltimo.Enabled = true;
            } 
            #endregion
        } 
    }
}
