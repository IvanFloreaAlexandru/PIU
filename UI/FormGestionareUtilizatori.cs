using System.Windows.Forms;
using System.Drawing;
using LibrarieModele;
using NivelStocareDate;
using System.IO;
using MetroFramework.Forms;
using MetroFramework.Controls;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using GestionareSpital;
using MetroFramework;
using System.Collections.Generic;

namespace UI
{
    public partial class FormGestionareUtilizatori : MetroForm
    {
        private AdministrareUser_FisierText adminUtilizatori;
        private AdministrareUser_Memorie adminMemorie;
        private FlowLayoutPanel panelMeniu;

        private User utilizatorCurent;

        public FormGestionareUtilizatori(
            AdministrareUser_FisierText utilizatoriAdmin,
            AdministrareUser_Memorie utilizatoriAdminMemorie,
            User utilizatorCurent)
        {
            this.adminUtilizatori = utilizatoriAdmin;
            this.adminMemorie = utilizatoriAdminMemorie;
            this.utilizatorCurent = utilizatorCurent;

            ConfigureazaMeniu();

            Console.WriteLine("Utilizator curent: " + utilizatorCurent.Nume);
        }

        private void ConfigureazaMeniu()
        {
            this.Text = "Gestionare Utilizatori";
            this.Size = new Size(800, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Style = MetroFramework.MetroColorStyle.Black;

            panelMeniu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
            };

            MetroButton btnAdaugareUtilizator = new MetroButton { Text = "Adaugare utilizator nou", Size = new Size(250, 40) };
            MetroButton btnVizualizareUtilizatori = new MetroButton { Text = "Vizualizare toti utilizatorii", Size = new Size(250, 40) };
            MetroButton btnModificareUtilizator = new MetroButton { Text = "Modificare utilizator", Size = new Size(250, 40) };
            MetroButton btnCautareUtilizator = new MetroButton { Text = "Cautare utilizator", Size = new Size(250, 40) };
            MetroButton btnInchide = new MetroButton { Text = "Inchide", Size = new Size(250, 40) };

            btnAdaugareUtilizator.Click += BtnAdaugareUtilizator_Click;
            btnVizualizareUtilizatori.Click += BtnVizualizareUtilizatori_Click;
            btnModificareUtilizator.Click += BtnModificareUtilizatori_Click;
            btnCautareUtilizator.Click += BtnCautareUtilizator_Click;
            btnInchide.Click += (s, e) => this.Close();

            panelMeniu.Controls.Add(btnAdaugareUtilizator);
            panelMeniu.Controls.Add(btnVizualizareUtilizatori);
            panelMeniu.Controls.Add(btnModificareUtilizator);
            panelMeniu.Controls.Add(btnCautareUtilizator);
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
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugareUtilizatori))
            {
                this.Style = MetroFramework.MetroColorStyle.Black;


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

                using (MetroForm formUtilizator = new MetroForm())
                {
                    formUtilizator.Text = "Adaugare Utilizator Nou";
                    formUtilizator.Size = new Size(800, 400);
                    formUtilizator.StartPosition = FormStartPosition.CenterScreen;
                    formUtilizator.Resizable = false;
                    formUtilizator.Style = MetroFramework.MetroColorStyle.Black;

                    Panel panelFormular = new Panel
                    {
                        Dock = DockStyle.Fill,
                        AutoScroll = true
                    };

                    MetroLabel lblNumeUtilizator = new MetroLabel { Text = "Nume *", Location = new Point(10, 10), Width = 180 };
                    MetroLabel lblPrenumeUtilizator = new MetroLabel { Text = "Prenume *", Location = new Point(10, 50), Width = 180 };
                    MetroLabel lblEmailUtilizator = new MetroLabel { Text = "Email *", Location = new Point(10, 90), Width = 180 };
                    MetroLabel lblTelefonUtilizator = new MetroLabel { Text = "Telefon", Location = new Point(10, 130), Width = 180 };
                    MetroLabel lblParolaUtilizator = new MetroLabel { Text = "Parola *", Location = new Point(10, 170), Width = 180 };
                    MetroLabel lblConfirmareParola = new MetroLabel { Text = "Confirmare Parola *", Location = new Point(10, 210), Width = 180 };
                    MetroLabel lblRangUtilizator = new MetroLabel { Text = "Rang *", Location = new Point(10, 250), Width = 180 };

                    MetroLabel[] eticheteCampuri = new MetroLabel[]
                    {
            lblNumeUtilizator,
            lblPrenumeUtilizator,
            lblEmailUtilizator,
            lblTelefonUtilizator,
            lblParolaUtilizator,
            lblConfirmareParola,
            lblRangUtilizator
                    };

                    TextBox txtNumeUtilizator = new TextBox { Location = new Point(200, 10), Width = 260 };
                    TextBox txtPrenumeUtilizator = new TextBox { Location = new Point(200, 50), Width = 260 };
                    TextBox txtEmailUtilizator = new TextBox { Location = new Point(200, 90), Width = 260 };
                    TextBox txtTelefonUtilizator = new TextBox { Location = new Point(200, 130), Width = 260 };
                    TextBox txtParolaUtilizator = new TextBox { Location = new Point(200, 170), Width = 260, PasswordChar = '●' };
                    TextBox txtConfirmareParola = new TextBox { Location = new Point(200, 210), Width = 260, PasswordChar = '●' };

                    TextBox[] campuriText = new TextBox[]
                    {
            txtNumeUtilizator,
            txtPrenumeUtilizator,
            txtEmailUtilizator,
            txtTelefonUtilizator,
            txtParolaUtilizator,
            txtConfirmareParola
                    };

                    MetroLabel lblMesajEroare = new MetroLabel
                    {
                        Location = new Point(10, 280),
                        Width = 460,
                        ForeColor = Color.Red,
                        Visible = false
                    };
                    panelFormular.Controls.Add(lblMesajEroare);

                    MetroComboBox cboRangUtilizator = new MetroComboBox
                    {
                        Location = new Point(200, 250),
                        Width = 260,
                        DropDownStyle = ComboBoxStyle.DropDownList
                    };

                    foreach (RangUtilizator rang in Enum.GetValues(typeof(RangUtilizator)))
                    {
                        cboRangUtilizator.Items.Add(rang);
                    }
                    cboRangUtilizator.SelectedIndex = 0;

                    txtEmailUtilizator.Leave += (s, ev) =>
                    {
                        if (!string.IsNullOrWhiteSpace(txtEmailUtilizator.Text) &&
                            !Regex.IsMatch(txtEmailUtilizator.Text, ConstanteValidare.REGEX_EMAIL))
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
                            !Regex.IsMatch(txtTelefonUtilizator.Text, ConstanteValidare.REGEX_TELEFON))
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

                    MetroButton btnSalveazaUtilizator = new MetroButton
                    {
                        Text = "Salveaza Utilizatorul",
                        Location = new Point(10, 320),
                        Width = 450
                    };
                    panelFormular.Controls.Add(btnSalveazaUtilizator);


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
                        else if (!Regex.IsMatch(txtEmailUtilizator.Text, ConstanteValidare.REGEX_EMAIL))
                        {
                            lblEmailUtilizator.ForeColor = Color.Red;
                            txtEmailUtilizator.BackColor = Color.LightPink;
                            valid = false;
                            mesajEroare = "Adresa de email nu este valida!";
                        }

                        if (!string.IsNullOrWhiteSpace(txtTelefonUtilizator.Text) &&
                            !Regex.IsMatch(txtTelefonUtilizator.Text, ConstanteValidare.REGEX_TELEFON))
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
                    formUtilizator.Controls.Add(panelFormular);

                    formUtilizator.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a adauga utilizatori!");
            }
        }
       



        private void BtnVizualizareUtilizatori_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareUtilizatori))
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
                        MessageBox.Show("Fisierul este gol!", "Informatie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    using (Form formVizualizare = new Form())
                    {
                        formVizualizare.Text = "Lista Utilizatori";
                        formVizualizare.Size = new Size(750, 450);
                        formVizualizare.StartPosition = FormStartPosition.CenterScreen;

                        ListView listViewUtilizatori = new ListView
                        {
                            Dock = DockStyle.Fill,
                            View = View.Details,
                            FullRowSelect = true,
                            GridLines = true,
                            Font = new Font("Segoe UI", 10),
                            Margin = new Padding(5)
                        };
                        listViewUtilizatori.Font = new Font("Arial", 10, FontStyle.Regular);

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

                        MetroPanel panelContinut = new MetroPanel
                        {
                            Dock = DockStyle.Fill,
                            AutoScroll = true,
                            Padding = new Padding(10)
                        };

                        panelContinut.Controls.Add(listViewUtilizatori);
                        formVizualizare.Controls.Add(panelContinut);

                        formVizualizare.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare: {ex.Message}\n\nDetalii: {ex.StackTrace}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        else
            {
                MessageBox.Show("Nu aveti permisiunea de a vizualiza utilizatorii!");
            }
}


        private void BtnModificareUtilizatori_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.ModificareUtilizatori))
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

                    Console.WriteLine("Continutul fisierului citit:");
                    foreach (string linie in continutFisier)
                    {
                        Console.WriteLine(linie);
                    }

                    if (continutFisier.Length == 0)
                    {
                        MessageBox.Show("Fisierul este gol!", "Informatie");
                        return;
                    }

                    using (MetroForm formModificare = new MetroForm())
                    {
                        formModificare.Text = "Modifica Utilizator";
                        formModificare.Size = new Size(800, 400);
                        formModificare.Style = MetroFramework.MetroColorStyle.Black;

                        ListView listViewUtilizatori = new ListView
                        {
                            Dock = DockStyle.Fill,
                            View = View.Details,
                            FullRowSelect = true,
                            OwnerDraw = true,
                            BackColor = Color.White
                        };

                        listViewUtilizatori.Columns.Add("ID Utilizator", 100);
                        listViewUtilizatori.Columns.Add("Nume", 150);
                        listViewUtilizatori.Columns.Add("Prenume", 150);
                        listViewUtilizatori.Columns.Add("Email", 200);
                        listViewUtilizatori.Columns.Add("Rang", 100);

                        listViewUtilizatori.DrawSubItem += (modSender, modE) =>
                        {
                            bool selected = (modE.ItemState & ListViewItemStates.Selected) != 0;
                            Brush backBrush = selected ? Brushes.LightBlue : Brushes.White;
                            Brush textBrush = Brushes.Black;

                            modE.Graphics.FillRectangle(backBrush, modE.Bounds);
                            TextRenderer.DrawText(modE.Graphics, modE.SubItem.Text, modE.Item.Font, modE.Bounds, Color.Black);
                        };


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

                        listViewUtilizatori.Invalidate();

                        formModificare.Controls.Add(listViewUtilizatori);

                        MetroButton btnModifica = new MetroButton
                        {
                            Text = "Modifica Utilizator",
                            Dock = DockStyle.Bottom,
                            Size = new Size(formModificare.ClientSize.Width, 40)
                        };

                        btnModifica.Click += (s, ev) =>
                        {
                            if (listViewUtilizatori.SelectedItems.Count == 0)
                            {
                                MessageBox.Show("Va rugam selectati un utilizator din lista!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            ListViewItem selectedItem = listViewUtilizatori.SelectedItems[0];
                            string idUtilizator = selectedItem.Text;
                            string nume = selectedItem.SubItems[1].Text;
                            string prenume = selectedItem.SubItems[2].Text;
                            string email = selectedItem.SubItems[3].Text;
                            string rang = selectedItem.SubItems[4].Text;

                            using (MetroForm formModificareDetalii = new MetroForm())
                            {
                                formModificareDetalii.Size = new Size(800, 400);
                                formModificareDetalii.StartPosition = FormStartPosition.CenterScreen;
                                formModificareDetalii.Style = MetroFramework.MetroColorStyle.Black;

                                MetroLabel lblNume = new MetroLabel { Text = "Nume:", Location = new Point(10, 10) };
                                MetroTextBox txtNume = new MetroTextBox { Location = new Point(170, 10), Text = nume, Width = 200 };

                                MetroLabel lblPrenume = new MetroLabel { Text = "Prenume:", Location = new Point(10, 40) };
                                MetroTextBox txtPrenume = new MetroTextBox { Location = new Point(170, 40), Text = prenume, Width = 200 };

                                MetroLabel lblEmail = new MetroLabel { Text = "Email:", Location = new Point(10, 70) };
                                MetroTextBox txtEmail = new MetroTextBox { Location = new Point(170, 70), Text = email, Width = 200 };

                                MetroLabel lblRang = new MetroLabel { Text = "Rang:", Location = new Point(10, 100) };
                                MetroComboBox cboRang = new MetroComboBox
                                {
                                    Location = new Point(170, 100),
                                    Width = 200,
                                    DropDownStyle = ComboBoxStyle.DropDownList
                                };

                                foreach (RangUtilizator rangEnum in Enum.GetValues(typeof(RangUtilizator)))
                                {
                                    cboRang.Items.Add(rangEnum);
                                }
                                cboRang.SelectedItem = Enum.Parse(typeof(RangUtilizator), rang);

                                MetroButton btnSalveaza = new MetroButton
                                {
                                    Text = "Salveaza Modificari",
                                    Location = new Point(10, 150),
                                    Width = 360
                                };

                                btnSalveaza.Click += (modS, modEv) =>
                                {
                                    if (string.IsNullOrWhiteSpace(txtNume.Text) || string.IsNullOrWhiteSpace(txtPrenume.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
                                    {
                                        MessageBox.Show("Toate campurile trebuie completate!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    string newNume = txtNume.Text;
                                    string newPrenume = txtPrenume.Text;
                                    string newEmail = txtEmail.Text;
                                    RangUtilizator newRang = (RangUtilizator)cboRang.SelectedItem;


                                    var updatedContent = continutFisier.ToList();
                                    var userLine = updatedContent.FirstOrDefault(l => l.StartsWith(idUtilizator));
                                    if (userLine != null)
                                    {
                                        string[] detaliiVechi = userLine.Split(',');
                                        string parolaVeche = detaliiVechi.Length > 4 ? detaliiVechi[4] : "";

                                        string modifiedUser = $"{idUtilizator},{newNume},{newPrenume},{newEmail},{parolaVeche},{newRang}";

                                        int index = updatedContent.IndexOf(userLine);
                                        updatedContent[index] = modifiedUser;
                                    }


                                    File.WriteAllLines(caleCompletaFisier, updatedContent);

                                    MessageBox.Show("Modificarile au fost salvate cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    formModificareDetalii.DialogResult = DialogResult.OK;
                                    formModificareDetalii.Close();
                                };

                                formModificareDetalii.Controls.Add(lblNume);
                                formModificareDetalii.Controls.Add(txtNume);
                                formModificareDetalii.Controls.Add(lblPrenume);
                                formModificareDetalii.Controls.Add(txtPrenume);
                                formModificareDetalii.Controls.Add(lblEmail);
                                formModificareDetalii.Controls.Add(txtEmail);
                                formModificareDetalii.Controls.Add(lblRang);
                                formModificareDetalii.Controls.Add(cboRang);
                                formModificareDetalii.Controls.Add(btnSalveaza);

                                formModificareDetalii.ShowDialog();
                            }
                        };

                        formModificare.Controls.Add(btnModifica);
                        formModificare.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare: {ex.Message}\n\nDetalii: {ex.StackTrace}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a modifica utilizatorii!");
            }

        }

        private void BtnCautareUtilizator_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareUtilizatori))
            {
                using (MetroForm formCautare = new MetroForm())
                {
                    formCautare.Text = "Cautare Utilizatori";
                    formCautare.Size = new Size(600, 500);
                    formCautare.StartPosition = FormStartPosition.CenterScreen;
                    formCautare.Style = MetroFramework.MetroColorStyle.Black;

                    MetroPanel panelOptiuni = new MetroPanel
                    {
                        Dock = DockStyle.Top,
                        Height = 200,
                        AutoScroll = true,
                        Padding = new Padding(10)
                    };

                    MetroLabel lblTitlu = new MetroLabel
                    {
                        Text = "Selectati criteriul de cautare:",
                        FontWeight = MetroLabelWeight.Bold,
                        Location = new Point(10, 10),
                        Size = new Size(400, 30)
                    };
                    panelOptiuni.Controls.Add(lblTitlu);

                    GroupBox groupTipCautare = new GroupBox
                    {
                        Text = "Tip de cautare",
                        Location = new Point(10, 40),
                        Size = new Size(250, 80)
                    };

                    RadioButton radioExact = new RadioButton
                    {
                        Text = "Potrivire exacta",
                        Location = new Point(10, 20),
                        Checked = true
                    };
                    RadioButton radioPartial = new RadioButton
                    {
                        Text = "Potrivire partiala",
                        Location = new Point(10, 45)
                    };

                    groupTipCautare.Controls.Add(radioExact);
                    groupTipCautare.Controls.Add(radioPartial);
                    panelOptiuni.Controls.Add(groupTipCautare);

                    GroupBox groupCampuri = new GroupBox
                    {
                        Text = "Criterii de cautare",
                        Location = new Point(270, 40),
                        Size = new Size(300, 150)
                    };

                    CheckBox checkId = new CheckBox { Text = "ID Utilizator", Location = new Point(10, 20), Checked = true };
                    CheckBox checkNume = new CheckBox { Text = "Nume", Location = new Point(10, 45) };
                    CheckBox checkPrenume = new CheckBox { Text = "Prenume", Location = new Point(10, 70) };
                    CheckBox checkEmail = new CheckBox { Text = "Email", Location = new Point(150, 20) };
                    CheckBox checkTelefon = new CheckBox { Text = "Telefon", Location = new Point(150, 45) };
                    CheckBox checkRang = new CheckBox { Text = "Rang", Location = new Point(150, 70) };

                    groupCampuri.Controls.Add(checkId);
                    groupCampuri.Controls.Add(checkNume);
                    groupCampuri.Controls.Add(checkPrenume);
                    groupCampuri.Controls.Add(checkEmail);
                    groupCampuri.Controls.Add(checkTelefon);
                    groupCampuri.Controls.Add(checkRang);
                    panelOptiuni.Controls.Add(groupCampuri);
                    List<CheckBox> listaCheckBoxuri = new List<CheckBox> { checkId, checkNume, checkPrenume, checkEmail, checkTelefon, checkRang };
                    void CheckBox_CheckedChanged(object s, EventArgs ev)
                    {
                        if (s is CheckBox current)
                        {
                            if (current.Checked)
                            {
                                foreach (var cb in listaCheckBoxuri)
                                {
                                    if (cb != current)
                                        cb.Checked = false;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Evenimentul nu a fost declansat de un CheckBox.", "Eroare");
                        }
                    }


                    foreach (var cb in listaCheckBoxuri)
                    {
                        cb.CheckedChanged += CheckBox_CheckedChanged;
                    }


                    MetroLabel lblTermenCautare = new MetroLabel
                    {
                        Text = "Termen de cautare:",
                        Location = new Point(10, 250),
                        Size = new Size(150, 30)
                    };
                    formCautare.Controls.Add(lblTermenCautare);

                    MetroTextBox txtTermenCautare = new MetroTextBox
                    {
                        Location = new Point(160, 250),
                        Size = new Size(250, 30),
                    };
                    formCautare.Controls.Add(txtTermenCautare);

                    MetroLabel lblRezultate = new MetroLabel
                    {
                        Text = "Rezultatele cautarii:",
                        Location = new Point(10, 290),
                        Size = new Size(150, 30)
                    };
                    formCautare.Controls.Add(lblRezultate);

                    ListView listViewRezultate = new ListView
                    {
                        Location = new Point(10, 320),
                        Size = new Size(570, 130),
                        View = View.Details,
                        FullRowSelect = true,
                        GridLines = true
                    };

                    listViewRezultate.Columns.Add("ID", 50);
                    listViewRezultate.Columns.Add("Nume", 120);
                    listViewRezultate.Columns.Add("Prenume", 120);
                    listViewRezultate.Columns.Add("Email", 180);
                    listViewRezultate.Columns.Add("Telefon", 100);
                    listViewRezultate.Columns.Add("Rang", 100);
                    formCautare.Controls.Add(listViewRezultate);

                    MetroButton btnCauta = new MetroButton
                    {
                        Text = "Cauta",
                        Location = new Point(420, 250),
                        Size = new Size(100, 30)
                    };
                    formCautare.Controls.Add(btnCauta);

                    btnCauta.Click += (s, ev) =>
                    {
                        if (string.IsNullOrWhiteSpace(txtTermenCautare.Text))
                        {
                            MessageBox.Show("Introduceti un termen de cautare!", "Atentie",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (!checkId.Checked && !checkNume.Checked && !checkPrenume.Checked &&
                            !checkEmail.Checked && !checkTelefon.Checked && !checkRang.Checked)
                        {
                            MessageBox.Show("Selectati cel putin un criteriu de cautare!", "Atentie",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        CautaUtilizatori(
                            listViewRezultate,
                            txtTermenCautare.Text,
                            radioPartial.Checked,
                            checkId.Checked,
                            checkNume.Checked,
                            checkPrenume.Checked,
                            checkEmail.Checked,
                            checkTelefon.Checked,
                            checkRang.Checked
                        );
                    };

                    listViewRezultate.DoubleClick += (s, ev) =>
                    {
                        if (listViewRezultate.SelectedItems.Count > 0)
                        {
                            int idUtilizator = int.Parse(listViewRezultate.SelectedItems[0].Text);
                            AfiseazaDetaliiUtilizator(idUtilizator);
                        }
                    };

                    formCautare.Controls.Add(panelOptiuni);
                    formCautare.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a cauta utilizatori.",
                    "Acces restrictionat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CautaUtilizatori(
     ListView listViewRezultate,
     string termenCautare,
     bool cautarePartiala,
     bool cautaDupaId,
     bool cautaDupaNume,
     bool cautaDupaPrenume,
     bool cautaDupaEmail,
     bool cautaDupaTelefon,
     bool cautaDupaRang)
        {
            listViewRezultate.Items.Clear();

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

                string[] linii = File.ReadAllLines(caleCompletaFisier);
                if (linii.Length == 0)
                {
                    MessageBox.Show("Fisierul este gol!", "Informatie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string termLower = termenCautare.ToLower();
                bool gasite = false;

                foreach (string linie in linii)
                {
                    string[] detaliiUtilizator = linie.Split(',');
                    if (detaliiUtilizator.Length < 6) continue;

                    bool potrivire = false;

                    if (cautaDupaId && detaliiUtilizator[0].Contains(termenCautare))
                    {
                        potrivire = true;
                    }

                    if (cautaDupaNume)
                    {
                        string numeLower = detaliiUtilizator[1].ToLower();
                        if ((cautarePartiala && numeLower.Contains(termLower)) ||
                            (!cautarePartiala && numeLower.Equals(termLower)))
                        {
                            potrivire = true;
                        }
                    }

                    if (cautaDupaPrenume)
                    {
                        string prenumeLower = detaliiUtilizator[2].ToLower();
                        if ((cautarePartiala && prenumeLower.Contains(termLower)) ||
                            (!cautarePartiala && prenumeLower.Equals(termLower)))
                        {
                            potrivire = true;
                        }
                    }

                    if (cautaDupaEmail)
                    {
                        string emailLower = detaliiUtilizator[3].ToLower();
                        if ((cautarePartiala && emailLower.Contains(termLower)) ||
                            (!cautarePartiala && emailLower.Equals(termLower)))
                        {
                            potrivire = true;
                        }
                    }

                    if (cautaDupaTelefon && detaliiUtilizator.Length > 4)
                    {
                        string telefonLower = detaliiUtilizator[4].ToLower();
                        if ((cautarePartiala && telefonLower.Contains(termLower)) ||
                            (!cautarePartiala && telefonLower.Equals(termLower)))
                        {
                            potrivire = true;
                        }
                    }

                    if (cautaDupaRang)
                    {
                        string rangLower = detaliiUtilizator[5].ToLower();
                        if ((cautarePartiala && rangLower.Contains(termLower)) ||
                            (!cautarePartiala && rangLower.Equals(termLower)))
                        {
                            potrivire = true;
                        }
                    }

                    if (potrivire)
                    {
                        ListViewItem item = new ListViewItem(detaliiUtilizator[0]);
                        item.SubItems.Add(detaliiUtilizator[1]);
                        item.SubItems.Add(detaliiUtilizator[2]);
                        item.SubItems.Add(detaliiUtilizator[3]);
                        item.SubItems.Add(detaliiUtilizator.Length > 4 ? detaliiUtilizator[4] : "N/A");
                        item.SubItems.Add(detaliiUtilizator[5]);

                        listViewRezultate.Items.Add(item);
                        gasite = true;
                    }
                }

                if (!gasite)
                {
                    MessageBox.Show("Nu s-au gasit utilizatori care sa corespunda criteriilor selectate.",
                        "Rezultat cautare", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare in timpul cautarii: {ex.Message}", "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AfiseazaDetaliiUtilizator(int idUtilizator)
        {
            User utilizator = GetUtilizatorDupaId(idUtilizator);

            if (utilizator == null)
            {
                MessageBox.Show("Nu s-a gasit utilizatorul selectat!", "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MetroForm formDetalii = new MetroForm())
            {
                formDetalii.Text = $"Detalii Utilizator #{utilizator.IdUser}";
                formDetalii.Size = new Size(500, 400);
                formDetalii.StartPosition = FormStartPosition.CenterScreen;
                formDetalii.Style = MetroFramework.MetroColorStyle.Black;
                formDetalii.Padding = new Padding(20);

                TableLayoutPanel tableLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 6,
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
                };

                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

                AddRowToTable(tableLayout, 0, "ID Utilizator:", utilizator.IdUser.ToString());
                AddRowToTable(tableLayout, 1, "Nume:", utilizator.Nume);
                AddRowToTable(tableLayout, 2, "Prenume:", utilizator.Prenume);
                AddRowToTable(tableLayout, 3, "Email:", utilizator.Email);
                AddRowToTable(tableLayout, 4, "Telefon:", utilizator.Telefon ?? "N/A");
                AddRowToTable(tableLayout, 5, "Rang:", utilizator.Rang.ToString());

                formDetalii.Controls.Add(tableLayout);
                formDetalii.ShowDialog();
            }
        }
        private void AddRowToTable(TableLayoutPanel table, int rowIndex, string label, string value)
        {
            Label lblLabel = new Label
            {
                Text = label,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Padding = new Padding(5)
            };

            Label lblValue = new Label
            {
                Text = value,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                AutoSize = false
            };

            table.Controls.Add(lblLabel, 0, rowIndex);
            table.Controls.Add(lblValue, 1, rowIndex);
        }

        private User GetUtilizatorDupaId(int idUtilizator)
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
                    return null;
                }

                string[] linii = File.ReadAllLines(caleCompletaFisier);

                foreach (string linie in linii)
                {
                    string[] detaliiUtilizator = linie.Split(',');
                    if (detaliiUtilizator.Length < 6) continue;

                    if (int.TryParse(detaliiUtilizator[0], out int id) && id == idUtilizator)
                    {
                        User utilizator = new User
                        {
                            IdUser = id,
                            Nume = detaliiUtilizator[1],
                            Prenume = detaliiUtilizator[2],
                            Email = detaliiUtilizator[3],
                            Telefon = detaliiUtilizator.Length > 4 ? detaliiUtilizator[4] : null,
                            Rang = (RangUtilizator)Enum.Parse(typeof(RangUtilizator), detaliiUtilizator[5])
                        };
                        return utilizator;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la cautarea utilizatorului: {ex.Message}", "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }




    }
}