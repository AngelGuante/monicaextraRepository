using System.Windows.Forms;

namespace MonicaExtra.Conrtroller
{
    class ControllersBase
    {
        /// <summary>
        /// Cierra todas las conexiones y ventanas en el proyecto.
        /// </summary>
        protected void Dispose()
        {
            Application.Exit();
        }
    }
}
