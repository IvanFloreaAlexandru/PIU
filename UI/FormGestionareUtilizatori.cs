using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LibrarieModele;
using NivelStocareDate;

namespace UI
{
    public partial class FormGestionareUtilizatori : Form
    {
        private AdministrareUser_FisierText adminUtilizatori;
        private AdministrareUser_Memorie adminMemorie;


        public FormGestionareUtilizatori(AdministrareUser_FisierText utilizatoriAdmin, AdministrareUser_Memorie utilizatoriAdminMemorie)
        {
            adminUtilizatori = utilizatoriAdmin;
            adminMemorie = utilizatoriAdminMemorie;

            ConfigureazaMeniu();
        }

        private void ConfigureazaMeniu()
        {
            this.Text = "Gestionare Utilizatori";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            FlowLayoutPanel panelMeniu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
            };

            Button btnAdaugareUtilizator = new Button { Text = "Adaugare utilizator nou", Size = new Size(250, 40) };
            Button btnVizualizareUtilizatori = new Button { Text = "Vizualizare toti utilizatorii", Size = new Size(250, 40) };
            Button btnModificareUtilizator = new Button { Text = "Modificare utilizator", Size = new Size(250, 40) };
            Button btnInchide = new Button { Text = "Inchide", Size = new Size(250, 40) };

            btnAdaugareUtilizator.Click += BtnAdaugareUtilizator_Click;
            btnVizualizareUtilizatori.Click += BtnVizualizareUtilizatori_Click;
            btnModificareUtilizator.Click += BtnModificareUtilizatori_Click;
            btnInchide.Click += (s, e) => this.Close();

            panelMeniu.Controls.Add(btnAdaugareUtilizator);
            panelMeniu.Controls.Add(btnVizualizareUtilizatori);
            panelMeniu.Controls.Add(btnModificareUtilizator);
            panelMeniu.Controls.Add(btnInchide);

            this.Controls.Add(panelMeniu);
        }

        public static class ConstanteValidare
        {
            public const int MINIM_CARACTERE_NUME = 3;
            public const int MINIM_CARACTERE_PRENUME = 3;
            public const int MINIM_CARACTERE_PAROLA = 8;
            public const string REGEX_EMAIL = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            public const string REGEX_TELEFON = @"^(\+4|)?(07[0-8]{1}[0-9]{1}|02[0-9]{2}|03[0-9]{2}){1}?(\s|\.|\-)?([0-9]{3}(\s|\.|\-|)){2}$";
        }

        private void BtnAdaugareUtilizator_Click(object sender, EventArgs e)
        {
            string[] potentialPaths = new string[]
            {
        Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "User.txt")
            };

            string caleCompletaFisierUtilizatori = potentialPaths.FirstOrDefault(File.Exists);

            if (string.IsNullOrEmpty(caleCompletaFisierUtilizatori))
            {
                MessageBox.Show("Nu s-a putut gasi fisierul User.txt!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AdministrareUser_FisierText adminUser = new AdministrareUser_FisierText(caleCompletaFisierUtilizatori);
            AdministrareUser_Memorie adminUserMemorie = new AdministrareUser_Memorie();

            int nrUtilizatori;
            var utilizatoriDinFisier = adminUser.GetUsers(out nrUtilizatori);
            foreach (var user in utilizatoriDinFisier)
            {
                adminUserMemorie.AddUser(user);
            }

            using (Form formUtilizator = new Form())
            {
                formUtilizator.Text = "Adaugare Utilizator Nou";
                formUtilizator.Size = new Size(450, 500);
                formUtilizator.StartPosition = FormStartPosition.CenterScreen;

                Panel panelFormular = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true
                };

                Label lblNumeUtilizator = new Label { Text = "Nume *:", Location = new Point(10, 10), Width = 150, Name = "lblNumeUtilizator" };
                Label lblPrenumeUtilizator = new Label { Text = "Prenume *:", Location = new Point(10, 40), Width = 150, Name = "lblPrenumeUtilizator" };
                Label lblEmailUtilizator = new Label { Text = "Email *:", Location = new Point(10, 70), Width = 150, Name = "lblEmailUtilizator" };
                Label lblTelefonUtilizator = new Label { Text = "Telefon:", Location = new Point(10, 100), Width = 150, Name = "lblTelefonUtilizator" };
                Label lblParolaUtilizator = new Label { Text = "Parola *:", Location = new Point(10, 130), Width = 150, Name = "lblParolaUtilizator" };
                Label lblConfirmareParola = new Label { Text = "Confirmare Parola *:", Location = new Point(10, 160), Width = 150, Name = "lblConfirmareParola" };
                Label lblRangUtilizator = new Label { Text = "Rang *:", Location = new Point(10, 190), Width = 150, Name = "lblRangUtilizator" };

                Label[] eticheteCampuri = new Label[] {
            lblNumeUtilizator,
            lblPrenumeUtilizator,
            lblEmailUtilizator,
            lblTelefonUtilizator,
            lblParolaUtilizator,
            lblConfirmareParola,
            lblRangUtilizator
        };

                TextBox txtNumeUtilizator = new TextBox { Location = new Point(200, 10), Width = 220, Name = "txtNumeUtilizator" };
                TextBox txtPrenumeUtilizator = new TextBox { Location = new Point(200, 40), Width = 220, Name = "txtPrenumeUtilizator" };
                TextBox txtEmailUtilizator = new TextBox { Location = new Point(200, 70), Width = 220, Name = "txtEmailUtilizator" };
                TextBox txtTelefonUtilizator = new TextBox { Location = new Point(200, 100), Width = 220, Name = "txtTelefonUtilizator" };
                TextBox txtParolaUtilizator = new TextBox { Location = new Point(200, 130), Width = 220, PasswordChar = '*', Name = "txtParolaUtilizator" };
                TextBox txtConfirmareParola = new TextBox { Location = new Point(200, 160), Width = 220, PasswordChar = '*', Name = "txtConfirmareParola" };

                TextBox[] campuriText = new TextBox[] {
            txtNumeUtilizator,
            txtPrenumeUtilizator,
            txtEmailUtilizator,
            txtTelefonUtilizator,
            txtParolaUtilizator,
            txtConfirmareParola
        };

                Label lblMesajEroare = new Label
                {
                    Location = new Point(10, 220),
                    Width = 410,
                    ForeColor = Color.Red,
                    Visible = false,
                    Name = "lblMesajEroare"
                };
                panelFormular.Controls.Add(lblMesajEroare);

                ComboBox cboRangUtilizator = new ComboBox
                {
                    Location = new Point(200, 190),
                    Width = 220,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Name = "cboRangUtilizator"
                };

                foreach (RangUtilizator rang in Enum.GetValues(typeof(RangUtilizator)))
                {
                    cboRangUtilizator.Items.Add(rang);
                }
                cboRangUtilizator.SelectedIndex = 0;

                txtEmailUtilizator.Leave += (s, ev) =>
                {
                    if (!string.IsNullOrWhiteSpace(txtEmailUtilizator.Text) &&
                        !System.Text.RegularExpressions.Regex.IsMatch(txtEmailUtilizator.Text, ConstanteValidare.REGEX_EMAIL))
                    {
                        lblEmailUtilizator.ForeColor = Color.Red;
                        txtEmailUtilizator.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "Adresa de email nu este valida!";
                        lblMesajEroare.Visible = true;
                    }
                    else
                    {
                        lblEmailUtilizator.ForeColor = SystemColors.ControlText;
                        txtEmailUtilizator.BackColor = SystemColors.Window;
                        lblMesajEroare.Visible = false;
                    }
                };

                txtTelefonUtilizator.Leave += (s, ev) =>
                {
                    if (!string.IsNullOrWhiteSpace(txtTelefonUtilizator.Text) &&
                        !System.Text.RegularExpressions.Regex.IsMatch(txtTelefonUtilizator.Text, ConstanteValidare.REGEX_TELEFON))
                    {
                        lblTelefonUtilizator.ForeColor = Color.Red;
                        txtTelefonUtilizator.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "Numarul de telefon nu este valid!";
                        lblMesajEroare.Visible = true;
                    }
                    else
                    {
                        lblTelefonUtilizator.ForeColor = SystemColors.ControlText;
                        txtTelefonUtilizator.BackColor = SystemColors.Window;
                        lblMesajEroare.Visible = false;
                    }
                };

                for (int i = 0; i < eticheteCampuri.Length; i++)
                {
                    panelFormular.Controls.Add(eticheteCampuri[i]);
                    if (i < campuriText.Length)
                        panelFormular.Controls.Add(campuriText[i]);
                }
                panelFormular.Controls.Add(cboRangUtilizator);

                Button btnSalveazaUtilizator = new Button
                {
                    Text = "Salveaza Utilizatorul",
                    Location = new Point(10, 250),
                    Width = 410,
                    Name = "btnSalveazaUtilizator"
                };

                btnSalveazaUtilizator.Click += (s, ev) =>
                {
                    foreach (Label lbl in eticheteCampuri)
                    {
                        lbl.ForeColor = SystemColors.ControlText;
                    }
                    foreach (TextBox txt in campuriText)
                    {
                        txt.BackColor = SystemColors.Window;
                    }
                    lblMesajEroare.Visible = false;

                    bool valid = true;
                    string mesajEroare = "";

                    if (string.IsNullOrWhiteSpace(txtNumeUtilizator.Text))
                    {
                        lblNumeUtilizator.ForeColor = Color.Red;
                        txtNumeUtilizator.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Numele este obligatoriu!";
                    }
                    else if (txtNumeUtilizator.Text.Length < ConstanteValidare.MINIM_CARACTERE_NUME)
                    {
                        lblNumeUtilizator.ForeColor = Color.Red;
                        txtNumeUtilizator.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = $"Numele trebuie sa aiba minim {ConstanteValidare.MINIM_CARACTERE_NUME} caractere!";
                    }

                    if (string.IsNullOrWhiteSpace(txtPrenumeUtilizator.Text))
                    {
                        lblPrenumeUtilizator.ForeColor = Color.Red;
                        txtPrenumeUtilizator.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Prenumele este obligatoriu!";
                    }
                    else if (txtPrenumeUtilizator.Text.Length < ConstanteValidare.MINIM_CARACTERE_PRENUME)
                    {
                        lblPrenumeUtilizator.ForeColor = Color.Red;
                        txtPrenumeUtilizator.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = $"Prenumele trebuie sa aiba minim {ConstanteValidare.MINIM_CARACTERE_PRENUME} caractere!";
                    }

                    if (string.IsNullOrWhiteSpace(txtEmailUtilizator.Text))
                    {
                        lblEmailUtilizator.ForeColor = Color.Red;
                        txtEmailUtilizator.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Adresa de email este obligatorie!";
                    }
                    else if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmailUtilizator.Text, ConstanteValidare.REGEX_EMAIL))
                    {
                        lblEmailUtilizator.ForeColor = Color.Red;
                        txtEmailUtilizator.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Adresa de email nu este valida!";
                    }

                    if (!string.IsNullOrWhiteSpace(txtTelefonUtilizator.Text) &&
                        !System.Text.RegularExpressions.Regex.IsMatch(txtTelefonUtilizator.Text, ConstanteValidare.REGEX_TELEFON))
                    {
                        lblTelefonUtilizator.ForeColor = Color.Red;
                        txtTelefonUtilizator.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Numarul de telefon nu este valid!";
                    }

                    if (string.IsNullOrWhiteSpace(txtParolaUtilizator.Text))
                    {
                        lblParolaUtilizator.ForeColor = Color.Red;
                        txtParolaUtilizator.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Parola este obligatorie!";
                    }
                    else if (txtParolaUtilizator.Text.Length < ConstanteValidare.MINIM_CARACTERE_PAROLA)
                    {
                        lblParolaUtilizator.ForeColor = Color.Red;
                        txtParolaUtilizator.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = $"Parola trebuie sa aiba minim {ConstanteValidare.MINIM_CARACTERE_PAROLA} caractere!";
                    }

                    if (string.IsNullOrWhiteSpace(txtConfirmareParola.Text))
                    {
                        lblConfirmareParola.ForeColor = Color.Red;
                        txtConfirmareParola.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Confirmarea parolei este obligatorie!";
                    }
                    else if (txtParolaUtilizator.Text != txtConfirmareParola.Text)
                    {
                        lblConfirmareParola.ForeColor = Color.Red;
                        txtConfirmareParola.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Parolele nu coincid!";
                    }

                    if (!valid)
                    {
                        lblMesajEroare.Text = mesajEroare;
                        lblMesajEroare.Visible = true;
                        return;
                    }

                    User[] utilizatoriExistenti = adminUserMemorie.GetUsers(out int nrUtilizatoriMemorie);
                    int nuoulId = nrUtilizatoriMemorie > 0 ? utilizatoriExistenti[nrUtilizatoriMemorie - 1].IdUser + 1 : 1;

                    User utilizatorNou = new User
                    {
                        IdUser = nuoulId,
                        Nume = txtNumeUtilizator.Text.Trim(),
                        Prenume = txtPrenumeUtilizator.Text.Trim(),
                        Email = txtEmailUtilizator.Text.Trim(),
                        Telefon = txtTelefonUtilizator.Text.Trim(),
                        Parola = txtParolaUtilizator.Text,
                        Rang = (RangUtilizator)cboRangUtilizator.SelectedItem
                    };

                    try
                    {
                        adminUser.AddUser(utilizatorNou);
                        adminUserMemorie.AddUser(utilizatorNou);
                        MessageBox.Show("Utilizator adaugat cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formUtilizator.DialogResult = DialogResult.OK;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Eroare la adaugarea utilizatorului: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                panelFormular.Controls.Add(btnSalveazaUtilizator);
                formUtilizator.Controls.Add(panelFormular);
                formUtilizator.ShowDialog();
            }
        }


        private void BtnVizualizareUtilizatori_Click(object sender, EventArgs e)
        {
            try
            {
                string[] potentialPaths = new string[]
                {
            Path.Combine(Directory.GetCurrentDirectory(), "User.txt"),
            Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "User.txt")
                };

                string caleCompletaFisier = potentialPaths.FirstOrDefault(File.Exists);
                if (string.IsNullOrEmpty(caleCompletaFisier))
                {
                    MessageBox.Show("Fisierul User.txt nu a fost gasit!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string[] continutFisier = File.ReadAllLines(caleCompletaFisier);
                if (continutFisier.Length == 0)
                {
                    MessageBox.Show("Fisierul este gol!", "Informatie");
                    return;
                }

                using (Form formVizualizare = new Form())
                {
                    formVizualizare.Text = "Lista Utilizatori";
                    formVizualizare.Size = new Size(700, 400);

                    ListView listViewUtilizatori = new ListView
                    {
                        Dock = DockStyle.Fill,
                        View = View.Details,
                        FullRowSelect = true
                    };

                    listViewUtilizatori.Columns.Add("ID Utilizator", 100);
                    listViewUtilizatori.Columns.Add("Nume", 150);
                    listViewUtilizatori.Columns.Add("Prenume", 150);
                    listViewUtilizatori.Columns.Add("Email", 200);
                    listViewUtilizatori.Columns.Add("Rang", 100);

                    foreach (string linie in continutFisier)
                    {
                        string[] detaliiUtilizator = linie.Split(',');

                        if (detaliiUtilizator.Length >= 6)
                        {
                            ListViewItem item = new ListViewItem(detaliiUtilizator[0]);
                            item.SubItems.Add(detaliiUtilizator[1]);
                            item.SubItems.Add(detaliiUtilizator[2]);
                            item.SubItems.Add(detaliiUtilizator[3]);
                            item.SubItems.Add(detaliiUtilizator[5]);

                            listViewUtilizatori.Items.Add(item);
                        }
                    }

                    formVizualizare.Controls.Add(listViewUtilizatori);

                    formVizualizare.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare: {ex.Message}\n\nDetalii: {ex.StackTrace}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnModificareUtilizatori_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");

        }
    }
}