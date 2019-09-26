using System.Windows.Forms;

namespace MonicaExtra.Utils
{
    class UtilsMethods
    {
        /// <summary>
        /// Retorna un KeyPressEventHandler para solo permitir numeros en los textboxes.
        /// </summary>
        /// <returns></returns>
        public static KeyPressEventHandler OnlyNumbers_AndBackSpace() =>
            new KeyPressEventHandler((object sender, KeyPressEventArgs e) =>
            {
                if (!(char.IsDigit(e.KeyChar) || e.KeyChar == 8))
                    e.Handled = true;
            });
    }
}
