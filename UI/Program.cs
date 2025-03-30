using System;
using System.Windows.Forms;
using LibrarieModele;


namespace UI
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            User utilizatorCurent = Autentificare.AutentificareUtilizator();
            if (utilizatorCurent != null)
            {
                Application.Run(new FormPrincipal(utilizatorCurent));
            }
        }
    }
}
