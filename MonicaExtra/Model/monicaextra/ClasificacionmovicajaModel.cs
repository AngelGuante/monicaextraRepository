using System;

namespace MonicaExtra.Model.monicaextra
{
    class ClasificacionmovicajaModel
    {
        public short NumeroTransacion { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public Nullable<short> FijarTipo { get; set; }
        public string Contable { get; set; }
        public Nullable<short> Visible { get; set; }
    }
}
