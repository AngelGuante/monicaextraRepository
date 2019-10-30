using MonicaExtra.Model.monica_global;
using MonicaExtra.Model.monicaextra;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonicaExtra.Utils.DB
{
    class Connection
    {
        private SqlConnection _monica10Conn { get; set; } = new SqlConnection("Server=MIKI\\MONICA10;Database=monica10;Trusted_Connection=True;");
        private SqlConnection _monica10_GlovalConn { get; set; } = new SqlConnection("Server=MIKI\\MONICA10;Database=monica10_global;Trusted_Connection=True;");


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
                            var Select = (query.ToString(query.ToString().IndexOf("SELECT ") + 7, query.ToString().IndexOf(" FROM") - 7)).Replace(" ", "").Replace("TOP", "").Replace("50", "").Replace("DISTINCT", "").Split(',');
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
    }
}
