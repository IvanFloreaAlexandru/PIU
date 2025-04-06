using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LibrarieModele;
using NivelStocareDate;

namespace UI
{
    public partial class FormGestionareDepartamente : Form
    {
        private AdministrareDepartamente_FisierText adminDepartamente;

        public FormGestionareDepartamente(AdministrareDepartamente_FisierText departamenteAdmin)
        {
            adminDepartamente = departamenteAdmin;

            ConfigureazaMeniu();
        }

        private void ConfigureazaMeniu()
        {
            this.Text = "Gestionare Departamente";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            FlowLayoutPanel panelMeniu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
            };

            Button btnAdaugareDepartament = new Button { Text = "Adaugare departament nou", Size = new Size(250, 40) };
            Button btnVizualizareDepartamente = new Button { Text = "Vizualizare toate departamentele", Size = new Size(250, 40) };
            Button btnModificareDepartament = new Button { Text = "Modificare departament", Size = new Size(250, 40) };
            Button btnInchide = new Button { Text = "Inchide", Size = new Size(250, 40) };

            btnAdaugareDepartament.Click += BtnAdaugareDepartament_Click;
            btnVizualizareDepartamente.Click += BtnVizualizareDepartamente_Click;
            btnModificareDepartament.Click += BtnModificareDepartament_Click;
            btnInchide.Click += (s, e) => this.Close();

            panelMeniu.Controls.Add(btnAdaugareDepartament);
            panelMeniu.Controls.Add(btnVizualizareDepartamente);
            panelMeniu.Controls.Add(btnModificareDepartament);
            panelMeniu.Controls.Add(btnInchide);

            this.Controls.Add(panelMeniu);
        }

        public static class ConstanteValidare
        {
            public const int MAXIM_CARACTERE_NUME_DEPARTAMENT = 50;
            public const int MINIM_CARACTERE_NUME_DEPARTAMENT = 3;
            public const int MAXIM_CARACTERE_DESCRIERE_DEPARTAMENT = 500;
            public const int MAXIM_CARACTERE_LOCATIE_DEPARTAMENT = 100;
            public const int MINIM_CARACTERE_LOCATIE_DEPARTAMENT = 5;
            public const string PATTERN_LOCATIE_DEPARTAMENT = @"^[A-Za-z0-9\s\.,\-]{5,100}$";

        }

            private void BtnAdaugareDepartament_Click(object sender, EventArgs e)
        {
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisierDepartamente = Path.Combine(locatieFisierSolutie, "Departamente.txt");

            using (Form formDepartament = new Form())
            {
                formDepartament.Text = "Departament Nou";
                formDepartament.Size = new Size(500, 450);
                formDepartament.StartPosition = FormStartPosition.CenterScreen;
                formDepartament.FormBorderStyle = FormBorderStyle.FixedDialog;
                formDepartament.MaximizeBox = false;
                formDepartament.MinimizeBox = false;

                Panel panelFormular = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    Name = "panelFormular"
                };

                Label lblTitluFormular = new Label
                {
                    Text = "Adaugare Departament Nou",
                    Location = new Point(10, 10),
                    Width = 460,
                    Font = new Font(SystemFonts.DefaultFont.FontFamily, 10, FontStyle.Bold),
                    Name = "lblTitluFormular"
                };

                Label lblNumeDepartament = new Label
                {
                    Text = "Nume Departament *:",
                    Location = new Point(10, 50),
                    Width = 150,
                    Name = "lblNumeDepartament"
                };

                Label lblDescriereDepartament = new Label
                {
                    Text = "Descriere Departament:",
                    Location = new Point(10, 80),
                    Width = 150,
                    Name = "lblDescriereDepartament"
                };

                Label lblLocatieDepartament = new Label
                {
                    Text = "Locatie Departament *:",
                    Location = new Point(10, 150),
                    Width = 150,
                    Name = "lblLocatieDepartament"
                };

                Label lblCapacitateDepartament = new Label
                {
                    Text = "Capacitate Departament:",
                    Location = new Point(10, 180),
                    Width = 150,
                    Name = "lblCapacitateDepartament"
                };

                Label[] eticheteCampuri = new Label[] {
            lblNumeDepartament,
            lblDescriereDepartament,
            lblLocatieDepartament,
            lblCapacitateDepartament
        };

                TextBox txtNumeDepartament = new TextBox
                {
                    Location = new Point(170, 50),
                    Width = 300,
                    Name = "txtNumeDepartament"
                };

                TextBox txtDescriereDepartament = new TextBox
                {
                    Location = new Point(170, 80),
                    Width = 300,
                    Height = 60,
                    Multiline = true,
                    Name = "txtDescriereDepartament"
                };

                TextBox txtLocatieDepartament = new TextBox
                {
                    Location = new Point(170, 150),
                    Width = 300,
                    Name = "txtLocatieDepartament"
                };



                Label lblMesajEroare = new Label
                {
                    Location = new Point(10, 210),
                    Width = 460,
                    Height = 40,
                    ForeColor = Color.Red,
                    Visible = false,
                    Name = "lblMesajEroare"
                };

                txtNumeDepartament.Leave += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNumeDepartament.Text))
                    {
                        lblNumeDepartament.ForeColor = Color.Red;
                        txtNumeDepartament.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "Numele departamentului este obligatoriu!";
                        lblMesajEroare.Visible = true;
                    }
                    else if (txtNumeDepartament.Text.Length < ConstanteValidare.MINIM_CARACTERE_NUME_DEPARTAMENT)
                    {
                        lblNumeDepartament.ForeColor = Color.Red;
                        txtNumeDepartament.BackColor = Color.LightPink;
                        lblMesajEroare.Text = $"Numele departamentului trebuie sa contina minim {ConstanteValidare.MINIM_CARACTERE_NUME_DEPARTAMENT} caractere!";
                        lblMesajEroare.Visible = true;
                    }
                    else if (txtNumeDepartament.Text.Length > ConstanteValidare.MAXIM_CARACTERE_NUME_DEPARTAMENT)
                    {
                        lblNumeDepartament.ForeColor = Color.Red;
                        txtNumeDepartament.BackColor = Color.LightPink;
                        lblMesajEroare.Text = $"Numele departamentului nu poate depasi {ConstanteValidare.MAXIM_CARACTERE_NUME_DEPARTAMENT} caractere!";
                        lblMesajEroare.Visible = true;
                    }
                    else
                    {
                        lblNumeDepartament.ForeColor = SystemColors.ControlText;
                        txtNumeDepartament.BackColor = SystemColors.Window;
                        lblMesajEroare.Visible = false;
                    }
                };

                txtDescriereDepartament.Leave += (s, ev) =>
                {
                    if (!string.IsNullOrWhiteSpace(txtDescriereDepartament.Text) &&
                        txtDescriereDepartament.Text.Length > ConstanteValidare.MAXIM_CARACTERE_DESCRIERE_DEPARTAMENT)
                    {
                        lblDescriereDepartament.ForeColor = Color.Red;
                        txtDescriereDepartament.BackColor = Color.LightPink;
                        lblMesajEroare.Text = $"Descrierea departamentului nu poate depasi {ConstanteValidare.MAXIM_CARACTERE_DESCRIERE_DEPARTAMENT} caractere!";
                        lblMesajEroare.Visible = true;
                    }
                    else
                    {
                        lblDescriereDepartament.ForeColor = SystemColors.ControlText;
                        txtDescriereDepartament.BackColor = SystemColors.Window;
                        lblMesajEroare.Visible = false;
                    }
                };

                txtLocatieDepartament.Leave += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtLocatieDepartament.Text))
                    {
                        lblLocatieDepartament.ForeColor = Color.Red;
                        txtLocatieDepartament.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "Locatia departamentului este obligatorie!";
                        lblMesajEroare.Visible = true;
                    }
                    else if (txtLocatieDepartament.Text.Length < ConstanteValidare.MINIM_CARACTERE_LOCATIE_DEPARTAMENT)
                    {
                        lblLocatieDepartament.ForeColor = Color.Red;
                        txtLocatieDepartament.BackColor = Color.LightPink;
                        lblMesajEroare.Text = $"Locatia departamentului trebuie sa contina minim {ConstanteValidare.MINIM_CARACTERE_LOCATIE_DEPARTAMENT} caractere!";
                        lblMesajEroare.Visible = true;
                    }
                    else if (txtLocatieDepartament.Text.Length > ConstanteValidare.MAXIM_CARACTERE_LOCATIE_DEPARTAMENT)
                    {
                        lblLocatieDepartament.ForeColor = Color.Red;
                        txtLocatieDepartament.BackColor = Color.LightPink;
                        lblMesajEroare.Text = $"Locatia departamentului nu poate depasi {ConstanteValidare.MAXIM_CARACTERE_LOCATIE_DEPARTAMENT} caractere!";
                        lblMesajEroare.Visible = true;
                    }
                    else if (!Regex.IsMatch(txtLocatieDepartament.Text, ConstanteValidare.PATTERN_LOCATIE_DEPARTAMENT))
                    {
                        lblLocatieDepartament.ForeColor = Color.Red;
                        txtLocatieDepartament.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "Formatul locatiei este invalid! Exemplu corect: Cladire A, Etaj 2";
                        lblMesajEroare.Visible = true;
                    }
                    else
                    {
                        lblLocatieDepartament.ForeColor = SystemColors.ControlText;
                        txtLocatieDepartament.BackColor = SystemColors.Window;
                        lblMesajEroare.Visible = false;
                    }
                };

               

                Button btnSalveazaDepartament = new Button
                {
                    Text = "Salveaza Departamentul",
                    Location = new Point(10, 260),
                    Width = 460,
                    Height = 40,
                    Name = "btnSalveazaDepartament"
                };

                btnSalveazaDepartament.Click += (s, ev) =>
                {
                    foreach (Label lbl in eticheteCampuri)
                    {
                        lbl.ForeColor = SystemColors.ControlText;
                    }
                    txtNumeDepartament.BackColor = SystemColors.Window;
                    txtDescriereDepartament.BackColor = SystemColors.Window;
                    txtLocatieDepartament.BackColor = SystemColors.Window;
                    lblMesajEroare.Visible = false;

                    bool valid = true;
                    string mesajEroare = "";

                    if (string.IsNullOrWhiteSpace(txtNumeDepartament.Text))
                    {
                        lblNumeDepartament.ForeColor = Color.Red;
                        txtNumeDepartament.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Numele departamentului este obligatoriu!";
                    }
                    else if (txtNumeDepartament.Text.Length < ConstanteValidare.MINIM_CARACTERE_NUME_DEPARTAMENT)
                    {
                        lblNumeDepartament.ForeColor = Color.Red;
                        txtNumeDepartament.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = $"Numele departamentului trebuie sa contina minim {ConstanteValidare.MINIM_CARACTERE_NUME_DEPARTAMENT} caractere!";
                    }
                    else if (txtNumeDepartament.Text.Length > ConstanteValidare.MAXIM_CARACTERE_NUME_DEPARTAMENT)
                    {
                        lblNumeDepartament.ForeColor = Color.Red;
                        txtNumeDepartament.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = $"Numele departamentului nu poate depasi {ConstanteValidare.MAXIM_CARACTERE_NUME_DEPARTAMENT} caractere!";
                    }

                    if (!string.IsNullOrWhiteSpace(txtDescriereDepartament.Text) &&
                        txtDescriereDepartament.Text.Length > ConstanteValidare.MAXIM_CARACTERE_DESCRIERE_DEPARTAMENT)
                    {
                        lblDescriereDepartament.ForeColor = Color.Red;
                        txtDescriereDepartament.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = $"Descrierea departamentului nu poate depasi {ConstanteValidare.MAXIM_CARACTERE_DESCRIERE_DEPARTAMENT} caractere!";
                    }

                    if (string.IsNullOrWhiteSpace(txtLocatieDepartament.Text))
                    {
                        lblLocatieDepartament.ForeColor = Color.Red;
                        txtLocatieDepartament.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Locatia departamentului este obligatorie!";
                    }
                    else if (txtLocatieDepartament.Text.Length < ConstanteValidare.MINIM_CARACTERE_LOCATIE_DEPARTAMENT)
                    {
                        lblLocatieDepartament.ForeColor = Color.Red;
                        txtLocatieDepartament.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = $"Locatia departamentului trebuie sa contina minim {ConstanteValidare.MINIM_CARACTERE_LOCATIE_DEPARTAMENT} caractere!";
                    }
                    else if (txtLocatieDepartament.Text.Length > ConstanteValidare.MAXIM_CARACTERE_LOCATIE_DEPARTAMENT)
                    {
                        lblLocatieDepartament.ForeColor = Color.Red;
                        txtLocatieDepartament.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = $"Locatia departamentului nu poate depasi {ConstanteValidare.MAXIM_CARACTERE_LOCATIE_DEPARTAMENT} caractere!";
                    }
                    else if (!Regex.IsMatch(txtLocatieDepartament.Text, ConstanteValidare.PATTERN_LOCATIE_DEPARTAMENT))
                    {
                        lblLocatieDepartament.ForeColor = Color.Red;
                        txtLocatieDepartament.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Formatul locatiei este invalid! Exemplu corect: Cladire A, Etaj 2";
                    }

                   

                    if (!valid)
                    {
                        lblMesajEroare.Text = mesajEroare;
                        lblMesajEroare.Visible = true;
                        return;
                    }

                    try
                    {
                        Departament[] departamenteExistente = adminDepartamente.GetDepartamente(out int nrDepartamente);
                        int nuoulId = nrDepartamente > 0 ? departamenteExistente[nrDepartamente - 1].IdDepartament + 1 : 1;

                        Departament departamentNou = new Departament
                        {
                            IdDepartament = nuoulId,
                            Nume = txtNumeDepartament.Text.Trim(),
                            Descriere = txtDescriereDepartament.Text.Trim(),
                            Locatie = txtLocatieDepartament.Text.Trim(),
                        };

                        adminDepartamente.AddDepartament(departamentNou);
                        MessageBox.Show($"Departamentul '{departamentNou.Nume}' a fost adaugat cu succes!",
                            "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formDepartament.DialogResult = DialogResult.OK;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Eroare la adaugarea departamentului: {ex.Message}",
                            "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                panelFormular.Controls.Add(lblTitluFormular);
                foreach (Label lbl in eticheteCampuri)
                {
                    panelFormular.Controls.Add(lbl);
                }
                panelFormular.Controls.Add(txtNumeDepartament);
                panelFormular.Controls.Add(txtDescriereDepartament);
                panelFormular.Controls.Add(txtLocatieDepartament);
                panelFormular.Controls.Add(lblMesajEroare);
                panelFormular.Controls.Add(btnSalveazaDepartament);

                formDepartament.Controls.Add(panelFormular);
                formDepartament.ShowDialog();
            }
        }

        private void BtnVizualizareDepartamente_Click(object sender, EventArgs e)
        {
            Departament[] departamente = adminDepartamente.GetDepartamente(out int nrDepartamente);

            using (Form formVizualizare = new Form())
            {
                formVizualizare.Text = "Toate Departamentele";
                formVizualizare.Size = new Size(600, 400);
                formVizualizare.StartPosition = FormStartPosition.CenterScreen;

                ListView listView = new ListView
                {
                    Dock = DockStyle.Fill,
                    View = View.Details
                };

                listView.Columns.Add("ID", 50);
                listView.Columns.Add("Nume", 150);
                listView.Columns.Add("Descriere", 200);
                listView.Columns.Add("Locatie", 150);

                foreach (var departament in departamente)
                {
                    ListViewItem item = new ListViewItem(departament.IdDepartament.ToString());
                    item.SubItems.Add(departament.Nume);
                    item.SubItems.Add(departament.Descriere);
                    item.SubItems.Add(departament.Locatie);

                    listView.Items.Add(item);
                }

                formVizualizare.Controls.Add(listView);
                formVizualizare.ShowDialog();
            }
        }

        private void BtnModificareDepartament_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");

        }
    }
}