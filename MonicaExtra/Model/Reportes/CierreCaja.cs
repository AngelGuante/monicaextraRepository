using MonicaExtra.Model.monica_global;

namespace MonicaExtra.Model.Reportes
{
    public class CierreCaja : EmpresaModel
    {
        public string NumeroCierre { get; set; }
        public string FechaProceso { get; set; }
        public string SaldoFinal { get; set; }
        public string Comentario { get; set; }
    }
}
