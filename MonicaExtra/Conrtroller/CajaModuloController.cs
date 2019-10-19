using MonicaExtra.Model.monicaextra;
using MonicaExtra.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonicaExtra.Conrtroller
{
    class CajaModuloController : ControllersBase
    {
        #region VARIABLES
        private readonly CajaModulo _view;
        private readonly monicaextraEntities _dbContext;

        private readonly List<clasificacionmovicaja> _clasificacionMovimientoCaja;
        private readonly List<string> _clasificacionRNC;
        private readonly List<clasificacionfiscal> _clasificacionFiscal;
        private readonly List<string> _clasificacionNroCaja;
        private readonly List<usuario> _usuarios;

        private readonly ControlCajaChicaModulo _ventanaAnterior;
        //  Almacenara el objeto seleccionado en el DataGridView, para cuando se le vaya a hacer una modificacion.
        private movimientocaja objMovimientoSeleccionado = null;
        #endregion

        public CajaModuloController(CajaModulo view, ControlCajaChicaModulo VentanaAnterior)
        {
            _ventanaAnterior = VentanaAnterior;
            _view = view;
            _dbContext = new monicaextraEntities();

            _clasificacionMovimientoCaja = _dbContext.clasificacionmovicajas.OrderBy(o => new { o.Tipo, o.Descripcion })
                                                                            .ToList();
            _clasificacionRNC = _dbContext.movimientocajas.Where(w => w.Rnc != null && w.Rnc != "")
                                                          .Select(s => s.Rnc)
                                                          .Distinct()
                                                          .ToList();
            _clasificacionNroCaja = _dbContext.movimientocajas.Where(w => w.NumeroCaja != null && w.NumeroCaja.Value.ToString() != "")
                                                              .Select(s => s.NumeroCaja.Value.ToString())
                                                              .Distinct()
                                                              .ToList();
            _clasificacionFiscal = _dbContext.clasificacionfiscals.OrderBy(o => o.Descripcion)
                                                                  .ToList();
            _usuarios = _dbContext.usuarios
                                  .ToList();

            LlenarComponentesConDB(true);
            AplicarEventosAVista();

            _ventanaAnterior.Hide();
        }

        /// <summary>
        /// Llenar los componentes con los datos correspondientes, con data desde la base de datos y demas.
        /// </summary>
        private void LlenarComponentesConDB(bool LlenarComboBoxes)
        {
            if (LlenarComboBoxes)
            {
                //  ComboBox para el filtro usuarios
                Dictionary<short, string> usuariosCmbb = new Dictionary<short, string>();
                _usuarios.Where(U => U.activo == true).ToList()
                                                      .ForEach(f =>
                                                      {
                                                          usuariosCmbb.Add((short)f.id_usuario, string.Concat("(", f.usuario1, ")\t - ", f.nombre_completo));
                                                      });
                _view.cmbbCargadoA.DataSource = new BindingSource(usuariosCmbb, null);
                _view.cmbbCargadoA.DisplayMember = "Value";
                _view.cmbbCargadoA.ValueMember = "Key";

                //  ComboBox para el filtro de tipo de movimiento
                Dictionary<short, string> tipoMovimientoCmbb = new Dictionary<short, string>();
                _clasificacionMovimientoCaja.Where(x => x.Visible == 1).ToList()
                                                                       .ForEach(f =>
                                                                       {
                                                                           tipoMovimientoCmbb.Add(f.NumeroTransacion, string.Concat("(", f.Tipo, ")\t - ", f.Descripcion));
                                                                       });
                _view.cmbbTipoMovimiento.DataSource = new BindingSource(tipoMovimientoCmbb, null);
                _view.cmbbTipoMovimiento.DisplayMember = "Value";
                _view.cmbbTipoMovimiento.ValueMember = "Key";

                //  ComboBox para el filtro de la clasificacion de NCF
                Dictionary<short, string> clasfNcfCmbb = new Dictionary<short, string>();
                _clasificacionFiscal.ToList()
                                    .ForEach(f =>
                                    {
                                        clasfNcfCmbb.Add(f.NumeroTransacion, f.Descripcion);
                                    });
                _view.cmbbClasifFiscal.DisplayMember = "Value";
                _view.cmbbClasifFiscal.ValueMember = "Key";
                _view.cmbbClasifFiscal.DataSource = new BindingSource(clasfNcfCmbb, null);

                //  ComboBox para el filtro de busqueda de movimientos
                Dictionary<int, string> filtroCmbb = new Dictionary<int, string>();
                filtroCmbb.Add(1, "Todo");
                filtroCmbb.Add(2, "Tipo Movimiento");
                filtroCmbb.Add(3, "Entrada/Salida");
                filtroCmbb.Add(4, "Beneficiario");
                filtroCmbb.Add(5, "RNC");
                filtroCmbb.Add(6, "Clasificacion Fiscal");
                filtroCmbb.Add(7, "Nro. Caja");
                _view.cmbbFiltroMovimientos.DisplayMember = "Value";
                _view.cmbbFiltroMovimientos.ValueMember = "Key";
                _view.cmbbFiltroMovimientos.DataSource = new BindingSource(filtroCmbb, null);
            }

            _view.dataGridView1.DataSource = _dbContext.movimientocajas.Where(e => e.NumeroCierre == (_dbContext.cierrecajas.Max(m => m.NumeroCierre) + 1) &&
                                                                                   e.Estatus == 1)
                                                                       .Select(s => new { s.NumeroTransacion, s.Beneficiario, s.Concepto, s.Monto, s.Fecha })
                                                                       .OrderByDescending(o => o.NumeroTransacion)
                                                                       .ToList();
        }

        /// <summary>
        /// Colocar a los componentes de el VIEW los eventos correspondientes.
        /// </summary>
        private void AplicarEventosAVista()
        {
            #region CheckBoxes
            _view.chbDatosFiscales.CheckedChanged += new EventHandler((object sender, EventArgs e) =>
            {
                SwitchEstadoCamposFiscales(_view.chbDatosFiscales.Checked);
            });
            #endregion

            #region Buttones
            _view.btnHacerCuadre.Click += new EventHandler((object sender, EventArgs e) =>
            {
                _view.Visible = false;
                new CuadreModulo(_view).Show();
            });

            _view.btnGuardar.Click += new EventHandler((object sender, EventArgs e) =>
            {
                var EntradaSalida = (short)Convert.ToInt32(_view.cmbbTipoMovimiento.SelectedValue.ToString());
                //  Datos Basicos
                var movimiento = new movimientocaja
                {
                    NumeroTransacion = (short)(_dbContext.movimientocajas.Max(m => m.NumeroTransacion) + 1),
                    Fecha = _view.dtpEmicion.Value.ToString("yyy-MM-dd"),
                    Beneficiario = _view.cmbbCargadoA.SelectedValue.ToString(),
                    Monto = Convert.ToDecimal(_view.txtbMonto.Text),
                    TipoMovimiento = _clasificacionMovimientoCaja.FirstOrDefault(f => f.NumeroTransacion == (short)Convert.ToInt32(_view.cmbbTipoMovimiento.SelectedValue.ToString()))?.NumeroTransacion,
                    Concepto = _view.txtbConcepto.Text,
                    EntradaSalida = _clasificacionMovimientoCaja.FirstOrDefault(f => f.NumeroTransacion == EntradaSalida)?.Tipo,
                    NumeroCierre = (short)(_dbContext.cierrecajas.Max(m => m.NumeroCierre) + 1),
                    NumeroCaja = 1,
                    TipoMoneda = "P",
                    TasaCambio = (decimal)0.0000,
                    Estatus = 1
                };

                //Datos Fiscales
                if (_view.chbDatosFiscales.Checked)
                {
                    movimiento.Rnc = _view.txtbRNC.Text;
                    movimiento.Ncf = _view.txtbNCF.Text;
                    movimiento.Clasificancf = _clasificacionFiscal.FirstOrDefault(s => s.NumeroTransacion == (short)Convert.ToInt32(_view.cmbbClasifFiscal.SelectedValue))?.NumeroTransacion;
                    movimiento.Neto = Convert.ToDecimal(_view.txtbValorSNITBIS.Text);
                    movimiento.Itebis = _view.txtbITBISFactur.Text;

                    _view.txtbRNC.Enabled = false;
                    _view.txtbNCF.Enabled = false;
                    _view.cmbbClasifFiscal.Enabled = false;
                    _view.txtbValorSNITBIS.Enabled = false;
                    _view.txtbITBISFactur.Enabled = false;
                }

                _dbContext.movimientocajas.Add(movimiento);
                _dbContext.SaveChanges();

                ResetVentana();
            });

            _view.btnLimpiarCampos.Click += new EventHandler((object sender, EventArgs e) =>
            {
                ResetVentana();
            });

            _view.btnModificar.Click += new EventHandler((object sender, EventArgs e) =>
            {
                var mod = _dbContext.movimientocajas.FirstOrDefault(f => f.NumeroTransacion == objMovimientoSeleccionado.NumeroTransacion);

                //  Datos Basicos
                mod.NumeroTransacion = objMovimientoSeleccionado.NumeroTransacion;
                mod.Fecha = _view.dtpEmicion.Value.ToString("yyy-MM-dd hh:mm:ss");
                mod.Beneficiario = _view.cmbbCargadoA.SelectedValue.ToString();
                mod.Monto = Convert.ToDecimal(_view.txtbMonto.Text);
                mod.TipoMovimiento = _clasificacionMovimientoCaja.FirstOrDefault(f => f.NumeroTransacion == (short)Convert.ToInt32(_view.cmbbTipoMovimiento.SelectedValue.ToString()))?.NumeroTransacion;
                mod.Concepto = _view.txtbConcepto.Text;
                mod.EntradaSalida = _view.cmbbTipoMovimiento.SelectedValue.ToString().Substring(_view.cmbbTipoMovimiento.SelectedValue.ToString().IndexOf("(") + 1, 1);

                //Datos Fiscales
                if (_view.chbDatosFiscales.Checked)
                {
                    mod.Rnc = _view.txtbRNC.Text;
                    mod.Ncf = _view.txtbNCF.Text;
                    mod.Clasificancf = _clasificacionFiscal.FirstOrDefault(s => s.Descripcion == _view.cmbbClasifFiscal.SelectedValue.ToString())?.NumeroTransacion;
                    mod.Neto = Convert.ToDecimal(_view.txtbValorSNITBIS.Text);
                    mod.Itebis = _view.txtbITBISFactur.Text;

                    _view.txtbRNC.Enabled = false;
                    _view.txtbNCF.Enabled = false;
                    _view.cmbbClasifFiscal.Enabled = false;
                    _view.txtbValorSNITBIS.Enabled = false;
                    _view.txtbITBISFactur.Enabled = false;
                }
                else
                {
                    mod.Rnc = null;
                    mod.Ncf = null;
                    mod.Clasificancf = null;
                    mod.Neto = null;
                    mod.Itebis = null;
                }

                _dbContext.SaveChanges();
                ResetVentana();
            });

            _view.btnBuscarMovimientos.Click += new EventHandler((object sender, EventArgs e) =>
            {
                var filtroSeleccionado = _view.cmbbFiltroMovimientos.SelectedValue.ToString();
                var fechaDesde = _view.dtpFechaDesde.Value.ToString("yyy-MM-dd");
                var fechaHasta = _view.dtpFechaHasta.Value.ToString("yyy-MM-dd");
                string tipoMovimientoSeleccionado = "";
                StringBuilder _query = new StringBuilder();
                _query.Append("SELECT * " +
                              "FROM [monicaextra].[movimientocaja] " +
                             $"WHERE Fecha >= '{fechaDesde}' AND Fecha <= '{fechaHasta}' ");

                switch (filtroSeleccionado)
                {
                    case "2":
                        tipoMovimientoSeleccionado = _view.cmbbFiltroSeleccionado.SelectedValue.ToString();
                        _query.Append(" AND " +
                                     $"TipoMovimiento = '{tipoMovimientoSeleccionado}'");
                        break;
                    case "3":
                        tipoMovimientoSeleccionado = _view.cmbbFiltroSeleccionado.SelectedValue.ToString();
                        _query.Append(" AND " +
                                     $"EntradaSalida = '{tipoMovimientoSeleccionado}' ");
                        break;
                    case "4":
                        tipoMovimientoSeleccionado = _view.cmbbFiltroSeleccionado.SelectedValue.ToString();
                        _query.Append(" AND " +
                                     $"Beneficiario = '{tipoMovimientoSeleccionado}' ");
                        break;
                    case "5":
                        tipoMovimientoSeleccionado = _view.cmbbFiltroSeleccionado.SelectedValue.ToString();
                        _query.Append(" AND " +
                                     $"Rnc = '{tipoMovimientoSeleccionado}' ");
                        break;
                    case "6":
                        tipoMovimientoSeleccionado = _view.cmbbFiltroSeleccionado.SelectedValue.ToString();
                        _query.Append(" AND " +
                                     $"Ncf = '{tipoMovimientoSeleccionado}' ");
                        break;
                    case "7":
                        tipoMovimientoSeleccionado = _view.cmbbFiltroSeleccionado.SelectedValue.ToString();
                        _query.Append(" AND " +
                                     $"NumeroCaja = '{tipoMovimientoSeleccionado}' ");
                        break;
                }

                _query.Append("ORDER BY NumeroTransacion DESC ");

                using (var _dbontext2 = new monicaextraEntities())
                    _view.dataGridView1.DataSource = _dbontext2.Database.SqlQuery<movimientocaja>(_query.ToString()).Select(s => new { s.NumeroTransacion, s.Beneficiario, s.Concepto, s.Monto, s.Fecha }).ToList();
            });

            _view.btnAtras.Click += new EventHandler((object sender, EventArgs e) =>
            {
                _ventanaAnterior.Show();
                _view.Dispose();
            });

            _view.btnMovimientoAImprimir.Click += new EventHandler((object sender, EventArgs e) =>
            {
                var ObjMovimiento = new Model.Reportes.MovimientoSeleccionado
                {
                    NroTransaccion = objMovimientoSeleccionado.NumeroTransacion.ToString(),
                    Beneficiario = objMovimientoSeleccionado.Beneficiario,
                    Concepto = objMovimientoSeleccionado.Concepto,
                    Monto = objMovimientoSeleccionado.Monto.ToString(),
                    Fecha = objMovimientoSeleccionado.Fecha
                };

                List<Model.Reportes.MovimientoSeleccionado> movimiento = new List<Model.Reportes.MovimientoSeleccionado> { ObjMovimiento };

                if (_view.chbDatosFiscales.Checked)
                {
                    ObjMovimiento.RNC = objMovimientoSeleccionado.Rnc;
                    ObjMovimiento.NCF = objMovimientoSeleccionado.Ncf;
                    ObjMovimiento.ClsfFiscal = objMovimientoSeleccionado.Clasificancf.ToString();
                    ObjMovimiento.VSinItbis = objMovimientoSeleccionado.Neto.ToString();
                    ObjMovimiento.ITBISFacturado = objMovimientoSeleccionado.Itebis;
                };

                Utils.GlobalVariables.SetEmpresaValues(ObjMovimiento);

                new MovimientoSeleccionado(movimiento).Show();
            });
            #endregion

            #region TextBoxes
            _view.txtbMonto.TextChanged += new EventHandler((object sender, EventArgs e) =>
            {
                HabilitarBtnGuardarMovimiento();
            });

            _view.txtbConcepto.TextChanged += new EventHandler((object sender, EventArgs e) =>
            {
                HabilitarBtnGuardarMovimiento();
            });

            _view.txtbMonto.KeyPress += Utils.UtilsMethods.OnlyNumbers_AndBackSpace();
            _view.txtbValorSNITBIS.KeyPress += Utils.UtilsMethods.OnlyNumbers_AndBackSpace();
            _view.txtbITBISFactur.KeyPress += Utils.UtilsMethods.OnlyNumbers_AndBackSpace();
            #endregion

            #region ComboBox
            _view.cmbbFiltroMovimientos.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) =>
            {
                Dictionary<string, string> dictionaryCmbb = new Dictionary<string, string>();
                if (_view.cmbbFiltroMovimientos.SelectedValue.ToString() == "1")
                    _view.cmbbFiltroSeleccionado.Visible = false;
                else if (_view.cmbbFiltroMovimientos.SelectedValue.ToString() == "2")
                {
                    _clasificacionMovimientoCaja.Where(x => x.Visible == 1).ToList()
                                                                           .ForEach(f =>
                                                                           {
                                                                               dictionaryCmbb.Add(f.NumeroTransacion.ToString(), f.Descripcion);
                                                                           });
                }
                else if (_view.cmbbFiltroMovimientos.SelectedValue.ToString() == "3")
                {
                    dictionaryCmbb.Add("E", "Entradas");
                    dictionaryCmbb.Add("S", "Salidas");
                }
                else if (_view.cmbbFiltroMovimientos.SelectedValue.ToString() == "4")
                    _usuarios.ForEach(f =>
                    {
                        dictionaryCmbb.Add(f.id_usuario.ToString(), f.nombre_completo);
                    });
                else if (_view.cmbbFiltroMovimientos.SelectedValue.ToString() == "5")
                    _clasificacionRNC.ForEach(f =>
                    {
                        dictionaryCmbb.Add(f, f);
                    });
                else if (_view.cmbbFiltroMovimientos.SelectedValue.ToString() == "6")
                    _clasificacionFiscal.ToList()
                                        .ForEach(f =>
                                        {
                                            dictionaryCmbb.Add(f.NumeroTransacion.ToString(), f.Descripcion);
                                        });
                else if (_view.cmbbFiltroMovimientos.SelectedValue.ToString() == "7")
                    _clasificacionNroCaja.ToList()
                                        .ForEach(f =>
                                        {
                                            dictionaryCmbb.Add(f, f);
                                        });

                if (dictionaryCmbb.Count > 0)
                {
                    _view.cmbbFiltroSeleccionado.DataSource = new BindingSource(dictionaryCmbb, null);
                    _view.cmbbFiltroSeleccionado.DisplayMember = "Value";
                    _view.cmbbFiltroSeleccionado.ValueMember = "Key";
                    _view.cmbbFiltroSeleccionado.Visible = true;
                }
            });
            #endregion

            #region DataGridViews
            _view.dataGridView1.CellClick += new DataGridViewCellEventHandler((object sender, DataGridViewCellEventArgs e) =>
            {
                if (e.RowIndex < 0) return;
                var idObj = (short)Convert.ToInt32(_view.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                objMovimientoSeleccionado = _dbContext.movimientocajas.FirstOrDefault(s => s.NumeroTransacion == idObj);
                _view.dataGridView1.CurrentRow.Selected = true;

                //  Datos Basicos
                _view.dtpEmicion.Value = new DateTime(Convert.ToDateTime(objMovimientoSeleccionado.Fecha).Year, Convert.ToDateTime(objMovimientoSeleccionado.Fecha).Month, Convert.ToDateTime(objMovimientoSeleccionado.Fecha).Day);
                _view.cmbbCargadoA.SelectedValue = Convert.ToInt16(objMovimientoSeleccionado.Beneficiario);
                _view.txtbMonto.Text = objMovimientoSeleccionado.Monto.ToString();
                _view.cmbbTipoMovimiento.SelectedValue = objMovimientoSeleccionado.TipoMovimiento.Value;
                _view.txtbConcepto.Text = objMovimientoSeleccionado.Concepto.ToString();

                //  Datos Fiscales
                if (!string.IsNullOrEmpty(objMovimientoSeleccionado.Rnc))
                {
                    _view.txtbRNC.Text = objMovimientoSeleccionado.Rnc;
                    _view.txtbNCF.Text = objMovimientoSeleccionado.Ncf;
                    _view.cmbbClasifFiscal.SelectedIndex = objMovimientoSeleccionado.Clasificancf.Value;
                    _view.txtbValorSNITBIS.Text = objMovimientoSeleccionado.Neto.ToString();
                    _view.txtbITBISFactur.Text = objMovimientoSeleccionado.Itebis;
                    _view.chbDatosFiscales.Checked = true;
                }
                else
                {
                    _view.txtbRNC.Text = "";
                    _view.txtbNCF.Text = "";
                    _view.cmbbClasifFiscal.SelectedIndex = 0;
                    _view.txtbValorSNITBIS.Text = "";
                    _view.txtbITBISFactur.Text = "";
                    _view.chbDatosFiscales.Checked = false;
                }

                _view.btnLimpiarCampos.Enabled = true;
                _view.btnModificar.Enabled = true;
                _view.btnGuardar.Enabled = false;
            });
            #endregion

            _view.FormClosing += new FormClosingEventHandler((object sender, FormClosingEventArgs e) => { Dispose(); });
        }

        /// <summary>
        /// Habilita/Deshabilita los TextBox del panel de 'Datos Fiscales'
        /// </summary>
        /// <param name="estado"></param>
        private void SwitchEstadoCamposFiscales(bool estado)
        {
            _view.txtbRNC.Enabled = estado;
            _view.txtbNCF.Enabled = estado;
            _view.cmbbClasifFiscal.Enabled = estado;
            _view.txtbValorSNITBIS.Enabled = estado;
            _view.txtbITBISFactur.Enabled = estado;
        }

        /// <summary>
        /// Habilita/Deshabilita el boton para guardar el movimiento
        /// </summary>
        private void HabilitarBtnGuardarMovimiento()
        {
            if (objMovimientoSeleccionado != null)
                return;

            if (!string.IsNullOrWhiteSpace(_view.txtbMonto.Text) &&
                !string.IsNullOrWhiteSpace(_view.txtbConcepto.Text))
                _view.btnGuardar.Enabled = true;
            else
                _view.btnGuardar.Enabled = false;
        }

        /// <summary>
        /// Pone la ventana en su estado inicial, limpiando los campos, y volviendo a llenar la lista.
        /// </summary>
        private void ResetVentana()
        {
            LlenarComponentesConDB(false);
            objMovimientoSeleccionado = null;

            //  Datos Basicos
            _view.dtpEmicion.Value = DateTime.Now;
            _view.cmbbCargadoA.SelectedIndex = 0;
            _view.txtbMonto.Text = "";
            _view.cmbbTipoMovimiento.SelectedIndex = 0;
            _view.txtbConcepto.Text = "";

            //  Datos Fiscales
            if (_view.chbDatosFiscales.Checked)
            {
                _view.txtbRNC.Text = "";
                _view.txtbNCF.Text = "";
                _view.cmbbClasifFiscal.SelectedIndex = 0;
                _view.txtbValorSNITBIS.Text = "";
                _view.txtbITBISFactur.Text = "";
                _view.chbDatosFiscales.Checked = false;
            }

            _view.btnModificar.Enabled = false;
            _view.btnGuardar.Enabled = false;
            _view.btnLimpiarCampos.Enabled = false;
        }
    }
}
