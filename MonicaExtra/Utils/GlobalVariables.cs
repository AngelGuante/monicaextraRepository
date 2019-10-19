using MonicaExtra.Model.monica_global;

namespace MonicaExtra.Utils
{
    public static class GlobalVariables
    {
        public static empresa _empresaSeleccionada { get; set; }

        /// <summary>
        /// Asigna las propiedades de la empresa a un objeto pasado por parametro, para fines de los reportes.
        /// </summary>
        public static void SetEmpresaValues(empresa ObjMovimiento)
        {
            ObjMovimiento.Nombre_empresa = _empresaSeleccionada.Nombre_empresa;
            ObjMovimiento.direccion1 = _empresaSeleccionada.direccion1;
            ObjMovimiento.direccion2 = _empresaSeleccionada.direccion2;
            ObjMovimiento.direccion3 = _empresaSeleccionada.direccion3;
            ObjMovimiento.Telefono1 = _empresaSeleccionada.Telefono1;
        }
    }
}
