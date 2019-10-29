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
                                            empresa_id = Convert.ToInt32((reader[0].ToString())),
                                            Nombre_empresa = (reader[1].ToString())
                                        });
                                }
                            }
                            return empresas;
                        case "movimientocaja":
                            List<MovimientoCajaModel> movimientos = new List<MovimientoCajaModel>();
                            using (var reader = cmmnd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    movimientos.Add(
                                        new MovimientoCajaModel
                                        {
                                            NumeroTransacion = Convert.ToInt16(reader[0].ToString()),
                                            Beneficiario = reader[1].ToString(),
                                            Concepto = reader[2].ToString(),
                                            Monto = decimal.Parse(reader[6].ToString()),
                                            Fecha = reader[11].ToString()
                                        });
                                }
                            }
                            return movimientos;
                    }
                    cmmnd.Connection.Dispose();
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
