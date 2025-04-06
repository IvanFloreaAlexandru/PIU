using System;
using System.Windows.Forms;
using System.Drawing;
using GestionareSpital;
using LibrarieModele;
using NivelStocareDate;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace UI
{
    public partial class FormGestionarePacienti : Form
    {
        private Button btnAdaugarePacient;
        private Button btnModificarePacient;
        private Button btnStergerePacient;
        private Button btnVizualizarePacienti;
        private Button btnInapoi;

        private AdministrarePacienti_FisierText adminPacienti;
        private User utilizatorCurent;

        public FormGestionarePacienti(User utilizator, Size size)
        {
            InitializeComponent();
            utilizatorCurent = utilizator;
            adminPacienti = new AdministrarePacienti_FisierText("Pacienti.txt");

            this.Size = size;
            this.MinimumSize = new Size(1080, 400); 
            this.StartPosition = FormStartPosition.CenterScreen;

            btnAdaugarePacient = new Button { Text = "Adaugare pacient" };
            btnModificarePacient = new Button { Text = "Modificare pacient" };
            btnStergerePacient = new Button { Text = "Stergere pacient" };
            btnVizualizarePacienti = new Button { Text = "Vizualizare pacienti" };
            btnInapoi = new Button { Text = "Inapoi" };

            btnAdaugarePacient.Size = new Size(200, 40);
            btnModificarePacient.Size = new Size(200, 40);
            btnStergerePacient.Size = new Size(200, 40);
            btnVizualizarePacienti.Size = new Size(200, 40);
            btnInapoi.Size = new Size(200, 40);

            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(10)
            };

            panel.Controls.Add(btnAdaugarePacient);
            panel.Controls.Add(btnModificarePacient);
            panel.Controls.Add(btnStergerePacient);
            panel.Controls.Add(btnVizualizarePacienti);
            panel.Controls.Add(btnInapoi);
            this.Controls.Add(panel);

            btnAdaugarePacient.Click += BtnAdaugarePacient_Click;
            btnModificarePacient.Click += BtnModificarePacient_Click;
            btnStergerePacient.Click += BtnStergerePacient_Click;
            btnVizualizarePacienti.Click += BtnVizualizarePacienti_Click;
            btnInapoi.Click += BtnInapoi_Click;
        }

        private void BtnAdaugarePacient_Click(object sender, EventArgs e)
        {
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string numeFisierPacienti = "Pacienti.txt";
            string caleCompletaFisierPacienti = Path.Combine(locatieFisierSolutie, numeFisierPacienti);

            using (Form formAdaugarePacient = new Form())
            {
                formAdaugarePacient.Text = "Adaugare Pacient Nou";
                formAdaugarePacient.Size = new Size(500, 600);
                formAdaugarePacient.StartPosition = FormStartPosition.CenterScreen;
                formAdaugarePacient.FormBorderStyle = FormBorderStyle.FixedDialog;
                formAdaugarePacient.MaximizeBox = false;
                formAdaugarePacient.MinimizeBox = false;

                Panel panelFormular = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    Name = "panelFormular"
                };

                Label lblTitluFormular = new Label
                {
                    Text = "Adaugare Pacient Nou",
                    Location = new Point(10, 10),
                    Width = 460,
                    Font = new Font(SystemFonts.DefaultFont.FontFamily, 10, FontStyle.Bold),
                    Name = "lblTitluFormular"
                };

                Label lblNume = new Label
                {
                    Text = "Nume *:",
                    Location = new Point(10, 50),
                    Width = 150,
                    Name = "lblNume"
                };

                Label lblPrenume = new Label
                {
                    Text = "Prenume *:",
                    Location = new Point(10, 80),
                    Width = 150,
                    Name = "lblPrenume"
                };

                Label lblCNP = new Label
                {
                    Text = "CNP *:",
                    Location = new Point(10, 110),
                    Width = 150,
                    Name = "lblCNP"
                };

                Label lblDataNasterii = new Label
                {
                    Text = "Data nasterii *:",
                    Location = new Point(10, 140),
                    Width = 150,
                    Name = "lblDataNasterii"
                };

                Label lblGen = new Label
                {
                    Text = "Gen *:",
                    Location = new Point(10, 170),
                    Width = 150,
                    Name = "lblGen"
                };

                Label lblAdresa = new Label
                {
                    Text = "Adresa *:",
                    Location = new Point(10, 200),
                    Width = 150,
                    Name = "lblAdresa"
                };

                Label lblTelefon = new Label
                {
                    Text = "Telefon *:",
                    Location = new Point(10, 270),
                    Width = 150,
                    Name = "lblTelefon"
                };

                Label lblEmail = new Label
                {
                    Text = "Email:",
                    Location = new Point(10, 300),
                    Width = 150,
                    Name = "lblEmail"
                };

                Label lblGrupaSanguina = new Label
                {
                    Text = "Grupa sanguina:",
                    Location = new Point(10, 330),
                    Width = 150,
                    Name = "lblGrupaSanguina"
                };

                Label lblAlergii = new Label
                {
                    Text = "Alergii:",
                    Location = new Point(10, 360),
                    Width = 150,
                    Name = "lblAlergii"
                };

                Label[] eticheteCampuri = new Label[]
                {
            lblNume,
            lblPrenume,
            lblCNP,
            lblDataNasterii,
            lblGen,
            lblAdresa,
            lblTelefon,
            lblEmail,
            lblGrupaSanguina,
            lblAlergii
                };

                TextBox txtNume = new TextBox
                {
                    Location = new Point(170, 50),
                    Width = 300,
                    Name = "txtNume"
                };

                TextBox txtPrenume = new TextBox
                {
                    Location = new Point(170, 80),
                    Width = 300,
                    Name = "txtPrenume"
                };

                TextBox txtCNP = new TextBox
                {
                    Location = new Point(170, 110),
                    Width = 300,
                    Name = "txtCNP",
                    MaxLength = 13
                };

                DateTimePicker dtpDataNasterii = new DateTimePicker
                {
                    Format = DateTimePickerFormat.Short,
                    Location = new Point(170, 140),
                    Width = 300,
                    Name = "dtpDataNasterii"
                };

                ComboBox cmbGen = new ComboBox
                {
                    Location = new Point(170, 170),
                    Width = 300,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Name = "cmbGen"
                };
                cmbGen.Items.AddRange(new object[] { "M", "F" });

                TextBox txtAdresa = new TextBox
                {
                    Location = new Point(170, 200),
                    Width = 300,
                    Height = 60,
                    Multiline = true,
                    Name = "txtAdresa"
                };

                TextBox txtTelefon = new TextBox
                {
                    Location = new Point(170, 270),
                    Width = 300,
                    Name = "txtTelefon"
                };

                TextBox txtEmail = new TextBox
                {
                    Location = new Point(170, 300),
                    Width = 300,
                    Name = "txtEmail"
                };

                ComboBox cmbGrupaSanguina = new ComboBox
                {
                    Location = new Point(170, 330),
                    Width = 300,
                    Name = "cmbGrupaSanguina"
                };
                cmbGrupaSanguina.Items.AddRange(new object[] { "0+", "0-", "A+", "A-", "B+", "B-", "AB+", "AB-" });

                TextBox txtAlergii = new TextBox
                {
                    Location = new Point(170, 360),
                    Width = 300,
                    Height = 60,
                    Multiline = true,
                    Name = "txtAlergii"
                };

                Label lblMesajEroare = new Label
                {
                    Location = new Point(10, 430),
                    Width = 460,
                    Height = 40,
                    ForeColor = Color.Red,
                    Visible = false,
                    Name = "lblMesajEroare"
                };

                txtNume.Leave += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNume.Text))
                    {
                        lblNume.ForeColor = Color.Red;
                        txtNume.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "Numele este obligatoriu!";
                        lblMesajEroare.Visible = true;
                    }
                    else if (!Regex.IsMatch(txtNume.Text, @"^[a-zA-Z\s-]+$"))
                    {
                        lblNume.ForeColor = Color.Red;
                        txtNume.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "Numele poate contine doar litere, spatii si cratime!";
                        lblMesajEroare.Visible = true;
                    }
                    else
                    {
                        lblNume.ForeColor = SystemColors.ControlText;
                        txtNume.BackColor = SystemColors.Window;
                        lblMesajEroare.Visible = false;
                    }
                };

                txtPrenume.Leave += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtPrenume.Text))
                    {
                        lblPrenume.ForeColor = Color.Red;
                        txtPrenume.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "Prenumele este obligatoriu!";
                        lblMesajEroare.Visible = true;
                    }
                    else if (!Regex.IsMatch(txtPrenume.Text, @"^[a-zA-Z\s-]+$"))
                    {
                        lblPrenume.ForeColor = Color.Red;
                        txtPrenume.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "Prenumele poate contine doar litere, spatii si cratime!";
                        lblMesajEroare.Visible = true;
                    }
                    else
                    {
                        lblPrenume.ForeColor = SystemColors.ControlText;
                        txtPrenume.BackColor = SystemColors.Window;
                        lblMesajEroare.Visible = false;
                    }
                };

                txtCNP.Leave += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtCNP.Text))
                    {
                        lblCNP.ForeColor = Color.Red;
                        txtCNP.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "CNP-ul este obligatoriu!";
                        lblMesajEroare.Visible = true;
                    }
                    else if (txtCNP.Text.Length != 13 || !Regex.IsMatch(txtCNP.Text, @"^\d+$"))
                    {
                        lblCNP.ForeColor = Color.Red;
                        txtCNP.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "CNP-ul trebuie sa contina exact 13 cifre!";
                        lblMesajEroare.Visible = true;
                    }
                    else
                    {
                        Pacient[] pacientiExistenti = adminPacienti.GetPacienti(out int nrPacientiExistenti);
                        bool cnpExistent = false;
                        foreach (var pacient in pacientiExistenti)
                        {
                            if (pacient.CNP == txtCNP.Text)
                            {
                                cnpExistent = true;
                                break;
                            }
                        }

                        if (cnpExistent)
                        {
                            lblCNP.ForeColor = Color.Red;
                            txtCNP.BackColor = Color.LightPink;
                            lblMesajEroare.Text = "Exista deja un pacient inregistrat cu acest CNP!";
                            lblMesajEroare.Visible = true;
                        }
                        else
                        {
                            lblCNP.ForeColor = SystemColors.ControlText;
                            txtCNP.BackColor = SystemColors.Window;
                            lblMesajEroare.Visible = false;
                        }
                    }
                };

                dtpDataNasterii.ValueChanged += (s, ev) =>
                {
                    if (dtpDataNasterii.Value > DateTime.Now)
                    {
                        lblDataNasterii.ForeColor = Color.Red;
                        lblMesajEroare.Text = "Data nasterii nu poate fi in viitor!";
                        lblMesajEroare.Visible = true;
                    }
                    else if (DateTime.Now.Year - dtpDataNasterii.Value.Year > 120)
                    {
                        lblDataNasterii.ForeColor = Color.Red;
                        lblMesajEroare.Text = "Data nasterii nu este valida (varsta prea mare)!";
                        lblMesajEroare.Visible = true;
                    }
                    else
                    {
                        lblDataNasterii.ForeColor = SystemColors.ControlText;
                        lblMesajEroare.Visible = false;
                    }
                };

                cmbGen.SelectedIndexChanged += (s, ev) =>
                {
                    if (cmbGen.SelectedIndex == -1)
                    {
                        lblGen.ForeColor = Color.Red;
                        lblMesajEroare.Text = "Selectarea genului este obligatorie!";
                        lblMesajEroare.Visible = true;
                    }
                    else
                    {
                        lblGen.ForeColor = SystemColors.ControlText;
                        lblMesajEroare.Visible = false;
                    }
                };

                txtAdresa.Leave += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtAdresa.Text))
                    {
                        lblAdresa.ForeColor = Color.Red;
                        txtAdresa.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "Adresa este obligatorie!";
                        lblMesajEroare.Visible = true;
                    }
                    else if (txtAdresa.Text.Length < 5)
                    {
                        lblAdresa.ForeColor = Color.Red;
                        txtAdresa.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "Adresa trebuie sa contina minim 5 caractere!";
                        lblMesajEroare.Visible = true;
                    }
                    else
                    {
                        lblAdresa.ForeColor = SystemColors.ControlText;
                        txtAdresa.BackColor = SystemColors.Window;
                        lblMesajEroare.Visible = false;
                    }
                };

                txtTelefon.Leave += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtTelefon.Text))
                    {
                        lblTelefon.ForeColor = Color.Red;
                        txtTelefon.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "Numarul de telefon este obligatoriu!";
                        lblMesajEroare.Visible = true;
                    }
                    else if (!Regex.IsMatch(txtTelefon.Text, @"^(07\d{8}|02\d{8}|03\d{8})$"))
                    {
                        lblTelefon.ForeColor = Color.Red;
                        txtTelefon.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "Formatul numarului de telefon este invalid (ex: 07xxxxxxxx, 02xxxxxxxx)!";
                        lblMesajEroare.Visible = true;
                    }
                    else
                    {
                        lblTelefon.ForeColor = SystemColors.ControlText;
                        txtTelefon.BackColor = SystemColors.Window;
                        lblMesajEroare.Visible = false;
                    }
                };

                txtEmail.Leave += (s, ev) =>
                {
                    if (!string.IsNullOrWhiteSpace(txtEmail.Text) &&
                        !Regex.IsMatch(txtEmail.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                    {
                        lblEmail.ForeColor = Color.Red;
                        txtEmail.BackColor = Color.LightPink;
                        lblMesajEroare.Text = "Formatul adresei de email este invalid!";
                        lblMesajEroare.Visible = true;
                    }
                    else
                    {
                        lblEmail.ForeColor = SystemColors.ControlText;
                        txtEmail.BackColor = SystemColors.Window;
                        lblMesajEroare.Visible = false;
                    }
                };

                Button btnSalveazaPacient = new Button
                {
                    Text = "Salveaza Pacient",
                    Location = new Point(10, 480),
                    Width = 460,
                    Height = 40,
                    Name = "btnSalveazaPacient"
                };

                btnSalveazaPacient.Click += (s, ev) =>
                {
                    foreach (Label lbl in eticheteCampuri)
                    {
                        lbl.ForeColor = SystemColors.ControlText;
                    }
                    txtNume.BackColor = SystemColors.Window;
                    txtPrenume.BackColor = SystemColors.Window;
                    txtCNP.BackColor = SystemColors.Window;
                    txtAdresa.BackColor = SystemColors.Window;
                    txtTelefon.BackColor = SystemColors.Window;
                    txtEmail.BackColor = SystemColors.Window;
                    txtAlergii.BackColor = SystemColors.Window;
                    lblMesajEroare.Visible = false;

                    bool valid = true;
                    string mesajEroare = "";

                    if (string.IsNullOrWhiteSpace(txtNume.Text))
                    {
                        lblNume.ForeColor = Color.Red;
                        txtNume.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Numele este obligatoriu!";
                    }
                    else if (!Regex.IsMatch(txtNume.Text, @"^[a-zA-Z\s-]+$"))
                    {
                        lblNume.ForeColor = Color.Red;
                        txtNume.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Numele poate contine doar litere, spatii si cratime!";
                    }

                    if (string.IsNullOrWhiteSpace(txtPrenume.Text))
                    {
                        lblPrenume.ForeColor = Color.Red;
                        txtPrenume.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Prenumele este obligatoriu!";
                    }
                    else if (!Regex.IsMatch(txtPrenume.Text, @"^[a-zA-Z\s-]+$"))
                    {
                        lblPrenume.ForeColor = Color.Red;
                        txtPrenume.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Prenumele poate contine doar litere, spatii si cratime!";
                    }

                    if (string.IsNullOrWhiteSpace(txtCNP.Text))
                    {
                        lblCNP.ForeColor = Color.Red;
                        txtCNP.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "CNP-ul este obligatoriu!";
                    }
                    else if (txtCNP.Text.Length != 13 || !Regex.IsMatch(txtCNP.Text, @"^\d+$"))
                    {
                        lblCNP.ForeColor = Color.Red;
                        txtCNP.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "CNP-ul trebuie sa contina exact 13 cifre!";
                    }
                    else
                    {
                        Pacient[] pacientiExistenti = adminPacienti.GetPacienti(out int nrPacientiExistenti);
                        foreach (var pacient in pacientiExistenti)
                        {
                            if (pacient.CNP == txtCNP.Text)
                            {
                                valid = false;
                                lblCNP.ForeColor = Color.Red;
                                txtCNP.BackColor = Color.LightPink;
                                mesajEroare = "Exista deja un pacient inregistrat cu acest CNP!";
                                break;
                            }
                        }
                    }

                    if (dtpDataNasterii.Value > DateTime.Now)
                    {
                        lblDataNasterii.ForeColor = Color.Red;
                        valid = false;
                        mesajEroare = "Data nasterii nu poate fi in viitor!";
                    }
                    else if (DateTime.Now.Year - dtpDataNasterii.Value.Year > 120)
                    {
                        lblDataNasterii.ForeColor = Color.Red;
                        valid = false;
                        mesajEroare = "Data nasterii nu este valida (varsta prea mare)!";
                    }

                    if (cmbGen.SelectedIndex == -1)
                    {
                        lblGen.ForeColor = Color.Red;
                        valid = false;
                        mesajEroare = "Selectarea genului este obligatorie!";
                    }

                    if (string.IsNullOrWhiteSpace(txtAdresa.Text))
                    {
                        lblAdresa.ForeColor = Color.Red;
                        txtAdresa.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Adresa este obligatorie!";
                    }
                    else if (txtAdresa.Text.Length < 5)
                    {
                        lblAdresa.ForeColor = Color.Red;
                        txtAdresa.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Adresa trebuie sa contina minim 5 caractere!";
                    }

                    if (string.IsNullOrWhiteSpace(txtTelefon.Text))
                    {
                        lblTelefon.ForeColor = Color.Red;
                        txtTelefon.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Numarul de telefon este obligatoriu!";
                    }
                    else if (!Regex.IsMatch(txtTelefon.Text, @"^(07\d{8}|02\d{8}|03\d{8})$"))
                    {
                        lblTelefon.ForeColor = Color.Red;
                        txtTelefon.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Formatul numarului de telefon este invalid (ex: 07xxxxxxxx, 02xxxxxxxx)!";
                    }

                    if (!string.IsNullOrWhiteSpace(txtEmail.Text) &&
                        !Regex.IsMatch(txtEmail.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                    {
                        lblEmail.ForeColor = Color.Red;
                        txtEmail.BackColor = Color.LightPink;
                        valid = false;
                        mesajEroare = "Formatul adresei de email este invalid!";
                    }

                    if (!valid)
                    {
                        lblMesajEroare.Text = mesajEroare;
                        lblMesajEroare.Visible = true;
                        return;
                    }

                    try
                    {
                        int maxId = adminPacienti.GetMaxIdPacient();
                        int nouId = maxId + 1;

                        string[] alergii = txtAlergii.Text.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                    .Select(a => a.Trim())
                                                    .ToArray();

                        Pacient pacientNou = new Pacient(
                            nouId,
                            txtNume.Text.Trim(),
                            txtPrenume.Text.Trim(),
                            txtCNP.Text.Trim(),
                            dtpDataNasterii.Value,
                            cmbGen.SelectedItem.ToString(),
                            txtAdresa.Text.Trim(),
                            txtTelefon.Text.Trim(),
                            txtEmail.Text.Trim(),
                            cmbGrupaSanguina.SelectedIndex != -1 ? cmbGrupaSanguina.SelectedItem.ToString() : ""
                        );

                        pacientNou.SetAlergii(alergii);

                        adminPacienti.AddPacient(pacientNou, caleCompletaFisierPacienti);
                        MessageBox.Show($"Pacientul '{pacientNou.Nume} {pacientNou.Prenume}' a fost adaugat cu succes!",
                            "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formAdaugarePacient.DialogResult = DialogResult.OK;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Eroare la adaugarea pacientului: {ex.Message}",
                            "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                panelFormular.Controls.Add(lblTitluFormular);
                foreach (Label lbl in eticheteCampuri)
                {
                    panelFormular.Controls.Add(lbl);
                }
                panelFormular.Controls.Add(txtNume);
                panelFormular.Controls.Add(txtPrenume);
                panelFormular.Controls.Add(txtCNP);
                panelFormular.Controls.Add(dtpDataNasterii);
                panelFormular.Controls.Add(cmbGen);
                panelFormular.Controls.Add(txtAdresa);
                panelFormular.Controls.Add(txtTelefon);
                panelFormular.Controls.Add(txtEmail);
                panelFormular.Controls.Add(cmbGrupaSanguina);
                panelFormular.Controls.Add(txtAlergii);
                panelFormular.Controls.Add(lblMesajEroare);
                panelFormular.Controls.Add(btnSalveazaPacient);

                formAdaugarePacient.Controls.Add(panelFormular);
                formAdaugarePacient.ShowDialog();
            }
        }

        private void BtnModificarePacient_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");
        }

        private void BtnStergerePacient_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");
        }

        private void BtnInapoi_Click(object sender, EventArgs e)
        {
            this.Hide();

            FormPrincipal formPrincipal = Application.OpenForms.OfType<FormPrincipal>().FirstOrDefault();

            if (formPrincipal != null)
            {
                formPrincipal.Show();
            }
            else
            {
                Application.Exit();
            }
        }

        private void BtnVizualizarePacienti_Click(object sender, EventArgs e)
        {
            Pacient[] pacienti = adminPacienti.GetPacienti(out int nrPacienti);

            using (Form formVizualizare = new Form())
            {
                formVizualizare.Text = "Toti Pacientii";
                formVizualizare.Size = new Size(700, 400);
                formVizualizare.StartPosition = FormStartPosition.CenterScreen;

                ListView listView = new ListView
                {
                    Dock = DockStyle.Fill,
                    View = View.Details,
                    FullRowSelect = true,
                    GridLines = true
                };

                listView.Columns.Add("ID", 50);
                listView.Columns.Add("Nume", 150);
                listView.Columns.Add("CNP", 120);
                listView.Columns.Add("Adresa", 150);
                listView.Columns.Add("Telefon", 100);
                listView.Columns.Add("Data Nasterii", 100);
                listView.Columns.Add("Gen", 50);

                foreach (var pacient in pacienti)
                {
                    ListViewItem item = new ListViewItem(pacient.IdPacient.ToString());
                    item.SubItems.Add(pacient.Nume);
                    item.SubItems.Add(pacient.CNP);
                    item.SubItems.Add(pacient.Adresa);
                    item.SubItems.Add(pacient.Telefon);
                    item.SubItems.Add(pacient.DataNasterii.ToShortDateString());
                    item.SubItems.Add(pacient.Gen);

                    listView.Items.Add(item);
                }

                formVizualizare.Controls.Add(listView);
                formVizualizare.ShowDialog();
            }
        }
    }
}
