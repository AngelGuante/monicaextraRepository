using MonicaExtra.Model.monica_global;

namespace MonicaExtra.Utils
{
    public static class GlobalVariables
    {
        public static EmpresaModel _empresaSeleccionada { get; set; }

        /// <summary>
        /// Asigna las propiedades de la empresa a un objeto pasado por parametro, para fines de los reportes.
        /// </summary>
        public static void SetEmpresaValues(EmpresaModel ObjEmpresa)
        {
            ObjEmpresa.Nombre_empresa = _empresaSeleccionada.Nombre_empresa;
            ObjEmpresa.direccion1 = _empresaSeleccionada.direccion1;
            ObjEmpresa.direccion2 = _empresaSeleccionada.direccion2;
            ObjEmpresa.direccion3 = _empresaSeleccionada.direccion3;
            ObjEmpresa.Telefono1 = _empresaSeleccionada.Telefono1;
        }
    }
}
