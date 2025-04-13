using System.Windows.Forms;
using LibrarieModele;
using NivelStocareDate;


namespace UI
{
    public class Autentificare
    {
        public static User AutentificareUtilizator()
        {
            AdministrareUser_FisierText adminUser = new AdministrareUser_FisierText("User.txt");
            int nrUtilizatori;
            var utilizatori = adminUser.GetUsers(out nrUtilizatori);
            int incercari = 0;
            const int MAX_INCERCARI = 3;
            while (incercari < MAX_INCERCARI)
            {
                FormAutentificare formAutentificare = new FormAutentificare();
                formAutentificare.ShowDialog();

                if (formAutentificare.DialogResult == DialogResult.Cancel)
                    return null;

                string email = formAutentificare.metroTextBoxEmail.Text;
                string parola = formAutentificare.metroTextBoxParola.Text;
                foreach (var user in utilizatori)
                {
                    if (user.Email == email && user.Parola == parola)
                    {
                        return user;
                    }
                }
                incercari++;
                MessageBox.Show(formAutentificare,
                    $"Email sau parola incorecte! Mai aveti {MAX_INCERCARI - incercari} incercari.",
                    "Eroare",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            MessageBox.Show(null,
                "Prea multe incercari esuate. Aplicatia se va inchide.",
                "Eroare",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return null;
        }
    }
}