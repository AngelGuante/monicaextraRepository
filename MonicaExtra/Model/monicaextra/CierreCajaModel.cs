using System;

namespace MonicaExtra.Model.monicaextra
{
    class CierreCajaModel
    {
        public short NumeroCierre { get; set; }
        public string FechaProceso { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public Nullable<short> NumeroCaja { get; set; }
        public Nullable<int> SaldoFinal { get; set; }
        public string Comentario { get; set; }
    }
}
