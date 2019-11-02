using MonicaExtra.Model.monica_global;
using MonicaExtra.Model.monicaextra;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace MonicaExtra.Utils.DB
{
    class Connection
    {
        private SqlConnection _monica10Conn { get; set; } = new SqlConnection("Data Source=INGALMONTE\\TECHNOTEL;Initial Catalog=monica10;User ID=sa;Passwor d=Admin2012");
        private SqlConnection _monica10_GlovalConn { get; set; } = new SqlConnection("Data Source=INGALMONTE\\TECHNOTEL;Initial Catalog=monica_global;User ID=sa;Passwor d=Admin2012");

        private SqlConnection GetConnection(string db)
        {
            switch (db)
            {
                case "monica10":
                    return _monica10Conn;
                case "monica10_global":
                    return _monica10_GlovalConn;
                default:
                    return null;
            }
        }

        public object ExecuteQuery(StringBuilder query, string db, string table)
        {
            try
            {
                SqlConnection conn = GetConnection(db);

                using (var cmmnd = new SqlCommand(query.ToString(), conn))
                {
                    cmmnd.Connection.Open();
                    switch (table)
                    {
                        case "empresa":
                            List<EmpresaModel> empresas = new List<EmpresaModel>();
                            using (var reader = cmmnd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    empresas.Add(
                                        new EmpresaModel
                                        {
                                            empresa_id = Convert.ToInt32((reader["empresa_id"].ToString())),
                                            Nombre_empresa = (reader["Nombre_empresa"].ToString())
                                        });
                                }
                            }
                            cmmnd.Connection.Dispose();
                            return empresas;
                        case "movimientocaja":
                            var Select = query.ToString(query.ToString().IndexOf("SELECT ") + 7, query.ToString().IndexOf(" FROM") - 7).Replace(" ", "").Replace("TOP", "").Replace("50", "").Replace("DISTINCT", "").Split(',');
                            List<MovimientoCajaModel> movimientos = new List<MovimientoCajaModel>();
                            using (var reader = cmmnd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var obj = new MovimientoCajaModel();
                                    foreach (var slct in Select)
                                    {
                                        if (slct == "NumeroTransacion")
                                            obj.NumeroTransacion = (short)reader["NumeroTransacion"];
                                        else if (slct == "Beneficiario")
                                            obj.Beneficiario = reader["Beneficiario"].ToString();
                                        else if (slct == "Concepto")
                                            obj.Concepto = reader["Concepto"].ToString();
                                        else if (slct == "Rnc")
                                            obj.Rnc = reader["Rnc"].ToString();
                                        else if (slct == "Ncf")
                                            obj.Ncf = reader["Ncf"].ToString();
                                        else if (slct == "Monto")
                                            obj.Monto = decimal.Parse(reader["Monto"].ToString());
                                        else if (slct == "Fecha")
                                            obj.Fecha = reader["Fecha"].ToString();
                                        else if (slct == "NumeroCaja")
                                            obj.NumeroCaja = (short)reader["NumeroCaja"];
                                        else if (slct == "TipoMovimiento")
                                            obj.TipoMovimiento = (short)reader["TipoMovimiento"];
                                        else if (slct == "Clasificancf")
                                            obj.Clasificancf = (short)reader["Clasificancf"];
                                        else if (slct == "Itebis")
                                            obj.Itebis = reader["Itebis"].ToString();
                                        else if (slct == "Neto")
                                            obj.Neto = (decimal)reader["Neto"];
                                        else if (slct == "EntradaSalida")
                                            obj.EntradaSalida = reader["EntradaSalida"].ToString();
                                        else if (slct == "CodigoCajero")
                                            obj.CodigoCajero = reader["CodigoCajero"].ToString();
                                        else if (slct == "TipoMoneda")
                                            obj.TipoMoneda = reader["TipoMoneda"].ToString();
                                        else if (slct == "TasaCambio")
                                            obj.TasaCambio = (decimal)reader["TasaCambio"];
                                        else if (slct == "NumeroCierre")
                                            obj.NumeroCierre = (short)reader["NumeroCierre"];
                                        else if (slct == "Estatus")
                                            obj.Estatus = (short)reader["Estatus"];
                                    }
                                    movimientos.Add(obj);
                                }
                            }
                            cmmnd.Connection.Dispose();
                            return movimientos;
                        case "clasificacionmovicaja":
                            List<ClasificacionmovicajaModel> clasificacionmovicajas = new List<ClasificacionmovicajaModel>();
                            using (var reader = cmmnd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    clasificacionmovicajas.Add(
                                        new ClasificacionmovicajaModel
                                        {
                                            NumeroTransacion = Convert.ToInt16(reader["NumeroTransacion"].ToString()),
                                            Descripcion = reader["Descripcion"].ToString(),
                                            Tipo = reader["Tipo"].ToString(),
                                            Visible = Convert.ToInt16(reader["Visible"].ToString())
                                        });
                                }
                            }
                            cmmnd.Connection.Dispose();
                            return clasificacionmovicajas;
                        case "clasificacionfiscal":
                            List<ClasificacionFiscalModel> clasificacionfiscal = new List<ClasificacionFiscalModel>();
                            using (var reader = cmmnd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    clasificacionfiscal.Add(
                                        new ClasificacionFiscalModel
                                        {
                                            NumeroTransacion = Convert.ToInt16(reader["NumeroTransacion"].ToString()),
                                            Descripcion = reader["Descripcion"].ToString(),
                                            Tipo = reader["Tipo"].ToString()
                                        });
                                }
                            }
                            cmmnd.Connection.Dispose();
                            return clasificacionfiscal;
                        case "cierrecaja":
                            List<CierreCajaModel> CierreCaja = new List<CierreCajaModel>();
                            using (var reader = cmmnd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    CierreCaja.Add(
                                        new CierreCajaModel
                                        {
                                            NumeroCierre = (short)reader["NumeroCierre"],
                                            FechaProceso = reader["FechaProceso"].ToString(),
                                            SaldoFinal = (int)reader["SaldoFinal"],
                                            Comentario = reader["Comentario"].ToString()
                                        });
                                }
                            }
                            cmmnd.Connection.Dispose();
                            return CierreCaja;
                        case "usuarios":
                            List<UsuarioModel> usuarios = new List<UsuarioModel>();
                            using (var reader = cmmnd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    usuarios.Add(
                                        new UsuarioModel
                                        {
                                            id_usuario = Convert.ToInt16(reader["id_usuario"].ToString()),
                                            usuario = reader["usuario"].ToString(),
                                            clave = reader["clave"].ToString(),
                                            categoria = reader["categoria"].ToString(),
                                            activo = Convert.ToBoolean(reader["activo"]),
                                            nombre_completo = reader["nombre_completo"].ToString()
                                        });
                                }
                            }
                            cmmnd.Connection.Dispose();
                            return usuarios;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }

        public int ExecuteScalar(StringBuilder query, string db, string table)
        {
            try
            {
                SqlConnection conn = GetConnection(db);
                int cantidad;
                using (var cmmnd = new SqlCommand(query.ToString(), conn))
                {
                    cmmnd.Connection.Open();
                    switch (table)
                    {
                        case "movimientocaja":
                            cantidad = (int)cmmnd.ExecuteScalar();
                            cmmnd.Connection.Dispose();
                            return cantidad;
                        case "cierrecaja":
                            cantidad = (int)cmmnd.ExecuteScalar();
                            cmmnd.Connection.Dispose();
                            return cantidad;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return -1;
        }

        public void InsertUpdateValues(StringBuilder query, string db, string table, MovimientoCajaModel movimiento, string[] paramsUpdate = null)
        {
            try
            {
                SqlConnection conn = GetConnection(db);
                using (var cmmnd = new SqlCommand(query.ToString(), conn))
                {
                    cmmnd.Connection.Open();
                    switch (table)
                    {
                        case "movimientocaja":
                            string[] campos;
                            if (query.ToString().Contains("INSERT"))
                                campos = query.ToString(query.ToString().IndexOf("(") + 1, query.ToString().IndexOf(") VALUES(") - 20).Replace(" ", "").Split(',');
                            else
                                campos = paramsUpdate;

                            foreach (var slct in campos)
                            {
                                if (slct == "NumeroTransacion")
                                    cmmnd.Parameters.Add(new SqlParameter("NumeroTransacion", movimiento.NumeroTransacion));
                                else if (slct == "Beneficiario")
                                    cmmnd.Parameters.Add(new SqlParameter("Beneficiario", movimiento.Beneficiario));
                                else if (slct == "Concepto")
                                    cmmnd.Parameters.Add(new SqlParameter("Concepto", movimiento.Concepto));
                                else if (slct == "Rnc")
                                    cmmnd.Parameters.Add(new SqlParameter("Rnc", movimiento.Rnc ?? ""));
                                else if (slct == "Ncf")
                                    cmmnd.Parameters.Add(new SqlParameter("Ncf", movimiento.Ncf ?? ""));
                                else if (slct == "Monto")
                                    cmmnd.Parameters.Add(new SqlParameter("Monto", movimiento.Monto));
                                else if (slct == "Fecha")
                                    cmmnd.Parameters.Add(new SqlParameter("Fecha", movimiento.Fecha));
                                else if (slct == "NumeroCaja")
                                    cmmnd.Parameters.Add(new SqlParameter("NumeroCaja", movimiento.NumeroCaja));
                                else if (slct == "TipoMovimiento")
                                    cmmnd.Parameters.Add(new SqlParameter("TipoMovimiento", movimiento.TipoMovimiento));
                                else if (slct == "Clasificancf")
                                    cmmnd.Parameters.Add(new SqlParameter("Clasificancf", movimiento.Clasificancf ?? 0));
                                else if (slct == "Itebis")
                                    cmmnd.Parameters.Add(new SqlParameter("Itebis", movimiento.Itebis ?? ""));
                                else if (slct == "Neto")
                                    cmmnd.Parameters.Add(new SqlParameter("Neto", movimiento.Neto ?? 0));
                                else if (slct == "EntradaSalida")
                                    cmmnd.Parameters.Add(new SqlParameter("EntradaSalida", movimiento.EntradaSalida));
                                else if (slct == "CodigoCajero")
                                    cmmnd.Parameters.Add(new SqlParameter("CodigoCajero", movimiento.CodigoCajero ?? ""));
                                else if (slct == "TipoMoneda")
                                    cmmnd.Parameters.Add(new SqlParameter("TipoMoneda", movimiento.TipoMoneda));
                                else if (slct == "TasaCambio")
                                    cmmnd.Parameters.Add(new SqlParameter("TasaCambio", movimiento.TasaCambio));
                                else if (slct == "NumeroCierre")
                                    cmmnd.Parameters.Add(new SqlParameter("NumeroCierre", movimiento.NumeroCierre));
                                else if (slct == "Estatus")
                                    cmmnd.Parameters.Add(new SqlParameter("Estatus", movimiento.Estatus));
                                else if (slct.Contains("Soporte"))
                                    cmmnd.Parameters.Add(new SqlParameter("Soporte", movimiento.Soporte));
                            }
                            cmmnd.ExecuteNonQuery();
                            cmmnd.Connection.Dispose();
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
