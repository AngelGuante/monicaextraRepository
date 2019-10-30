using System;

namespace MonicaExtra.Model.monicaextra
{
    public class UsuarioModel
    {
        public int id_usuario { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }
        public string categoria { get; set; }
        public Nullable<bool> activo { get; set; }
        public string nombre_completo { get; set; }
        public string animal { get; set; }
        public string plato { get; set; }
    }
}
