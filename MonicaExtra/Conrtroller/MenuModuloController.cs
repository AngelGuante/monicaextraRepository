using MonicaExtra.View;
using System;
using System.Windows.Forms;

namespace MonicaExtra.Conrtroller
{
    class MenuModuloController : ControllersBase
    {
        #region VARIABLES
        private MenuModulo _view;

        public MenuModuloController(MenuModulo view)
        {
            _view = view;

            AplicarEventosAVista();
        }
        #endregion

        /// <summary>
        /// Colocar a los componentes de el VIEW los eventos correspondientes.
        /// </summary>
        private void AplicarEventosAVista()
        {
            #region Buttons
            _view.btnMenuControlCajaChica.Click += new EventHandler((object sender, EventArgs e) => {
                new ControlCajaChicaModulo(_view).Show();
            });
            #endregion

            _view.FormClosing += new FormClosingEventHandler((object sender, FormClosingEventArgs e) => { Dispose(); });
        }
    }
}
