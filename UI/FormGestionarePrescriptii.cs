using System;
using System.IO;
using System.Windows.Forms;
using LibrarieModele;
using NivelStocareDate;
using System.Drawing;
using System.Text.RegularExpressions;
using MetroFramework.Forms;
using MetroFramework.Controls;
using MetroFramework;
using GestionareSpital;
using System.Collections.Generic;

namespace UI
{
    public class ValidatorPrescriptie
    {
        public static class Constante
        {
            public const int MINIM_CARACTERE_MEDICAMENTE = 10;
            public const int MAXIM_CARACTERE_MEDICAMENTE = 500;
            public const int MINIM_CARACTERE_DIAGNOSTIC = 10;
            public const int MAXIM_CARACTERE_DIAGNOSTIC = 500;
            public const int MINIM_CARACTERE_INDICATII = 10;
            public const int MAXIM_CARACTERE_INDICATII = 500;
            public const string PATTERN_MEDICAMENTE = @"^([A-Za-z0-9\s]+ [0-9]+[a-zA-Z]+)(;[A-Za-z0-9\s]+ [0-9]+[a-zA-Z]+)*$";
            public const string PATTERN_DIAGNOSTIC = @"^[A-Za-z0-9\s\.,;:!?-]+$";
        }

        public static (bool Valid, string MesajEroare) ValideazaLista(string medicamente)
        {
            if (string.IsNullOrWhiteSpace(medicamente))
            {
                return (false, "Lista de medicamente este obligatorie!");
            }
            else if (medicamente.Length < Constante.MINIM_CARACTERE_MEDICAMENTE)
            {
                return (false, $"Lista de medicamente trebuie sa contina minim {Constante.MINIM_CARACTERE_MEDICAMENTE} caractere!");
            }
            else if (medicamente.Length > Constante.MAXIM_CARACTERE_MEDICAMENTE)
            {
                return (false, $"Lista de medicamente nu poate depasi {Constante.MAXIM_CARACTERE_MEDICAMENTE} caractere!");
            }
            else if (!Regex.IsMatch(medicamente, Constante.PATTERN_MEDICAMENTE))
            {
                return (false, "Formatul medicamentelor este invalid! Exemplu corect: Paracetamol 500mg; Ibuprofen 400mg");
            }

            return (true, string.Empty);
        }

        public static (bool Valid, string MesajEroare) ValideazaDiagnostic(string diagnostic)
        {
            if (string.IsNullOrWhiteSpace(diagnostic))
            {
                return (false, "Diagnosticul este obligatoriu!");
            }
            else if (diagnostic.Length < Constante.MINIM_CARACTERE_DIAGNOSTIC)
            {
                return (false, $"Diagnosticul trebuie sa contina minim {Constante.MINIM_CARACTERE_DIAGNOSTIC} caractere!");
            }
            else if (diagnostic.Length > Constante.MAXIM_CARACTERE_DIAGNOSTIC)
            {
                return (false, $"Diagnosticul nu poate depasi {Constante.MAXIM_CARACTERE_DIAGNOSTIC} caractere!");
            }
            else if (!Regex.IsMatch(diagnostic, Constante.PATTERN_DIAGNOSTIC))
            {
                return (false, "Formatul diagnosticului este invalid!");
            }

            return (true, string.Empty);
        }

        public static (bool Valid, string MesajEroare) ValideazaIndicatii(string indicatii)
        {
            if (string.IsNullOrWhiteSpace(indicatii))
            {
                return (true, string.Empty);
            }
            else if (indicatii.Length < Constante.MINIM_CARACTERE_INDICATII)
            {
                return (false, $"Indicatiile trebuie sa contina minim {Constante.MINIM_CARACTERE_INDICATII} caractere!");
            }
            else if (indicatii.Length > Constante.MAXIM_CARACTERE_INDICATII)
            {
                return (false, $"Indicatiile nu pot depasi {Constante.MAXIM_CARACTERE_INDICATII} caractere!");
            }

            return (true, string.Empty);
        }

        public static (bool Valid, string MesajEroare) ValideazaDataEmitere(DateTime dataEmitere)
        {
            if (dataEmitere.Date > DateTime.Now.Date)
            {
                return (false, "Data emiterii nu poate fi in viitor!");
            }

            return (true, string.Empty);
        }
    }

    public partial class FormGestionarePrescriptii : MetroForm
    {
        private AdministrarePrescriptii_FisierText adminPrescriptii;
        private AdministrarePacienti_FisierText adminPacienti;
        private AdministrareMedici_FisierText adminMedici;

        private User utilizatorCurent;

        private MetroPanel panelMeniu;
        private MetroButton btnAdaugarePrescriptie;
        private MetroButton btnVizualizarePrescriptii;
        private MetroButton btnVizualizarePrescriptiiPacient;
        private MetroButton btnVizualizarePrescriptiiMedic;
        private MetroButton btnVizualizarePrescriptiiData;
        private MetroButton btnCautarePrescriptii;
        private MetroButton btnInchide;

        public FormGestionarePrescriptii(
            AdministrarePrescriptii_FisierText prescriptiiAdmin,
            AdministrarePacienti_FisierText pacientiAdmin,
            AdministrareMedici_FisierText mediciAdmin,
            User utilizatorCurent)
        {
            this.utilizatorCurent = utilizatorCurent;
            adminPrescriptii = prescriptiiAdmin;
            adminPacienti = pacientiAdmin;
            adminMedici = mediciAdmin;

            InitializeazaComponente();
        }

        private void InitializeazaComponente()
        {
            this.Text = "Gestionare Prescriptii";
            this.Size = new Size(800, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Style = MetroColorStyle.Black;
            this.Theme = MetroThemeStyle.Light;
            this.ShadowType = MetroFormShadowType.AeroShadow;

            InitializeazaPanelMeniu();
            InitializeazaButoane();
            SetarePozitiiButoane();
            AtaseazaEvenimenteButoane();
            AdaugaControaleInPanel();

            this.Controls.Add(panelMeniu);
        }

        private void InitializeazaPanelMeniu()
        {
            panelMeniu = new MetroPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10),
            };
        }

        private void InitializeazaButoane()
        {
            btnAdaugarePrescriptie = new MetroButton { Text = "Adaugare prescriptie noua", Size = new Size(250, 40) };
            btnVizualizarePrescriptii = new MetroButton { Text = "Vizualizare toate prescriptiile", Size = new Size(250, 40) };
            btnVizualizarePrescriptiiPacient = new MetroButton { Text = "Prescriptii pacient", Size = new Size(250, 40) };
            btnVizualizarePrescriptiiMedic = new MetroButton { Text = "Prescriptii emise de medic", Size = new Size(250, 40) };
            btnVizualizarePrescriptiiData = new MetroButton { Text = "Prescriptii dupa data", Size = new Size(250, 40) };
            btnCautarePrescriptii = new MetroButton { Text = "Cautare prescriptii", Size = new Size(250, 40) };

            btnInchide = new MetroButton { Text = "Inchide", Size = new Size(250, 40) };
        }

        private void SetarePozitiiButoane()
        {
            btnAdaugarePrescriptie.Location = new Point(75, 80);
            btnVizualizarePrescriptii.Location = new Point(75, 130);
            btnVizualizarePrescriptiiPacient.Location = new Point(75, 180);
            btnVizualizarePrescriptiiMedic.Location = new Point(75, 230);
            btnVizualizarePrescriptiiData.Location = new Point(75, 280);
            btnCautarePrescriptii.Location = new Point(75, 330);

            btnInchide.Location = new Point(75, 330);
        }

        private void AtaseazaEvenimenteButoane()
        {
            btnAdaugarePrescriptie.Click += BtnAdaugarePrescriptie_Click;
            btnVizualizarePrescriptii.Click += BtnVizualizarePrescriptii_Click;
            btnVizualizarePrescriptiiPacient.Click += BtnVizualizarePrescriptiiPacient_Click;
            btnVizualizarePrescriptiiMedic.Click += BtnVizualizarePrescriptiiMedic_Click;
            btnVizualizarePrescriptiiData.Click += BtnVizualizarePrescriptiiData_Click;
            btnCautarePrescriptii.Click += BtnCautarePrescriptii_Click;

            btnInchide.Click += (s, e) => this.Close();
        }

        private void AdaugaControaleInPanel()
        {
            panelMeniu.Controls.Add(btnAdaugarePrescriptie);
            panelMeniu.Controls.Add(btnVizualizarePrescriptii);
            panelMeniu.Controls.Add(btnVizualizarePrescriptiiPacient);
            panelMeniu.Controls.Add(btnVizualizarePrescriptiiMedic);
            panelMeniu.Controls.Add(btnVizualizarePrescriptiiData);
            panelMeniu.Controls.Add(btnCautarePrescriptii);

            panelMeniu.Controls.Add(btnInchide);
        }

        private void BtnAdaugarePrescriptie_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugarePrescriptii))
            {
                string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                string caleCompletaFisierPrescriptii = Path.Combine(locatieFisierSolutie, "Prescriptii.txt");

                Pacient[] pacienti = adminPacienti.GetPacienti(out int nrPacienti);
                Medic[] medici = adminMedici.GetMedici(out int nrMedici);

                if (nrPacienti == 0 || nrMedici == 0)
                {
                    MessageBox.Show(this, "Nu exista suficienti pacienti sau medici pentru a crea o prescriptie!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var pacientSelectat = AfiseazaSelectiePacient(pacienti);
                if (pacientSelectat == null) return;

                var medicSelectat = AfiseazaSelectieMedic(medici);
                if (medicSelectat == null) return;

                AfiseazaFormularPrescriptie(pacientSelectat, medicSelectat, caleCompletaFisierPrescriptii);
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a adauga prescriptii");
            }
        }

        private Pacient AfiseazaSelectiePacient(Pacient[] pacienti)
        {


            using (MetroForm formSelectiePacient = CreateBaseForm("Selecteaza Pacient", 800, 400))
            {
                ListView listPacienti = CreateListViewPacienti();

                foreach (var pacient in pacienti)
                {
                    ListViewItem item = new ListViewItem(pacient.IdPacient.ToString());
                    item.SubItems.Add(pacient.Nume);
                    item.SubItems.Add(pacient.Prenume);
                    listPacienti.Items.Add(item);
                }

                MetroButton btnSelecteazaPacient = CreateSelectButton();

                Pacient pacientSelectat = null;
                btnSelecteazaPacient.Click += (s, ev) =>
                {
                    if (listPacienti.SelectedItems.Count > 0)
                    {
                        int idPacientSelectat = int.Parse(listPacienti.SelectedItems[0].Text);
                        pacientSelectat = adminPacienti.GetPacientDupaId(idPacientSelectat);
                        formSelectiePacient.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show(formSelectiePacient, "Va rugam selectati un pacient!", "Atentie",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                formSelectiePacient.Controls.Add(listPacienti);
                formSelectiePacient.Controls.Add(btnSelecteazaPacient);

                if (formSelectiePacient.ShowDialog() != DialogResult.OK)
                    return null;

                return pacientSelectat;
            }
        }

        private Medic AfiseazaSelectieMedic(Medic[] medici)
        {
            using (MetroForm formSelectieMedic = CreateBaseForm("Selecteaza Medic", 800, 400))
            {
                ListView listMedici = CreateListViewMedici();

                foreach (var medic in medici)
                {
                    ListViewItem item = new ListViewItem(medic.IdUser.ToString());
                    item.SubItems.Add(medic.Nume);
                    item.SubItems.Add(medic.Prenume);
                    listMedici.Items.Add(item);
                }

                MetroButton btnSelecteazaMedic = CreateSelectButton();

                Medic medicSelectat = null;
                btnSelecteazaMedic.Click += (s, ev) =>
                {
                    if (listMedici.SelectedItems.Count > 0)
                    {
                        int idMedicSelectat = int.Parse(listMedici.SelectedItems[0].Text);
                        medicSelectat = adminMedici.GetMedicDupaId(idMedicSelectat);
                        formSelectieMedic.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show(formSelectieMedic, "Va rugam selectati un medic!", "Atentie",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                formSelectieMedic.Controls.Add(listMedici);
                formSelectieMedic.Controls.Add(btnSelecteazaMedic);

                if (formSelectieMedic.ShowDialog() != DialogResult.OK)
                    return null;

                return medicSelectat;
            }
        }

        private void AfiseazaFormularPrescriptie(Pacient pacientSelectat, Medic medicSelectat, string caleCompletaFisierPrescriptii)
        {
            using (MetroForm formPrescriptie = CreateBaseForm("Prescriptie Noua", 1280, 640))
            {
                formPrescriptie.Resizable = false;

                var (panel, controls) = CreatePrescriptionForm(pacientSelectat, medicSelectat);

                controls.btnSalveazaPrescriptie.Click += (s, ev) =>
                {
                    bool valid = ValidareFormular(controls, out string mesajEroare);

                    if (!valid)
                    {
                        controls.lblMesajEroare.Text = mesajEroare;
                        controls.lblMesajEroare.Visible = true;
                        return;
                    }

                    SalveazaPrescriptie(pacientSelectat, medicSelectat, controls, caleCompletaFisierPrescriptii);
                    formPrescriptie.DialogResult = DialogResult.OK;
                };

                formPrescriptie.Controls.Add(panel);
                formPrescriptie.ShowDialog();
            }
        }

        private MetroForm CreateBaseForm(string titlu, int latime, int inaltime)
        {
            return new MetroForm
            {
                Text = titlu,
                Size = new Size(latime, inaltime),
                StartPosition = FormStartPosition.CenterScreen,
                Style = MetroColorStyle.Black,
                Theme = MetroThemeStyle.Light,
                ShadowType = MetroFormShadowType.AeroShadow
            };
        }

        private ListView CreateListViewPacienti()
        {
            ListView listView = new ListView
            {
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                View = View.Details
            };

            listView.Columns.Add("ID", 50);
            listView.Columns.Add("Nume", 120);
            listView.Columns.Add("Prenume", 120);

            return listView;
        }

        private ListView CreateListViewMedici()
        {
            ListView listView = new ListView
            {
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                View = View.Details
            };

            listView.Columns.Add("ID", 50);
            listView.Columns.Add("Nume", 120);
            listView.Columns.Add("Prenume", 120);

            return listView;
        }

        private MetroButton CreateSelectButton()
        {
            return new MetroButton
            {
                Text = "Selecteaza",
                Dock = DockStyle.Bottom,
            };
        }

        private class PrescriptionFormControls
        {
            public MetroLabel lblTitluFormular;
            public MetroLabel lblPacientInfo;
            public MetroLabel lblMedicInfo;
            public MetroLabel lblMedicamente;
            public MetroLabel lblDiagnostic;
            public MetroLabel lblIndicatii;
            public MetroLabel lblDescriere;
            public MetroLabel lblDataEmitere;
            public MetroLabel lblMesajEroare;

            public TextBox txtMedicamente;
            public TextBox txtDiagnostic;
            public TextBox txtIndicatii;
            public TextBox txtDescriere;
            public DateTimePicker dtpDataEmitere;

            public MetroButton btnSalveazaPrescriptie;

            public Label[] EticheteCampuri;
        }

        private (MetroPanel, PrescriptionFormControls) CreatePrescriptionForm(Pacient pacientSelectat, Medic medicSelectat)
        {
            MetroPanel panelFormular = new MetroPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Name = "panelFormular"
            };

            var controls = new PrescriptionFormControls
            {
                lblTitluFormular = new MetroLabel
                {
                    Text = "Adaugare Prescriptie Noua",
                    Location = new Point(10, 60),
                    Width = 460,
                    FontWeight = MetroLabelWeight.Bold,
                    Name = "lblTitluFormular"
                },

                lblPacientInfo = new MetroLabel
                {
                    Text = $"Pacient: {pacientSelectat.Nume} {pacientSelectat.Prenume}",
                    Location = new Point(10, 90),
                    Width = 460,
                    Name = "lblPacientInfo"
                },

                lblMedicInfo = new MetroLabel
                {
                    Text = $"Medic: Dr. {medicSelectat.Nume} {medicSelectat.Prenume}",
                    Location = new Point(10, 120),
                    Width = 460,
                    Name = "lblMedicInfo"
                },

                lblMedicamente = new MetroLabel
                {
                    Text = "Medicamente *:",
                    Location = new Point(10, 150),
                    Width = 150,
                    Name = "lblMedicamente"
                },

                lblDiagnostic = new MetroLabel
                {
                    Text = "Diagnostic *:",
                    Location = new Point(10, 220),
                    Width = 150,
                    Name = "lblDiagnostic"
                },

                lblIndicatii = new MetroLabel
                {
                    Text = "Indicatii:",
                    Location = new Point(10, 250),
                    Width = 150,
                    Name = "lblIndicatii"
                },

                lblDescriere = new MetroLabel
                {
                    Text = "Descriere:",
                    Location = new Point(10, 320),
                    Width = 150,
                    Name = "lblDescriere"
                },

                lblDataEmitere = new MetroLabel
                {
                    Text = "Data emiterii *:",
                    Location = new Point(10, 390),
                    Width = 150,
                    Name = "lblDataEmitere"
                },

                lblMesajEroare = new MetroLabel
                {
                    Location = new Point(10, 420),
                    Width = 460,
                    Height = 40,
                    ForeColor = Color.Red,
                    Visible = false,
                    Name = "lblMesajEroare"
                },

                txtMedicamente = new TextBox
                {
                    Location = new Point(170, 150),
                    Width = 300,
                    Height = 60,
                    Multiline = true,
                    Name = "txtMedicamente"
                },

                txtDiagnostic = new TextBox
                {
                    Location = new Point(170, 220),
                    Width = 300,
                    Name = "txtDiagnostic"
                },

                txtIndicatii = new TextBox
                {
                    Location = new Point(170, 250),
                    Width = 300,
                    Height = 60,
                    Multiline = true,
                    Name = "txtIndicatii"
                },

                txtDescriere = new TextBox
                {
                    Location = new Point(170, 320),
                    Width = 300,
                    Height = 60,
                    Multiline = true,
                    Name = "txtDescriere"
                },

                dtpDataEmitere = new DateTimePicker
                {
                    Format = DateTimePickerFormat.Short,
                    Location = new Point(170, 390),
                    Width = 300,
                    Name = "dtpDataEmitere"
                },

                btnSalveazaPrescriptie = new MetroButton
                {
                    Text = "Salveaza Prescriptia",
                    Location = new Point(10, 460),
                    Width = 460,
                    Height = 40,
                    Name = "btnSalveazaPrescriptie",
                }
            };

            controls.EticheteCampuri = new Label[]
            {
            controls.lblMedicamente,
            controls.lblDiagnostic,
            controls.lblIndicatii,
            controls.lblDescriere,
            controls.lblDataEmitere
            };

            AtaseazaEvenimenteValidareControale(controls);

            panelFormular.Controls.Add(controls.lblTitluFormular);
            panelFormular.Controls.Add(controls.lblPacientInfo);
            panelFormular.Controls.Add(controls.lblMedicInfo);
            foreach (Label lbl in controls.EticheteCampuri)
            {
                panelFormular.Controls.Add(lbl);
            }
            panelFormular.Controls.Add(controls.txtMedicamente);
            panelFormular.Controls.Add(controls.txtDiagnostic);
            panelFormular.Controls.Add(controls.txtIndicatii);
            panelFormular.Controls.Add(controls.txtDescriere);
            panelFormular.Controls.Add(controls.dtpDataEmitere);
            panelFormular.Controls.Add(controls.lblMesajEroare);
            panelFormular.Controls.Add(controls.btnSalveazaPrescriptie);

            return (panelFormular, controls);
        }

        private void AtaseazaEvenimenteValidareControale(PrescriptionFormControls controls)
        {
            controls.txtMedicamente.Leave += (s, ev) =>
            {
                var (valid, mesajEroare) = ValidatorPrescriptie.ValideazaLista(controls.txtMedicamente.Text);
                ActualizeazaStareControl(controls.lblMedicamente, controls.txtMedicamente, valid, mesajEroare, controls.lblMesajEroare);
            };

            controls.txtDiagnostic.Leave += (s, ev) =>
            {
                var (valid, mesajEroare) = ValidatorPrescriptie.ValideazaDiagnostic(controls.txtDiagnostic.Text);
                ActualizeazaStareControl(controls.lblDiagnostic, controls.txtDiagnostic, valid, mesajEroare, controls.lblMesajEroare);
            };

            controls.txtIndicatii.Leave += (s, ev) =>
            {
                var (valid, mesajEroare) = ValidatorPrescriptie.ValideazaIndicatii(controls.txtIndicatii.Text);
                ActualizeazaStareControl(controls.lblIndicatii, controls.txtIndicatii, valid, mesajEroare, controls.lblMesajEroare);
            };
        }

        private void ActualizeazaStareControl(Label label, TextBox textBox, bool valid, string mesajEroare, Label lblMesajEroare)
        {
            if (!valid)
            {
                label.ForeColor = Color.Red;
                textBox.BackColor = Color.LightPink;
                lblMesajEroare.Text = mesajEroare;
                lblMesajEroare.Visible = true;
            }
            else
            {
                label.ForeColor = SystemColors.ControlText;
                textBox.BackColor = SystemColors.Window;
                lblMesajEroare.Visible = false;
            }
        }

        private bool ValidareFormular(PrescriptionFormControls controls, out string mesajEroare)
        {
            foreach (Label lbl in controls.EticheteCampuri)
            {
                lbl.ForeColor = SystemColors.ControlText;
            }
            controls.txtMedicamente.BackColor = SystemColors.Window;
            controls.txtDiagnostic.BackColor = SystemColors.Window;
            controls.txtIndicatii.BackColor = SystemColors.Window;
            controls.txtDescriere.BackColor = SystemColors.Window;
            controls.lblMesajEroare.Visible = false;

            var (validMedicamente, mesajMedicamente) = ValidatorPrescriptie.ValideazaLista(controls.txtMedicamente.Text);
            if (!validMedicamente)
            {
                controls.lblMedicamente.ForeColor = Color.Red;
                controls.txtMedicamente.BackColor = Color.LightPink;
                mesajEroare = mesajMedicamente;
                return false;
            }

            var (validDiagnostic, mesajDiagnostic) = ValidatorPrescriptie.ValideazaDiagnostic(controls.txtDiagnostic.Text);
            if (!validDiagnostic)
            {
                controls.lblDiagnostic.ForeColor = Color.Red;
                controls.txtDiagnostic.BackColor = Color.LightPink;
                mesajEroare = mesajDiagnostic;
                return false;
            }

            var (validIndicatii, mesajIndicatii) = ValidatorPrescriptie.ValideazaIndicatii(controls.txtIndicatii.Text);
            if (!validIndicatii)
            {
                controls.lblIndicatii.ForeColor = Color.Red;
                controls.txtIndicatii.BackColor = Color.LightPink;
                mesajEroare = mesajIndicatii;
                return false;
            }

            var (validData, mesajData) = ValidatorPrescriptie.ValideazaDataEmitere(controls.dtpDataEmitere.Value);
            if (!validData)
            {
                controls.lblDataEmitere.ForeColor = Color.Red;
                mesajEroare = mesajData;
                return false;
            }

            mesajEroare = string.Empty;
            return true;
        }

        private void SalveazaPrescriptie(Pacient pacientSelectat, Medic medicSelectat, PrescriptionFormControls controls, string caleCompletaFisierPrescriptii)
        {
            try
            {
                Prescriptie[] prescriptiiExistente = adminPrescriptii.GetPrescriptii(out int nrPrescriptii);
                int nuoulId = nrPrescriptii > 0 ? prescriptiiExistente[nrPrescriptii - 1].IdPrescriptie + 1 : 1;

                Prescriptie prescriptieNoua = new Prescriptie
                {
                    IdPrescriptie = nuoulId,
                    IdPacient = pacientSelectat.IdPacient,
                    IdMedic = medicSelectat.IdUser,
                    Medicamente = controls.txtMedicamente.Text.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries),
                    Diagnostic = controls.txtDiagnostic.Text.Trim(),
                    Indicatii = controls.txtIndicatii.Text.Trim(),
                    Descriere = controls.txtDescriere.Text.Trim(),
                    DataEmitere = controls.dtpDataEmitere.Value
                };

                adminPrescriptii.AddPrescriptie(prescriptieNoua, caleCompletaFisierPrescriptii);
                MessageBox.Show($"Prescriptia pentru pacientul '{pacientSelectat.Nume} {pacientSelectat.Prenume}' a fost adaugata cu succes!",
                    "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la adaugarea prescriptiei: {ex.Message}",
                    "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnVizualizarePrescriptii_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePrescriptii))
            {

                Prescriptie[] prescriptii = adminPrescriptii.GetPrescriptii(out int nrPrescriptii);

                using (MetroForm formVizualizare = new MetroForm())
                {
                    formVizualizare.Text = "Toate Prescriptiile";
                    formVizualizare.Size = new Size(800, 400);
                    formVizualizare.StartPosition = FormStartPosition.CenterScreen;
                    formVizualizare.Theme = MetroThemeStyle.Light;
                    formVizualizare.Style = MetroColorStyle.Black;

                    ListView listView = new ListView
                    {
                        Dock = DockStyle.Fill,
                        View = View.Details,
                        FullRowSelect = true,
                        GridLines = true,
                        Font = new Font("Segoe UI", 9F)
                    };

                    listView.Columns.Add("ID", 50);
                    listView.Columns.Add("Pacient", 150);
                    listView.Columns.Add("Medic", 150);
                    listView.Columns.Add("Medicamente", 150);
                    listView.Columns.Add("Diagnostic", 100);
                    listView.Columns.Add("Data", 100);

                    foreach (var prescriptie in prescriptii)
                    {
                        Pacient pacient = adminPacienti.GetPacientDupaId(prescriptie.IdPacient);
                        Medic medic = adminMedici.GetMedicDupaId(prescriptie.IdMedic);

                        ListViewItem item = new ListViewItem(prescriptie.IdPrescriptie.ToString());
                        item.SubItems.Add($"{pacient.IdPacient} - {pacient.Nume} {pacient.Prenume}");
                        item.SubItems.Add($"{medic.IdMedic} - Dr. {medic.Nume} {medic.Prenume}");
                        item.SubItems.Add(string.Join(", ", prescriptie.Medicamente));
                        item.SubItems.Add(prescriptie.Diagnostic);
                        item.SubItems.Add(prescriptie.DataEmitere.ToShortDateString());

                        listView.Items.Add(item);
                    }

                    formVizualizare.Controls.Add(listView);
                    formVizualizare.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a adauga prescriptii.", "Acces Restrictionat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnVizualizarePrescriptiiPacient_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePrescriptii))
            {

                Pacient[] pacienti = adminPacienti.GetPacienti(out int nrPacienti);

                using (MetroForm formSelectiePacient = new MetroForm())
                {
                    formSelectiePacient.Text = "Selecteaza Pacient";
                    formSelectiePacient.Size = new Size(800, 400);
                    formSelectiePacient.StartPosition = FormStartPosition.CenterScreen;
                    formSelectiePacient.Theme = MetroThemeStyle.Light;
                    formSelectiePacient.Style = MetroColorStyle.Black;

                    ListBox listPacienti = new ListBox
                    {
                        Dock = DockStyle.Fill,
                        Font = new Font("Segoe UI", 9F)
                    };

                    foreach (var pacient in pacienti)
                    {
                        listPacienti.Items.Add($"{pacient.IdPacient} - {pacient.Nume} {pacient.Prenume}");
                    }

                    MetroButton btnVizualizeaza = new MetroButton
                    {
                        Text = "Vizualizeaza Prescriptii",
                        Dock = DockStyle.Bottom,
                        BackColor = Color.FromArgb(0, 174, 219),
                        ForeColor = Color.White
                    };

                    btnVizualizeaza.Click += (s, ev) =>
                    {
                        if (listPacienti.SelectedItem != null)
                        {
                            int idPacientSelectat = int.Parse(listPacienti.SelectedItem.ToString().Split('-')[0].Trim());
                            Prescriptie[] prescriptiiPacient = adminPrescriptii.GetPrescriptiiDupaPacient(idPacientSelectat);

                            using (MetroForm formVizualizare = new MetroForm())
                            {
                                formVizualizare.Text = $"Prescriptii Pacient";
                                formVizualizare.Size = new Size(800, 400);
                                formVizualizare.StartPosition = FormStartPosition.CenterScreen;
                                formVizualizare.Theme = MetroThemeStyle.Light;
                                formVizualizare.Style = MetroColorStyle.Black;

                                ListView listView = new ListView
                                {
                                    Dock = DockStyle.Fill,
                                    View = View.Details,
                                    FullRowSelect = true,
                                    GridLines = true,
                                    Font = new Font("Segoe UI", 9F)
                                };

                                listView.Columns.Add("ID", 50);
                                listView.Columns.Add("Medic", 200);
                                listView.Columns.Add("Medicamente", 150);
                                listView.Columns.Add("Diagnostic", 100);
                                listView.Columns.Add("Data", 100);

                                foreach (var prescriptie in prescriptiiPacient)
                                {
                                    Medic medic = adminMedici.GetMedicDupaId(prescriptie.IdMedic);

                                    ListViewItem item = new ListViewItem(prescriptie.IdPrescriptie.ToString());
                                    item.SubItems.Add($"Dr. {medic.Nume} {medic.Prenume}");
                                    item.SubItems.Add(string.Join(", ", prescriptie.Medicamente));
                                    item.SubItems.Add(prescriptie.Diagnostic);
                                    item.SubItems.Add(prescriptie.DataEmitere.ToShortDateString());

                                    listView.Items.Add(item);
                                }

                                formVizualizare.Controls.Add(listView);
                                formVizualizare.ShowDialog();
                            }

                            formSelectiePacient.DialogResult = DialogResult.OK;
                        }
                    };

                    formSelectiePacient.Controls.Add(listPacienti);
                    formSelectiePacient.Controls.Add(btnVizualizeaza);

                    formSelectiePacient.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a vizualiza prescriptii.");
            }
        }

        private void BtnVizualizarePrescriptiiMedic_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePrescriptii))
            {

                Medic[] medici = adminMedici.GetMedici(out int nrMedici);

                using (MetroForm formSelectieMedic = new MetroForm())
                {
                    formSelectieMedic.Text = "Selecteaza Medic";
                    formSelectieMedic.Size = new Size(800, 400);
                    formSelectieMedic.StartPosition = FormStartPosition.CenterScreen;
                    formSelectieMedic.Theme = MetroThemeStyle.Light;
                    formSelectieMedic.Style = MetroColorStyle.Black;

                    ListBox listMedici = new ListBox
                    {
                        Dock = DockStyle.Fill,
                        Font = new Font("Segoe UI", 9F)
                    };

                    foreach (var medic in medici)
                    {
                        listMedici.Items.Add($"{medic.IdUser} - Dr. {medic.Nume} {medic.Prenume}");
                    }

                    MetroButton btnVizualizeaza = new MetroButton
                    {
                        Text = "Vizualizeaza Prescriptii",
                        Dock = DockStyle.Bottom,
                        BackColor = Color.FromArgb(0, 174, 219),
                        ForeColor = Color.White
                    };

                    btnVizualizeaza.Click += (s, ev) =>
                    {
                        if (listMedici.SelectedItem != null)
                        {
                            int idMedicSelectat = int.Parse(listMedici.SelectedItem.ToString().Split('-')[0].Trim());
                            Prescriptie[] prescriptiiMedic = adminPrescriptii.GetPrescriptiiDupaMedic(idMedicSelectat);

                            using (MetroForm formVizualizare = new MetroForm())
                            {
                                formVizualizare.Text = $"Prescriptii Medic";
                                formVizualizare.Size = new Size(800, 400);
                                formVizualizare.StartPosition = FormStartPosition.CenterScreen;
                                formVizualizare.Theme = MetroThemeStyle.Light;
                                formVizualizare.Style = MetroColorStyle.Black;

                                ListView listView = new ListView
                                {
                                    Dock = DockStyle.Fill,
                                    View = View.Details,
                                    FullRowSelect = true,
                                    GridLines = true,
                                    Font = new Font("Segoe UI", 9F)
                                };

                                listView.Columns.Add("ID", 50);
                                listView.Columns.Add("Pacient", 200);
                                listView.Columns.Add("Medicamente", 150);
                                listView.Columns.Add("Diagnostic", 100);
                                listView.Columns.Add("Data", 100);

                                foreach (var prescriptie in prescriptiiMedic)
                                {
                                    Pacient pacient = adminPacienti.GetPacientDupaId(prescriptie.IdPacient);

                                    ListViewItem item = new ListViewItem(prescriptie.IdPrescriptie.ToString());
                                    item.SubItems.Add($"{pacient.Nume} {pacient.Prenume}");
                                    item.SubItems.Add(string.Join(", ", prescriptie.Medicamente));
                                    item.SubItems.Add(prescriptie.Diagnostic);
                                    item.SubItems.Add(prescriptie.DataEmitere.ToShortDateString());

                                    listView.Items.Add(item);
                                }

                                formVizualizare.Controls.Add(listView);
                                formVizualizare.ShowDialog();
                            }

                            formSelectieMedic.DialogResult = DialogResult.OK;
                        }
                    };

                    formSelectieMedic.Controls.Add(listMedici);
                    formSelectieMedic.Controls.Add(btnVizualizeaza);

                    formSelectieMedic.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a vizualiza prescriptii.");
            }
        }

        private void BtnVizualizarePrescriptiiData_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePrescriptii))
            {
                using (MetroForm formSelectieData = new MetroForm())
                {
                    formSelectieData.Text = "Selecteaza Data";
                    formSelectieData.Size = new Size(800, 400);
                    formSelectieData.StartPosition = FormStartPosition.CenterScreen;
                    formSelectieData.Theme = MetroThemeStyle.Light;
                    formSelectieData.Style = MetroColorStyle.Black;

                    Panel panelDate = new Panel
                    {
                        Dock = DockStyle.Fill,
                        Padding = new Padding(20)
                    };

                    Label lblTitle = new Label
                    {
                        Text = "Selectati data pentru care doriti sa vedeti prescriptiile:",
                        Dock = DockStyle.Top,
                        Font = new Font("Segoe UI", 9F),
                        Padding = new Padding(0, 10, 0, 10)
                    };

                    DateTimePicker dtpData = new DateTimePicker
                    {
                        Format = DateTimePickerFormat.Short,
                        Dock = DockStyle.Top,
                        Font = new Font("Segoe UI", 9F)
                    };

                    MetroButton btnVizualizeaza = new MetroButton
                    {
                        Text = "Vizualizeaza Prescriptii",
                        Dock = DockStyle.Bottom,
                        BackColor = Color.FromArgb(0, 174, 219),
                        ForeColor = Color.White
                    };

                    btnVizualizeaza.Click += (s, ev) =>
                    {
                        Prescriptie[] prescriptiiData = adminPrescriptii.GetPrescriptiiDupaData(dtpData.Value);

                        using (MetroForm formVizualizare = new MetroForm())
                        {
                            formVizualizare.Text = $"Prescriptii din {dtpData.Value.ToShortDateString()}";
                            formVizualizare.Size = new Size(800, 400);
                            formVizualizare.StartPosition = FormStartPosition.CenterScreen;
                            formVizualizare.Theme = MetroThemeStyle.Light;
                            formVizualizare.Style = MetroColorStyle.Black;

                            ListView listView = new ListView
                            {
                                Dock = DockStyle.Fill,
                                View = View.Details,
                                FullRowSelect = true,
                                GridLines = true,
                                Font = new Font("Segoe UI", 9F)
                            };

                            listView.Columns.Add("ID", 50);
                            listView.Columns.Add("Pacient", 150);
                            listView.Columns.Add("Medic", 150);
                            listView.Columns.Add("Medicamente", 150);
                            listView.Columns.Add("Diagnostic", 100);
                            listView.Columns.Add("Data", 100);

                            foreach (var prescriptie in prescriptiiData)
                            {
                                Pacient pacient = adminPacienti.GetPacientDupaId(prescriptie.IdPacient);
                                Medic medic = adminMedici.GetMedicDupaId(prescriptie.IdMedic);

                                ListViewItem item = new ListViewItem(prescriptie.IdPrescriptie.ToString());
                                item.SubItems.Add($"{pacient.Nume} {pacient.Prenume}");
                                item.SubItems.Add($"Dr. {medic.Nume} {medic.Prenume}");
                                item.SubItems.Add(string.Join(", ", prescriptie.Medicamente));
                                item.SubItems.Add(prescriptie.Diagnostic);
                                item.SubItems.Add(prescriptie.DataEmitere.ToShortDateString());

                                listView.Items.Add(item);
                            }

                            formVizualizare.Controls.Add(listView);
                            formVizualizare.ShowDialog();
                        }

                        formSelectieData.DialogResult = DialogResult.OK;
                    };

                    panelDate.Controls.Add(lblTitle);
                    panelDate.Controls.Add(dtpData);
                    formSelectieData.Controls.Add(panelDate);
                    formSelectieData.Controls.Add(btnVizualizeaza);

                    formSelectieData.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a vizualiza prescriptii.");

            }
        }

        private void BtnCautarePrescriptii_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePrescriptii))
            {
                using (MetroForm formCautare = new MetroForm())
                {
                    formCautare.Text = "Cautare Prescriptii";
                    formCautare.Size = new Size(600, 500);
                    formCautare.StartPosition = FormStartPosition.CenterScreen;
                    formCautare.Theme = MetroThemeStyle.Light;
                    formCautare.Style = MetroColorStyle.Black;

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
                        Text = "Criteriu de cautare",
                        Location = new Point(270, 40),
                        Size = new Size(300, 150)
                    };

                    RadioButton radioIdPrescriptie = new RadioButton { Text = "ID Prescriptie", Location = new Point(10, 20), Checked = true };
                    RadioButton radioPacient = new RadioButton { Text = "Pacient", Location = new Point(10, 45) };
                    RadioButton radioMedic = new RadioButton { Text = "Medic", Location = new Point(10, 70) };
                    RadioButton radioMedicamente = new RadioButton { Text = "Medicamente", Location = new Point(150, 20) };
                    RadioButton radioDiagnostic = new RadioButton { Text = "Diagnostic", Location = new Point(150, 45) };
                    RadioButton radioData = new RadioButton { Text = "Data emiterii", Location = new Point(150, 70) };
                    RadioButton radioIndicatii = new RadioButton { Text = "Indicatii", Location = new Point(10, 95) };
                    RadioButton radioDescriere = new RadioButton { Text = "Descriere", Location = new Point(150, 95) };

                    groupCampuri.Controls.Add(radioIdPrescriptie);
                    groupCampuri.Controls.Add(radioPacient);
                    groupCampuri.Controls.Add(radioMedic);
                    groupCampuri.Controls.Add(radioMedicamente);
                    groupCampuri.Controls.Add(radioDiagnostic);
                    groupCampuri.Controls.Add(radioData);
                    groupCampuri.Controls.Add(radioIndicatii);
                    groupCampuri.Controls.Add(radioDescriere);
                    panelOptiuni.Controls.Add(groupCampuri);

                    MetroLabel lblTermenCautare = new MetroLabel
                    {
                        Text = "Termen de cautare:",
                        Location = new Point(10, 270),
                        Size = new Size(150, 30)
                    };
                    formCautare.Controls.Add(lblTermenCautare);

                    MetroTextBox txtTermenCautare = new MetroTextBox
                    {
                        Location = new Point(160, 270),
                        Size = new Size(250, 30),
                    };
                    formCautare.Controls.Add(txtTermenCautare);

                    MetroLabel lblData = new MetroLabel
                    {
                        Text = "Data emiterii:",
                        Location = new Point(10, 245),
                        Size = new Size(120, 30),
                        Visible = false
                    };
                    formCautare.Controls.Add(lblData);

                    DateTimePicker dtpData = new DateTimePicker
                    {
                        Format = DateTimePickerFormat.Short,
                        Location = new Point(130, 245),
                        Size = new Size(250, 30),
                        Visible = false
                    };
                    formCautare.Controls.Add(dtpData);

                    radioData.CheckedChanged += (s, ev) =>
                    {
                        lblData.Visible = radioData.Checked;
                        dtpData.Visible = radioData.Checked;
                        txtTermenCautare.Visible = !radioData.Checked;
                        lblTermenCautare.Visible = !radioData.Checked;
                    };

                    MetroLabel lblRezultate = new MetroLabel
                    {
                        Text = "Rezultatele cautarii:",
                        Location = new Point(10, 280),
                        Size = new Size(150, 30)
                    };
                    formCautare.Controls.Add(lblRezultate);

                    ListView listViewRezultate = new ListView
                    {
                        Location = new Point(10, 310),
                        Size = new Size(570, 130),
                        View = View.Details,
                        FullRowSelect = true,
                        GridLines = true
                    };

                    listViewRezultate.Columns.Add("ID", 50);
                    listViewRezultate.Columns.Add("Pacient", 120);
                    listViewRezultate.Columns.Add("Medic", 120);
                    listViewRezultate.Columns.Add("Diagnostic", 100);
                    listViewRezultate.Columns.Add("Data", 80);
                    listViewRezultate.Columns.Add("Medicamente", 100);
                    formCautare.Controls.Add(listViewRezultate);

                    MetroButton btnCauta = new MetroButton
                    {
                        Text = "Cauta",
                        Location = new Point(390, 270),
                        Size = new Size(100, 30)
                    };
                    formCautare.Controls.Add(btnCauta);

                    btnCauta.Click += (s, ev) =>
                    {
                        if (!radioData.Checked && string.IsNullOrWhiteSpace(txtTermenCautare.Text))
                        {
                            MessageBox.Show("Introduceti un termen de cautare!", "Atentie",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        CautaPrescriptii(
                            listViewRezultate,
                            txtTermenCautare.Text,
                            dtpData.Value,
                            radioPartial.Checked,
                            radioIdPrescriptie.Checked,
                            radioPacient.Checked,
                            radioMedic.Checked,
                            radioMedicamente.Checked,
                            radioDiagnostic.Checked,
                            radioData.Checked,
                            radioIndicatii.Checked,
                            radioDescriere.Checked
                        );
                    };

                    listViewRezultate.DoubleClick += (s, ev) =>
                    {
                        if (listViewRezultate.SelectedItems.Count > 0)
                        {
                            int idPrescriptie = int.Parse(listViewRezultate.SelectedItems[0].Text);
                            AfiseazaDetaliiPrescriptie(idPrescriptie);
                        }
                    };

                    formCautare.Controls.Add(panelOptiuni);
                    formCautare.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a cauta prescriptii.",
                    "Acces restrictionat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CautaPrescriptii(
            ListView listViewRezultate,
            string termenCautare,
            DateTime dataCautare,
            bool cautarePartiala,
            bool cautaDupaId,
            bool cautaDupaPacient,
            bool cautaDupaMedic,
            bool cautaDupaMedicamente,
            bool cautaDupaDiagnostic,
            bool cautaDupaData,
            bool cautaDupaIndicatii,
            bool cautaDupaDescriere)
        {
            listViewRezultate.Items.Clear();
            Prescriptie[] toatePrescriptiile = adminPrescriptii.GetPrescriptii(out int nrPrescriptii);
            List<Prescriptie> rezultate = new List<Prescriptie>();

            string termLower = termenCautare.ToLower();

            foreach (var prescriptie in toatePrescriptiile)
            {
                bool potrivire = false;

                if (cautaDupaId)
                {
                    if (prescriptie.IdPrescriptie.ToString().Contains(termenCautare))
                    {
                        potrivire = true;
                    }
                }
                else if (cautaDupaPacient)
                {
                    Pacient pacient = adminPacienti.GetPacientDupaId(prescriptie.IdPacient);
                    string numePacient = $"{pacient.Nume} {pacient.Prenume}".ToLower();

                    if (cautarePartiala && numePacient.Contains(termLower) ||
                        !cautarePartiala && numePacient.Equals(termLower))
                    {
                        potrivire = true;
                    }
                }
                else if (cautaDupaMedic)
                {
                    Medic medic = adminMedici.GetMedicDupaId(prescriptie.IdMedic);
                    string numeMedic = $"{medic.Nume} {medic.Prenume}".ToLower();

                    if (cautarePartiala && numeMedic.Contains(termLower) ||
                        !cautarePartiala && numeMedic.Equals(termLower))
                    {
                        potrivire = true;
                    }
                }
                else if (cautaDupaMedicamente)
                {
                    string medicamenteString = string.Join(" ", prescriptie.Medicamente).ToLower();

                    if (cautarePartiala && medicamenteString.Contains(termLower) ||
                        !cautarePartiala && medicamenteString.Equals(termLower))
                    {
                        potrivire = true;
                    }
                }
                else if (cautaDupaDiagnostic)
                {
                    string diagnosticLower = prescriptie.Diagnostic.ToLower();

                    if (cautarePartiala && diagnosticLower.Contains(termLower) ||
                        !cautarePartiala && diagnosticLower.Equals(termLower))
                    {
                        potrivire = true;
                    }
                }
                else if (cautaDupaData)
                {
                    if (prescriptie.DataEmitere.Date == dataCautare.Date)
                    {
                        potrivire = true;
                    }
                }
                else if (cautaDupaIndicatii && !string.IsNullOrEmpty(prescriptie.Indicatii))
                {
                    string indicatiiLower = prescriptie.Indicatii.ToLower();

                    if (cautarePartiala && indicatiiLower.Contains(termLower) ||
                        !cautarePartiala && indicatiiLower.Equals(termLower))
                    {
                        potrivire = true;
                    }
                }
                else if (cautaDupaDescriere && !string.IsNullOrEmpty(prescriptie.Descriere))
                {
                    string descriereLower = prescriptie.Descriere.ToLower();

                    if (cautarePartiala && descriereLower.Contains(termLower) ||
                        !cautarePartiala && descriereLower.Equals(termLower))
                    {
                        potrivire = true;
                    }
                }

                if (potrivire)
                {
                    rezultate.Add(prescriptie);
                }
            }

            foreach (var prescriptie in rezultate)
            {
                Pacient pacient = adminPacienti.GetPacientDupaId(prescriptie.IdPacient);
                Medic medic = adminMedici.GetMedicDupaId(prescriptie.IdMedic);

                ListViewItem item = new ListViewItem(prescriptie.IdPrescriptie.ToString());
                item.SubItems.Add($"{pacient.Nume} {pacient.Prenume}");
                item.SubItems.Add($"Dr. {medic.Nume} {medic.Prenume}");
                item.SubItems.Add(prescriptie.Diagnostic);
                item.SubItems.Add(prescriptie.DataEmitere.ToShortDateString());
                item.SubItems.Add(string.Join(", ", prescriptie.Medicamente));

                listViewRezultate.Items.Add(item);
            }

            if (rezultate.Count == 0)
            {
                MessageBox.Show("Nu s-au gasit prescriptii care sa corespunda criteriului selectat.",
                    "Rezultat cautare", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AfiseazaDetaliiPrescriptie(int idPrescriptie)
        {
            Prescriptie prescriptie = GetPrescriptieDupaId(idPrescriptie);
            if (prescriptie == null)
            {
                MessageBox.Show("Nu s-a gasit prescriptia selectata!", "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Pacient pacient = adminPacienti.GetPacientDupaId(prescriptie.IdPacient);
            Medic medic = adminMedici.GetMedicDupaId(prescriptie.IdMedic);

            using (MetroForm formDetalii = new MetroForm())
            {
                formDetalii.Text = $"Detalii Prescriptie #{prescriptie.IdPrescriptie}";
                formDetalii.Size = new Size(500, 500);
                formDetalii.StartPosition = FormStartPosition.CenterScreen;
                formDetalii.Theme = MetroThemeStyle.Light;
                formDetalii.Style = MetroColorStyle.Black;
                formDetalii.Padding = new Padding(20);

                TableLayoutPanel tableLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 8,
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
                };

                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

                AddRowToTable(tableLayout, 0, "ID Prescriptie:", prescriptie.IdPrescriptie.ToString());
                AddRowToTable(tableLayout, 1, "Pacient:", $"{pacient.Nume} {pacient.Prenume}");
                AddRowToTable(tableLayout, 2, "Medic:", $"Dr. {medic.Nume} {medic.Prenume}");
                AddRowToTable(tableLayout, 3, "Data emiterii:", prescriptie.DataEmitere.ToShortDateString());
                AddRowToTable(tableLayout, 4, "Diagnostic:", prescriptie.Diagnostic);
                AddRowToTable(tableLayout, 5, "Medicamente:", string.Join(Environment.NewLine, prescriptie.Medicamente));
                AddRowToTable(tableLayout, 6, "Indicatii:", prescriptie.Indicatii ?? "N/A");
                AddRowToTable(tableLayout, 7, "Descriere:", prescriptie.Descriere ?? "N/A");

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

        private Prescriptie GetPrescriptieDupaId(int idPrescriptie)
        {
            Prescriptie[] prescriptii = adminPrescriptii.GetPrescriptii(out int nrPrescriptii);
            foreach (var prescriptie in prescriptii)
            {
                if (prescriptie.IdPrescriptie == idPrescriptie)
                    return prescriptie;
            }
            return null;
        }

    }
}