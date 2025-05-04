using System;
using System.Windows.Forms;
using System.Drawing;
using GestionareSpital;
using LibrarieModele;
using NivelStocareDate;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using MetroFramework.Controls;
using MetroFramework.Forms;
using System.Collections.Generic;

namespace UI
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source?.IndexOf(value, comparisonType) >= 0;
        }
    }
    public partial class FormGestionarePacienti : MetroForm
    {
        private AdministrarePacienti_FisierText adminPacienti;
        private User utilizatorCurent;

        private Button btnAdaugarePacient;
        private Button btnModificarePacient;
        private Button btnStergerePacient;
        private Button btnVizualizarePacienti;
        private Button BtnVizualizarePacientiSearch;
        private Button btnInapoi;
        private TextBox txtNume;
        private TextBox txtPrenume;
        private TextBox txtTelefon;
        private TextBox txtEmail;
        private TextBox txtCNP;
        private TextBox txtAdresa;
        private TextBox txtAlergii;
        private DateTimePicker dtpDataNasterii;
        private ComboBox cmbGrupaSanguina;
        private ComboBox cmbGen;

        public FormGestionarePacienti(
            AdministrarePacienti_FisierText pacientiAdmin,
            User utilizatorCurent)
        {
            this.utilizatorCurent = utilizatorCurent;
            adminPacienti = pacientiAdmin;

            btnAdaugarePacient = new MetroButton { Text = "Adaugare pacient" };
            btnModificarePacient = new MetroButton { Text = "Modificare pacient" };
            btnStergerePacient = new MetroButton { Text = "Stergere pacient" };
            btnVizualizarePacienti = new MetroButton { Text = "Vizualizare pacienti" };
            BtnVizualizarePacientiSearch = new MetroButton { Text = "Cautare pacienti dupa criteriu" };
            btnInapoi = new MetroButton { Text = "Inapoi" };

            btnAdaugarePacient.Size = new Size(200, 40);
            btnModificarePacient.Size = new Size(200, 40);
            btnStergerePacient.Size = new Size(200, 40);
            btnVizualizarePacienti.Size = new Size(200, 40);
            BtnVizualizarePacientiSearch.Size = new Size(200, 40);
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
            panel.Controls.Add(BtnVizualizarePacientiSearch);
            panel.Controls.Add(btnInapoi);
            this.Controls.Add(panel);

            btnAdaugarePacient.Click += BtnAdaugarePacient_Click;
            btnModificarePacient.Click += BtnModificarePacient_Click;
            btnStergerePacient.Click += BtnStergerePacient_Click;
            btnVizualizarePacienti.Click += BtnVizualizarePacienti_Click;
            BtnVizualizarePacientiSearch.Click += BtnVizualizarePacientiSearch_Click;
            btnInapoi.Click += BtnInapoi_Click;
            this.Style = MetroFramework.MetroColorStyle.Black;
            this.Size = new Size(800, 400);
        }


        private static class ConstanteValidare
        {
            public const int CNP_LENGTH = 13;
            public const int ADRESA_MIN_LENGTH = 5;
            public const int VARSTA_MAXIMA = 120;
            public const string PATTERN_NUME = @"^[a-zA-Z\s-]+$";
            public const string PATTERN_CNP = @"^\d+$";
            public const string PATTERN_EMAIL = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            public const string PATTERN_TELEFON = @"^(07\d{8}|02\d{8}|03\d{8})$";
        }

        private static class MesajeEroare
        {
            public const string NUME_OBLIGATORIU = "Numele este obligatoriu!";
            public const string NUME_FORMAT = "Numele poate contine doar litere, spatii si cratime!";
            public const string PRENUME_OBLIGATORIU = "Prenumele este obligatoriu!";
            public const string PRENUME_FORMAT = "Prenumele poate contine doar litere, spatii si cratime!";
            public const string CNP_OBLIGATORIU = "CNP-ul este obligatoriu!";
            public const string CNP_FORMAT = "CNP-ul trebuie sa contina exact 13 cifre!";
            public const string CNP_EXISTENT = "Exista deja un pacient inregistrat cu acest CNP!";
            public const string DATA_VIITOR = "Data nasterii nu poate fi in viitor!";
            public const string DATA_INVALID = "Data nasterii nu este valida (varsta prea mare)!";
            public const string GEN_OBLIGATORIU = "Selectarea genului este obligatorie!";
            public const string ADRESA_OBLIGATORIE = "Adresa este obligatorie!";
            public const string ADRESA_LUNGIME = "Adresa trebuie sa contina minim 5 caractere!";
            public const string TELEFON_OBLIGATORIU = "Numarul de telefon este obligatoriu!";
            public const string TELEFON_FORMAT = "Formatul numarului de telefon este invalid (ex: 07xxxxxxxx, 02xxxxxxxx)!";
            public const string EMAIL_FORMAT = "Formatul adresei de email este invalid!";
            public const string TELEFON_EXISTENT = "Deja exista un pacient cu acest numar de telefon.";
        }

        private class ValidatorPacient
        {
            private readonly MetroLabel lblMesajEroare;
            private readonly Dictionary<Control, MetroLabel> controlsLabelsMap;

            public ValidatorPacient(MetroLabel lblMesajEroare, Dictionary<Control, MetroLabel> controlsLabelsMap)
            {
                this.lblMesajEroare = lblMesajEroare;
                this.controlsLabelsMap = controlsLabelsMap;
            }

            private void MarcareCampInvalid(Control control, string mesaj)
            {
                if (controlsLabelsMap.ContainsKey(control))
                {
                    controlsLabelsMap[control].ForeColor = Color.Red;
                }

                if (control is TextBox textBox)
                {
                    textBox.BackColor = Color.LightPink;
                }

                lblMesajEroare.Text = mesaj;
                lblMesajEroare.Visible = true;
            }

            public void ResetValidare()
            {
                foreach (var pair in controlsLabelsMap)
                {
                    pair.Value.ForeColor = SystemColors.ControlText;
                    if (pair.Key is TextBox textBox)
                    {
                        textBox.BackColor = SystemColors.Window;
                    }
                }
                lblMesajEroare.Visible = false;
            }

            public bool ValidareNume(TextBox txtNume)
            {
                if (string.IsNullOrWhiteSpace(txtNume.Text))
                {
                    MarcareCampInvalid(txtNume, MesajeEroare.NUME_OBLIGATORIU);
                    return false;
                }
                if (!Regex.IsMatch(txtNume.Text, ConstanteValidare.PATTERN_NUME))
                {
                    MarcareCampInvalid(txtNume, MesajeEroare.NUME_FORMAT);
                    return false;
                }
                return true;
            }

            public bool ValidarePrenume(TextBox txtPrenume)
            {
                if (string.IsNullOrWhiteSpace(txtPrenume.Text))
                {
                    MarcareCampInvalid(txtPrenume, MesajeEroare.PRENUME_OBLIGATORIU);
                    return false;
                }
                if (!Regex.IsMatch(txtPrenume.Text, ConstanteValidare.PATTERN_NUME))
                {
                    MarcareCampInvalid(txtPrenume, MesajeEroare.PRENUME_FORMAT);
                    return false;
                }
                return true;
            }

            public bool ValidareCNP(TextBox txtCNP, AdministrarePacienti_FisierText adminPacienti)
            {
                if (string.IsNullOrWhiteSpace(txtCNP.Text))
                {
                    MarcareCampInvalid(txtCNP, MesajeEroare.CNP_OBLIGATORIU);
                    return false;
                }
                if (txtCNP.Text.Length != ConstanteValidare.CNP_LENGTH || !Regex.IsMatch(txtCNP.Text, ConstanteValidare.PATTERN_CNP))
                {
                    MarcareCampInvalid(txtCNP, MesajeEroare.CNP_FORMAT);
                    return false;
                }

                Pacient[] pacientiExistenti = adminPacienti.GetPacienti(out int nrPacientiExistenti);
                foreach (var pacient in pacientiExistenti)
                {
                    if (pacient.CNP == txtCNP.Text)
                    {
                        MarcareCampInvalid(txtCNP, MesajeEroare.CNP_EXISTENT);
                        return false;
                    }
                }
                return true;
            }

            public bool ValidareDataNasterii(DateTimePicker dtpDataNasterii)
            {
                if (dtpDataNasterii.Value > DateTime.Now)
                {
                    controlsLabelsMap[dtpDataNasterii].ForeColor = Color.Red;
                    lblMesajEroare.Text = MesajeEroare.DATA_VIITOR;
                    lblMesajEroare.Visible = true;
                    return false;
                }
                if (DateTime.Now.Year - dtpDataNasterii.Value.Year > ConstanteValidare.VARSTA_MAXIMA)
                {
                    controlsLabelsMap[dtpDataNasterii].ForeColor = Color.Red;
                    lblMesajEroare.Text = MesajeEroare.DATA_INVALID;
                    lblMesajEroare.Visible = true;
                    return false;
                }
                return true;
            }

            public bool ValidareGen(ComboBox cmbGen)
            {
                if (cmbGen.SelectedIndex == -1)
                {
                    controlsLabelsMap[cmbGen].ForeColor = Color.Red;
                    lblMesajEroare.Text = MesajeEroare.GEN_OBLIGATORIU;
                    lblMesajEroare.Visible = true;
                    return false;
                }
                return true;
            }

            public bool ValidareAdresa(TextBox txtAdresa)
            {
                if (string.IsNullOrWhiteSpace(txtAdresa.Text))
                {
                    MarcareCampInvalid(txtAdresa, MesajeEroare.ADRESA_OBLIGATORIE);
                    return false;
                }
                if (txtAdresa.Text.Length < ConstanteValidare.ADRESA_MIN_LENGTH)
                {
                    MarcareCampInvalid(txtAdresa, MesajeEroare.ADRESA_LUNGIME);
                    return false;
                }
                return true;
            }

            public bool ValidareTelefon(TextBox txtTelefon, AdministrarePacienti_FisierText adminPacienti)
            {
                if (string.IsNullOrWhiteSpace(txtTelefon.Text))
                {
                    MarcareCampInvalid(txtTelefon, MesajeEroare.TELEFON_OBLIGATORIU);
                    return false;
                }
                if (!Regex.IsMatch(txtTelefon.Text, ConstanteValidare.PATTERN_TELEFON))
                {
                    MarcareCampInvalid(txtTelefon, MesajeEroare.TELEFON_FORMAT);
                    return false;
                }

                Pacient[] pacientiExistenti = adminPacienti.GetPacienti(out int nrPacientiExistenti);
                foreach (var pacient in pacientiExistenti)
                {
                    if (pacient.Telefon == txtTelefon.Text)
                    {
                        MarcareCampInvalid(txtTelefon, MesajeEroare.TELEFON_EXISTENT);
                        return false;
                    }
                }
                return true;
            }


            public bool ValidareEmail(TextBox txtEmail)
            {
                if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !Regex.IsMatch(txtEmail.Text, ConstanteValidare.PATTERN_EMAIL))
                {
                    MarcareCampInvalid(txtEmail, MesajeEroare.EMAIL_FORMAT);
                    return false;
                }
                return true;
            }

            public bool ValideazaPacient(
                TextBox txtNume,
                TextBox txtPrenume,
                TextBox txtCNP,
                DateTimePicker dtpDataNasterii,
                ComboBox cmbGen,
                TextBox txtAdresa,
                TextBox txtTelefon,
                TextBox txtEmail,
                AdministrarePacienti_FisierText adminPacienti)
            {
                ResetValidare();

                bool valid = true;

                if (!ValidareNume(txtNume))
                    valid = false;

                if (!ValidarePrenume(txtPrenume))
                    valid = false;

                if (!ValidareCNP(txtCNP, adminPacienti))
                    valid = false;

                if (!ValidareDataNasterii(dtpDataNasterii))
                    valid = false;

                if (!ValidareGen(cmbGen))
                    valid = false;

                if (!ValidareAdresa(txtAdresa))
                    valid = false;

                if (!ValidareTelefon(txtTelefon, adminPacienti))
                    valid = false;

                if (!ValidareEmail(txtEmail))
                    valid = false;

                return valid;
            }
        }

        private class InitializatorFormularPacient
        {
            private readonly MetroForm formular;
            private readonly Dictionary<Control, MetroLabel> controlsLabelsMap;
            private MetroLabel lblMesajEroare;
            private ValidatorPacient validator;

            public InitializatorFormularPacient(MetroForm formular)
            {
                this.formular = formular;
                this.controlsLabelsMap = new Dictionary<Control, MetroLabel>();
            }

            public TextBox TxtNume { get; private set; }
            public TextBox TxtPrenume { get; private set; }
            public TextBox TxtCNP { get; private set; }
            public DateTimePicker DtpDataNasterii { get; private set; }
            public ComboBox CmbGen { get; private set; }
            public TextBox TxtAdresa { get; private set; }
            public TextBox TxtTelefon { get; private set; }
            public TextBox TxtEmail { get; private set; }
            public ComboBox CmbGrupaSanguina { get; private set; }
            public TextBox TxtAlergii { get; private set; }
            public Button BtnSalveazaPacient { get; private set; }
            public MetroPanel PanelFormular { get; private set; }
            public ValidatorPacient Validator => validator;

            public void InitializeazaFormular()
            {
                PanelFormular = new MetroPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    Name = "panelFormular"
                };

                MetroLabel lblTitluFormular = new MetroLabel
                {
                    Text = "Adaugare Pacient Nou",
                    Location = new Point(10, 10),
                    Width = 460,
                    Font = new Font(SystemFonts.DefaultFont.FontFamily, 10, FontStyle.Bold),
                    Name = "lblTitluFormular"
                };
                PanelFormular.Controls.Add(lblTitluFormular);

                AadaugaCampuriFormular();

                lblMesajEroare = new MetroLabel
                {
                    Location = new Point(10, 430),
                    Width = 460,
                    Height = 40,
                    ForeColor = Color.Red,
                    Visible = false,
                    Name = "lblMesajEroare"
                };
                PanelFormular.Controls.Add(lblMesajEroare);

                BtnSalveazaPacient = new Button
                {
                    Text = "Salveaza Pacient",
                    Location = new Point(10, 480),
                    Width = 460,
                    Height = 40,
                    Name = "btnSalveazaPacient"
                };
                PanelFormular.Controls.Add(BtnSalveazaPacient);

                validator = new ValidatorPacient(lblMesajEroare, controlsLabelsMap);

                formular.Controls.Add(PanelFormular);
            }

            private void AadaugaCampuriFormular()
            {
                MetroLabel lblNume = CreeazaEticheta("Nume *:", 10, 50, 150, "lblNume");
                MetroLabel lblPrenume = CreeazaEticheta("Prenume *:", 10, 80, 150, "lblPrenume");
                MetroLabel lblCNP = CreeazaEticheta("CNP *:", 10, 110, 150, "lblCNP");
                MetroLabel lblDataNasterii = CreeazaEticheta("Data nasterii *:", 10, 140, 150, "lblDataNasterii");
                MetroLabel lblGen = CreeazaEticheta("Gen *:", 10, 170, 150, "lblGen");
                MetroLabel lblAdresa = CreeazaEticheta("Adresa *:", 10, 200, 150, "lblAdresa");
                MetroLabel lblTelefon = CreeazaEticheta("Telefon *:", 10, 270, 150, "lblTelefon");
                MetroLabel lblEmail = CreeazaEticheta("Email:", 10, 300, 150, "lblEmail");
                MetroLabel lblGrupaSanguina = CreeazaEticheta("Grupa sanguina:", 10, 330, 150, "lblGrupaSanguina");
                MetroLabel lblAlergii = CreeazaEticheta("Alergii:", 10, 360, 150, "lblAlergii");

                TxtNume = CreeazaTextBox(170, 50, 300, "txtNume");
                TxtPrenume = CreeazaTextBox(170, 80, 300, "txtPrenume");
                TxtCNP = CreeazaTextBox(170, 110, 300, "txtCNP", ConstanteValidare.CNP_LENGTH);
                DtpDataNasterii = new DateTimePicker
                {
                    Format = DateTimePickerFormat.Short,
                    Location = new Point(170, 140),
                    Width = 300,
                    Name = "dtpDataNasterii"
                };
                CmbGen = new ComboBox
                {
                    Location = new Point(170, 170),
                    Width = 300,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Name = "cmbGen"
                };
                CmbGen.Items.AddRange(new object[] { "M", "F" });
                TxtAdresa = CreeazaTextBox(170, 200, 300, "txtAdresa", multiline: true, height: 60);
                TxtTelefon = CreeazaTextBox(170, 270, 300, "txtTelefon");
                TxtEmail = CreeazaTextBox(170, 300, 300, "txtEmail");
                CmbGrupaSanguina = new ComboBox
                {
                    Location = new Point(170, 330),
                    Width = 300,
                    Name = "cmbGrupaSanguina"
                };
                CmbGrupaSanguina.Items.AddRange(new object[] { "0+", "0-", "A+", "A-", "B+", "B-", "AB+", "AB-" });
                TxtAlergii = CreeazaTextBox(170, 360, 300, "txtAlergii", multiline: true, height: 60);

                controlsLabelsMap.Add(TxtNume, lblNume);
                controlsLabelsMap.Add(TxtPrenume, lblPrenume);
                controlsLabelsMap.Add(TxtCNP, lblCNP);
                controlsLabelsMap.Add(DtpDataNasterii, lblDataNasterii);
                controlsLabelsMap.Add(CmbGen, lblGen);
                controlsLabelsMap.Add(TxtAdresa, lblAdresa);
                controlsLabelsMap.Add(TxtTelefon, lblTelefon);
                controlsLabelsMap.Add(TxtEmail, lblEmail);
                controlsLabelsMap.Add(CmbGrupaSanguina, lblGrupaSanguina);
                controlsLabelsMap.Add(TxtAlergii, lblAlergii);

                PanelFormular.Controls.Add(TxtNume);
                PanelFormular.Controls.Add(TxtPrenume);
                PanelFormular.Controls.Add(TxtCNP);
                PanelFormular.Controls.Add(DtpDataNasterii);
                PanelFormular.Controls.Add(CmbGen);
                PanelFormular.Controls.Add(TxtAdresa);
                PanelFormular.Controls.Add(TxtTelefon);
                PanelFormular.Controls.Add(TxtEmail);
                PanelFormular.Controls.Add(CmbGrupaSanguina);
                PanelFormular.Controls.Add(TxtAlergii);
            }

            private MetroLabel CreeazaEticheta(string text, int x, int y, int width, string name)
            {
                MetroLabel label = new MetroLabel
                {
                    Text = text,
                    Location = new Point(x, y),
                    Width = width,
                    Name = name
                };
                PanelFormular.Controls.Add(label);
                return label;
            }

            private TextBox CreeazaTextBox(int x, int y, int width, string name, int maxLength = 0, bool multiline = false, int height = 0)
            {
                TextBox textBox = new TextBox
                {
                    Location = new Point(x, y),
                    Width = width,
                    Name = name
                };

                if (maxLength > 0)
                    textBox.MaxLength = maxLength;

                if (multiline)
                {
                    textBox.Multiline = true;
                    textBox.Height = height;
                }

                return textBox;
            }

            public void AtaseazaValidatoriEvenimente(AdministrarePacienti_FisierText adminPacienti)
            {
                TxtNume.Leave += (s, e) => validator.ValidareNume(TxtNume);
                TxtPrenume.Leave += (s, e) => validator.ValidarePrenume(TxtPrenume);
                TxtCNP.Leave += (s, e) => validator.ValidareCNP(TxtCNP, adminPacienti);
                DtpDataNasterii.ValueChanged += (s, e) => validator.ValidareDataNasterii(DtpDataNasterii);
                CmbGen.SelectedIndexChanged += (s, e) => validator.ValidareGen(CmbGen);
                TxtAdresa.Leave += (s, e) => validator.ValidareAdresa(TxtAdresa);
                TxtTelefon.Leave += (s, e) => validator.ValidareTelefon(TxtTelefon, adminPacienti);
                TxtEmail.Leave += (s, e) => validator.ValidareEmail(TxtEmail);
            }
        }

        private void BtnAdaugarePacient_Click(object sender, EventArgs e)
        {
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string numeFisierPacienti = "Pacienti.txt";
            string caleCompletaFisierPacienti = Path.Combine(locatieFisierSolutie, numeFisierPacienti);

            AdministrarePacienti_FisierText adminPacientiLocal = adminPacienti;
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugarePacienti))
            {
                using (MetroForm formAdaugarePacient = new MetroForm())
                {
                    formAdaugarePacient.Size = new Size(1600, 800);
                    formAdaugarePacient.StartPosition = FormStartPosition.CenterScreen;
                    formAdaugarePacient.Style = MetroFramework.MetroColorStyle.Black;

                    InitializatorFormularPacient initializator = new InitializatorFormularPacient(formAdaugarePacient);
                    initializator.InitializeazaFormular();
                    initializator.AtaseazaValidatoriEvenimente(adminPacientiLocal);

                    initializator.BtnSalveazaPacient.Click += (s, ev) =>
                    {
                        if (!initializator.Validator.ValideazaPacient(
                            initializator.TxtNume,
                            initializator.TxtPrenume,
                            initializator.TxtCNP,
                            initializator.DtpDataNasterii,
                            initializator.CmbGen,
                            initializator.TxtAdresa,
                            initializator.TxtTelefon,
                            initializator.TxtEmail,
                            adminPacientiLocal))
                        {
                            return;
                        }

                        try
                        {
                            int maxId = adminPacientiLocal.GetMaxIdPacient();
                            int nouId = maxId + 1;

                            string[] alergii = initializator.TxtAlergii.Text.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                        .Select(a => a.Trim())
                                                        .ToArray();

                            Pacient pacientNou = new Pacient(
                                nouId,
                                initializator.TxtNume.Text.Trim(),
                                initializator.TxtPrenume.Text.Trim(),
                                initializator.TxtCNP.Text.Trim(),
                                initializator.DtpDataNasterii.Value,
                                initializator.CmbGen.SelectedItem.ToString(),
                                initializator.TxtAdresa.Text.Trim(),
                                initializator.TxtTelefon.Text.Trim(),
                                initializator.TxtEmail.Text.Trim(),
                                initializator.CmbGrupaSanguina.SelectedIndex != -1 ? initializator.CmbGrupaSanguina.SelectedItem.ToString() : ""
                            );

                            pacientNou.SetAlergii(alergii);

                            adminPacientiLocal.AddPacient(pacientNou, caleCompletaFisierPacienti);
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

                    formAdaugarePacient.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a adauga pacienti.");
            }
        }


        private void BtnModificarePacient_Click(object sender, EventArgs e)
        {
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string numeFisierPacienti = "Pacienti.txt";
            string caleCompletaFisierPacienti = Path.Combine(locatieFisierSolutie, numeFisierPacienti);
            this.ControlBox = true;
            this.MinimizeBox = true;
            this.MaximizeBox = true;

            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.ModificarePacienti))
            {
                string cnpUpdate = Microsoft.VisualBasic.Interaction.InputBox("Introduceti CNP-ul pacientului pentru modificare:", "Modificare pacient");

                Pacient pacientUpdate = adminPacienti.GetPacientDupaCNP(cnpUpdate);
                if (pacientUpdate != null)
                {
                    this.Controls.Clear();
                    this.Text = "Modificare Pacient";

                    MetroPanel panel = new MetroPanel
                    {
                        Dock = DockStyle.Fill,
                        AutoScroll = true,
                        Padding = new Padding(20),
                        BackColor = Color.White
                    };

                    int labelWidth = 150;
                    int controlWidth = 350;
                    int spacing = 40;
                    int top = 20;

                    MetroLabel CreateLabel(string text, int topPosition)
                    {
                        return new MetroLabel
                        {
                            Text = text,
                            Location = new Point(10, topPosition),
                            Width = labelWidth,
                            FontSize = MetroFramework.MetroLabelSize.Medium
                        };
                    }

                    Control CreateControl(Control control, int topPosition)
                    {
                        control.Location = new Point(labelWidth + 20, topPosition);
                        control.Width = controlWidth;
                        return control;
                    }

                    var lblNume = CreateLabel("Nume", top);
                    var txtNume = (MetroTextBox)CreateControl(
                        new MetroTextBox { Text = pacientUpdate.Nume }, top);

                    top += spacing;

                    var lblPrenume = CreateLabel("Prenume", top);
                    var txtPrenume = (MetroTextBox)CreateControl(
                        new MetroTextBox { Text = pacientUpdate.Prenume }, top);

                    top += spacing;

                    var lblCNP = CreateLabel("CNP", top);
                    var txtCNP = (MetroTextBox)CreateControl(
                        new MetroTextBox { Text = pacientUpdate.CNP, ReadOnly = true }, top);

                    top += spacing;

                    var lblData = CreateLabel("Data nasterii", top);
                    var dtpData = (DateTimePicker)CreateControl(
                        new DateTimePicker { Value = pacientUpdate.DataNasterii, Format = DateTimePickerFormat.Short }, top);


                    top += spacing;

                    var lblGen = CreateLabel("Gen", top);
                    var cmbGen = (MetroComboBox)CreateControl(
                        new MetroComboBox(), top);
                    cmbGen.Items.AddRange(new object[] { "M", "F" });
                    cmbGen.SelectedItem = pacientUpdate.Gen;

                    top += spacing;

                    var lblAdresa = CreateLabel("Adresa", top);
                    var txtAdresa = (MetroTextBox)CreateControl(
                        new MetroTextBox { Text = pacientUpdate.Adresa, Multiline = true, Height = 50 }, top);

                    top += spacing;

                    var lblTelefon = CreateLabel("Telefon", top);
                    var txtTelefon = (MetroTextBox)CreateControl(
                        new MetroTextBox { Text = pacientUpdate.Telefon }, top);

                    top += spacing;

                    var lblEmail = CreateLabel("Email", top);
                    var txtEmail = (MetroTextBox)CreateControl(
                        new MetroTextBox { Text = pacientUpdate.Email }, top);

                    top += spacing;

                    var lblGrupa = CreateLabel("Grupa sanguina", top);
                    var cmbGrupa = (MetroComboBox)CreateControl(
                        new MetroComboBox(), top);
                    cmbGrupa.Items.AddRange(new object[] { "0+", "0-", "A+", "A-", "B+", "B-", "AB+", "AB-" });
                    cmbGrupa.SelectedItem = pacientUpdate.GrupaSanguina;

                    top += spacing;

                    var lblAlergii = CreateLabel("Alergii", top);
                    var txtAlergii = (MetroTextBox)CreateControl(
                        new MetroTextBox { Text = string.Join(", ", pacientUpdate.Alergii), Multiline = true, Height = 50 }, top);

                    top += spacing + 20;

                    MetroButton btnSalveaza = new MetroButton
                    {
                        Text = "Salveaza modificarile",
                        Width = 220,
                        Height = 40,
                        Location = new Point(labelWidth + 20, top),
                        Theme = MetroFramework.MetroThemeStyle.Light,
                    };

                    btnSalveaza.Click += (clickSender, clickEventArgs) =>
                    {
                        pacientUpdate.Nume = txtNume.Text;
                        pacientUpdate.Prenume = txtPrenume.Text;
                        pacientUpdate.DataNasterii = dtpData.Value;
                        pacientUpdate.Gen = cmbGen.SelectedItem.ToString();
                        pacientUpdate.Adresa = txtAdresa.Text;
                        pacientUpdate.Telefon = txtTelefon.Text;
                        pacientUpdate.Email = txtEmail.Text;
                        pacientUpdate.GrupaSanguina = cmbGrupa.SelectedItem.ToString();
                        pacientUpdate.Alergii = txtAlergii.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                               .Select(a => a.Trim()).ToArray();

                        adminPacienti.UpdatePacient(pacientUpdate, caleCompletaFisierPacienti);
                        MessageBox.Show("Pacientul a fost modificat cu succes!");
                        this.Close();
                    };

                    panel.Controls.AddRange(new Control[]
                    {
                lblNume, txtNume,
                lblPrenume, txtPrenume,
                lblCNP, txtCNP,
                lblData, dtpData,
                lblGen, cmbGen,
                lblAdresa, txtAdresa,
                lblTelefon, txtTelefon,
                lblEmail, txtEmail,
                lblGrupa, cmbGrupa,
                lblAlergii, txtAlergii,
                btnSalveaza
                    });

                    this.Controls.Add(panel);
                }
                else
                {
                    MessageBox.Show("Pacientul cu acest CNP nu a fost gasit!");
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a modifica pacienti.");
            }
        }







        private void BtnStergerePacient_Click(object sender, EventArgs e)
        {
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string numeFisierPacienti = "Pacienti.txt";
            string caleCompletaFisierPacienti = Path.Combine(locatieFisierSolutie, numeFisierPacienti);

            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.StergerePacienti))
            {
                string cnpDelete = Microsoft.VisualBasic.Interaction.InputBox("Introduceti CNP-ul pacientului pentru stergere:", "Stergere pacient");

                Pacient pacientDelete = adminPacienti.GetPacientDupaCNP(cnpDelete);
                if (pacientDelete != null)
                {
                    DialogResult result = MessageBox.Show($"Sigur doriti sa stergeti pacientul {pacientDelete.Nume} {pacientDelete.Prenume}?",
                        "Confirmare stergere", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        adminPacienti.DeletePacient(pacientDelete, caleCompletaFisierPacienti);
                        MessageBox.Show("Pacientul a fost sters cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Nu exista pacient cu acest CNP!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiuni pentru aceasta actiune!");
            }
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

            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.StergerePacienti))
            {
                MetroForm formVizualizare = new MetroForm
                {
                    Text = "Toti Pacientii",
                    Size = new Size(1200, 600),
                    StartPosition = FormStartPosition.CenterScreen,
                    MaximizeBox = false,
                    MinimizeBox = true,
                    ControlBox = true,
                    Resizable = true,
                    Theme = MetroFramework.MetroThemeStyle.Light,
                    Style = MetroFramework.MetroColorStyle.Blue
                };

                formVizualizare.Style = MetroFramework.MetroColorStyle.Black;
                ListView listView = new ListView
                {
                    Dock = DockStyle.Fill,
                    View = View.Details,
                    FullRowSelect = true,
                    GridLines = true,
                    Font = new Font("Segoe UI", 10)
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

                MetroPanel panel = new MetroPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    Padding = new Padding(10),
                    Style = MetroFramework.MetroColorStyle.Blue,
                    Theme = MetroFramework.MetroThemeStyle.Light
                };
                panel.Controls.Add(listView);

                formVizualizare.Controls.Add(panel);
                formVizualizare.ShowDialog();
            }


            else
            {
                MessageBox.Show("Nu aveti permisiunea de a vizualiza pacienti.");
            }
        }
        private void BtnVizualizarePacientiSearch_Click(object sender, EventArgs e)
        {
            Pacient[] pacienti = adminPacienti.GetPacienti(out int nrPacienti);

            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.StergerePacienti))
            {
                MetroForm formVizualizare = new MetroForm
                {
                    Text = "Toti Pacientii",
                    Size = new Size(1450, 600),
                    StartPosition = FormStartPosition.CenterScreen,
                    MaximizeBox = false,
                    MinimizeBox = true,
                    ControlBox = true,
                    Resizable = false,
                    Theme = MetroFramework.MetroThemeStyle.Light,
                    Style = MetroFramework.MetroColorStyle.Black
                };

                MetroPanel searchPanel = new MetroPanel
                {
                    Dock = DockStyle.Top,
                    Height = 100,
                    Padding = new Padding(10)
                };

                MetroButton btnCautarePacient = new MetroButton
                {
                    Text = "Cauta Pacient",
                    Size = new Size(120, 30),
                    Location = new Point(1250, 35),
                    Theme = MetroFramework.MetroThemeStyle.Light
                };

                MetroTextBox txtCautare = new MetroTextBox
                {
                    Width = 250,
                    Location = new Point(820, 35),
                    PromptText = "Introduceti textul pentru cautare"
                };

                MetroLabel lblCautare = new MetroLabel
                {
                    Text = "Cauta dupa:",
                    Location = new Point(10, 10),
                    AutoSize = true,
                    FontWeight = MetroFramework.MetroLabelWeight.Bold
                };

                MetroRadioButton rbNume = new MetroRadioButton
                {
                    Text = "Nume",
                    Location = new Point(20, 35),
                    Checked = true
                };

                MetroRadioButton rbPrenume = new MetroRadioButton
                {
                    Text = "Prenume",
                    Location = new Point(160, 35)
                };

                MetroRadioButton rbCNP = new MetroRadioButton
                {
                    Text = "CNP",
                    Location = new Point(320, 35)
                };

                MetroRadioButton rbTelefon = new MetroRadioButton
                {
                    Text = "Telefon",
                    Location = new Point(480, 35)
                };

                MetroRadioButton rbEmail = new MetroRadioButton
                {
                    Text = "Email",
                    Location = new Point(640, 35)
                };

                MetroCheckBox chkCautareExacta = new MetroCheckBox
                {
                    Text = "Potrivire exacta",
                    Location = new Point(20, 65)
                };



                searchPanel.Controls.AddRange(new Control[] {
            lblCautare, rbNume, rbPrenume, rbCNP, rbTelefon, rbEmail,
            txtCautare, btnCautarePacient, chkCautareExacta
        });

                ListView listView = new ListView
                {
                    Dock = DockStyle.Fill,
                    View = View.Details,
                    FullRowSelect = true,
                    GridLines = true,
                    Font = new Font("Segoe UI", 10)
                };

                listView.Columns.Add("ID", 50);
                listView.Columns.Add("Nume", 100);
                listView.Columns.Add("Prenume", 100);
                listView.Columns.Add("CNP", 120);
                listView.Columns.Add("Adresa", 150);
                listView.Columns.Add("Telefon", 100);
                listView.Columns.Add("Email", 120);
                listView.Columns.Add("Data Nasterii", 100);
                listView.Columns.Add("Gen", 50);

                MetroPanel mainPanel = new MetroPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true
                };

                PopulateListView(listView, pacienti);
                mainPanel.Controls.Add(listView);

                formVizualizare.Controls.Add(mainPanel);
                formVizualizare.Controls.Add(searchPanel);

                btnCautarePacient.Click += (s, args) =>
                {
                    string searchText = txtCautare.Text.Trim();
                    bool exactMatch = chkCautareExacta.Checked;

                    string searchCriteria = "";
                    if (rbNume.Checked) searchCriteria = "Nume";
                    else if (rbPrenume.Checked) searchCriteria = "Prenume";
                    else if (rbCNP.Checked) searchCriteria = "CNP";
                    else if (rbTelefon.Checked) searchCriteria = "Telefon";
                    else if (rbEmail.Checked) searchCriteria = "Email";

                    Pacient[] filteredPacienti = SearchPacienti(pacienti, searchCriteria, searchText, exactMatch);

                    PopulateListView(listView, filteredPacienti);
                };

                formVizualizare.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a vizualiza pacienti.");
            }
        }

        private void PopulateListView(ListView listView, Pacient[] pacienti)
        {
            listView.Items.Clear();

            if (pacienti == null || pacienti.Length == 0)
            {
                MessageBox.Show("Nu au fost gasiti pacienti care sa corespunda criteriilor de cautare.",
                    "Cautare", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var pacient in pacienti)
            {
                ListViewItem item = new ListViewItem(pacient.IdPacient.ToString());
                item.SubItems.Add(pacient.Nume);
                item.SubItems.Add(pacient.Prenume);
                item.SubItems.Add(pacient.CNP);
                item.SubItems.Add(pacient.Adresa);
                item.SubItems.Add(pacient.Telefon);
                item.SubItems.Add(pacient.Email);
                item.SubItems.Add(pacient.DataNasterii.ToShortDateString());
                item.SubItems.Add(pacient.Gen);

                listView.Items.Add(item);
            }
        }

        private Pacient[] SearchPacienti(Pacient[] allPacienti, string searchCriteria, string searchText, bool exactMatch)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return allPacienti;

            List<Pacient> results = new List<Pacient>();
            StringComparison comparison = exactMatch ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            foreach (var pacient in allPacienti)
            {
                bool isMatch = false;

                switch (searchCriteria)
                {
                    case "Nume":
                        isMatch = exactMatch
                            ? pacient.Nume.Equals(searchText, comparison)
                            : pacient.Nume.Contains(searchText, comparison);
                        break;
                    case "Prenume":
                        isMatch = exactMatch
                            ? pacient.Prenume.Equals(searchText, comparison)
                            : pacient.Prenume.Contains(searchText, comparison);
                        break;
                    case "CNP":
                        isMatch = exactMatch
                            ? pacient.CNP.Equals(searchText, comparison)
                            : pacient.CNP.Contains(searchText, comparison);
                        break;
                    case "Telefon":
                        isMatch = exactMatch
                            ? pacient.Telefon.Equals(searchText, comparison)
                            : pacient.Telefon.Contains(searchText, comparison);
                        break;
                    case "Email":
                        isMatch = exactMatch
                            ? pacient.Email.Equals(searchText, comparison)
                            : pacient.Email.Contains(searchText, comparison);
                        break;
                }

                if (isMatch)
                    results.Add(pacient);
            }

            return results.ToArray();
        }

    }
}
