using System;
using System.IO;
using System.Windows.Forms;
using LibrarieModele;
using NivelStocareDate;
using System.Drawing;
using System.Text.RegularExpressions;
namespace UI
{
    public partial class FormGestionarePrescriptii : Form
    {
        private AdministrarePrescriptii_FisierText adminPrescriptii;
        private AdministrarePacienti_FisierText adminPacienti;
        private AdministrareMedici_FisierText adminMedici;

        public FormGestionarePrescriptii(
            AdministrarePrescriptii_FisierText prescriptiiAdmin,
            AdministrarePacienti_FisierText pacientiAdmin,
            AdministrareMedici_FisierText mediciAdmin)
        {
            adminPrescriptii = prescriptiiAdmin;
            adminPacienti = pacientiAdmin;
            adminMedici = mediciAdmin;

            ConfigureazaMeniu();
        }

        private void ConfigureazaMeniu()
        {
            this.Text = "Gestionare Prescriptii";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            FlowLayoutPanel panelMeniu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
            };

            Button btnAdaugarePrescriptie = new Button { Text = "Adaugare prescriptie noua", Size = new Size(250, 40) };
            Button btnVizualizarePrescriptii = new Button { Text = "Vizualizare toate prescriptiile", Size = new Size(250, 40) };
            Button btnVizualizarePrescriptiiPacient = new Button { Text = "Prescriptii pacient", Size = new Size(250, 40) };
            Button btnVizualizarePrescriptiiMedic = new Button { Text = "Prescriptii emise de medic", Size = new Size(250, 40) };
            Button btnVizualizarePrescriptiiData = new Button { Text = "Prescriptii dupa data", Size = new Size(250, 40) };
            Button btnInchide = new Button { Text = "Inchide", Size = new Size(250, 40) };

            btnAdaugarePrescriptie.Click += BtnAdaugarePrescriptie_Click;
            btnVizualizarePrescriptii.Click += BtnVizualizarePrescriptii_Click;
            btnVizualizarePrescriptiiPacient.Click += BtnVizualizarePrescriptiiPacient_Click;
            btnVizualizarePrescriptiiMedic.Click += BtnVizualizarePrescriptiiMedic_Click;
            btnVizualizarePrescriptiiData.Click += BtnVizualizarePrescriptiiData_Click;
            btnInchide.Click += (s, e) => this.Close();

            panelMeniu.Controls.Add(btnAdaugarePrescriptie);
            panelMeniu.Controls.Add(btnVizualizarePrescriptii);
            panelMeniu.Controls.Add(btnVizualizarePrescriptiiPacient);
            panelMeniu.Controls.Add(btnVizualizarePrescriptiiMedic);
            panelMeniu.Controls.Add(btnVizualizarePrescriptiiData);
            panelMeniu.Controls.Add(btnInchide);

            this.Controls.Add(panelMeniu);
        }

        public static class ConstanteValidarePrescriptie
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

        private void BtnAdaugarePrescriptie_Click(object sender, EventArgs e)
        {
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisierPrescriptii = Path.Combine(locatieFisierSolutie, "Prescriptii.txt");

            Pacient[] pacienti = adminPacienti.GetPacienti(out int nrPacienti);
            Medic[] medici = adminMedici.GetMedici(out int nrMedici);

            if (nrPacienti == 0 || nrMedici == 0)
            {
                MessageBox.Show("Nu exista suficienti pacienti sau medici pentru a crea o prescriptie!", "Eroare");
                return;
            }

            using (Form formSelectiePacient = new Form())
            {
                formSelectiePacient.Text = "Selecteaza Pacient";
                formSelectiePacient.Size = new Size(300, 400);
                formSelectiePacient.StartPosition = FormStartPosition.CenterScreen;

                ListBox listPacienti = new ListBox
                {
                    Dock = DockStyle.Fill
                };

                foreach (var pacient in pacienti)
                {
                    listPacienti.Items.Add($"{pacient.IdPacient} - {pacient.Nume} {pacient.Prenume}");
                }

                Button btnSelecteazaPacient = new Button
                {
                    Text = "Selecteaza",
                    Dock = DockStyle.Bottom
                };

                Pacient pacientSelectat = null;
                btnSelecteazaPacient.Click += (s, ev) =>
                {
                    if (listPacienti.SelectedItem != null)
                    {
                        int idPacientSelectat = int.Parse(listPacienti.SelectedItem.ToString().Split('-')[0].Trim());
                        pacientSelectat = adminPacienti.GetPacientDupaId(idPacientSelectat);
                        formSelectiePacient.DialogResult = DialogResult.OK;
                    }
                };

                formSelectiePacient.Controls.Add(listPacienti);
                formSelectiePacient.Controls.Add(btnSelecteazaPacient);

                if (formSelectiePacient.ShowDialog() != DialogResult.OK)
                    return;

                using (Form formSelectieMedic = new Form())
                {
                    formSelectieMedic.Text = "Selecteaza Medic";
                    formSelectieMedic.Size = new Size(300, 400);
                    formSelectieMedic.StartPosition = FormStartPosition.CenterScreen;

                    ListBox listMedici = new ListBox
                    {
                        Dock = DockStyle.Fill
                    };

                    foreach (var medic in medici)
                    {
                        listMedici.Items.Add($"{medic.IdUser} - Dr. {medic.Nume} {medic.Prenume}");
                    }

                    Button btnSelecteazaMedic = new Button
                    {
                        Text = "Selecteaza",
                        Dock = DockStyle.Bottom
                    };

                    Medic medicSelectat = null;
                    btnSelecteazaMedic.Click += (s, ev) =>
                    {
                        if (listMedici.SelectedItem != null)
                        {
                            int idMedicSelectat = int.Parse(listMedici.SelectedItem.ToString().Split('-')[0].Trim());
                            medicSelectat = adminMedici.GetMedicDupaId(idMedicSelectat);
                            formSelectieMedic.DialogResult = DialogResult.OK;
                        }
                    };

                    formSelectieMedic.Controls.Add(listMedici);
                    formSelectieMedic.Controls.Add(btnSelecteazaMedic);

                    if (formSelectieMedic.ShowDialog() != DialogResult.OK)
                        return;

                    using (Form formPrescriptie = new Form())
                    {
                        formPrescriptie.Text = "Prescriptie Noua";
                        formPrescriptie.Size = new Size(500, 520);
                        formPrescriptie.StartPosition = FormStartPosition.CenterScreen;
                        formPrescriptie.FormBorderStyle = FormBorderStyle.FixedDialog;
                        formPrescriptie.MaximizeBox = false;
                        formPrescriptie.MinimizeBox = false;

                        Panel panelFormular = new Panel
                        {
                            Dock = DockStyle.Fill,
                            AutoScroll = true,
                            Name = "panelFormular"
                        };

                        Label lblTitluFormular = new Label
                        {
                            Text = "Adaugare Prescriptie Noua",
                            Location = new Point(10, 10),
                            Width = 460,
                            Font = new Font(SystemFonts.DefaultFont.FontFamily, 10, FontStyle.Bold),
                            Name = "lblTitluFormular"
                        };

                        Label lblPacientInfo = new Label
                        {
                            Text = $"Pacient: {pacientSelectat.Nume} {pacientSelectat.Prenume}",
                            Location = new Point(10, 40),
                            Width = 460,
                            Name = "lblPacientInfo"
                        };

                        Label lblMedicInfo = new Label
                        {
                            Text = $"Medic: Dr. {medicSelectat.Nume} {medicSelectat.Prenume}",
                            Location = new Point(10, 60),
                            Width = 460,
                            Name = "lblMedicInfo"
                        };

                        Label lblMedicamente = new Label
                        {
                            Text = "Medicamente *:",
                            Location = new Point(10, 90),
                            Width = 150,
                            Name = "lblMedicamente"
                        };

                        Label lblDiagnostic = new Label
                        {
                            Text = "Diagnostic *:",
                            Location = new Point(10, 160),
                            Width = 150,
                            Name = "lblDiagnostic"
                        };

                        Label lblIndicatii = new Label
                        {
                            Text = "Indicatii:",
                            Location = new Point(10, 190),
                            Width = 150,
                            Name = "lblIndicatii"
                        };

                        Label lblDescriere = new Label
                        {
                            Text = "Descriere:",
                            Location = new Point(10, 260),
                            Width = 150,
                            Name = "lblDescriere"
                        };

                        Label lblDataEmitere = new Label
                        {
                            Text = "Data emiterii *:",
                            Location = new Point(10, 330),
                            Width = 150,
                            Name = "lblDataEmitere"
                        };

                        Label[] eticheteCampuri = new Label[]
                        {
                    lblMedicamente,
                    lblDiagnostic,
                    lblIndicatii,
                    lblDescriere,
                    lblDataEmitere
                        };

                        TextBox txtMedicamente = new TextBox
                        {
                            Location = new Point(170, 90),
                            Width = 300,
                            Height = 60,
                            Multiline = true,
                            Name = "txtMedicamente"
                        };

                        TextBox txtDiagnostic = new TextBox
                        {
                            Location = new Point(170, 160),
                            Width = 300,
                            Name = "txtDiagnostic"
                        };

                        TextBox txtIndicatii = new TextBox
                        {
                            Location = new Point(170, 190),
                            Width = 300,
                            Height = 60,
                            Multiline = true,
                            Name = "txtIndicatii"
                        };

                        TextBox txtDescriere = new TextBox
                        {
                            Location = new Point(170, 260),
                            Width = 300,
                            Height = 60,
                            Multiline = true,
                            Name = "txtDescriere"
                        };

                        DateTimePicker dtpDataEmitere = new DateTimePicker
                        {
                            Format = DateTimePickerFormat.Short,
                            Location = new Point(170, 330),
                            Width = 300,
                            Name = "dtpDataEmitere"
                        };

                        Label lblMesajEroare = new Label
                        {
                            Location = new Point(10, 360),
                            Width = 460,
                            Height = 40,
                            ForeColor = Color.Red,
                            Visible = false,
                            Name = "lblMesajEroare"
                        };

                        txtMedicamente.Leave += (s, ev) =>
                        {
                            if (string.IsNullOrWhiteSpace(txtMedicamente.Text))
                            {
                                lblMedicamente.ForeColor = Color.Red;
                                txtMedicamente.BackColor = Color.LightPink;
                                lblMesajEroare.Text = "Lista de medicamente este obligatorie!";
                                lblMesajEroare.Visible = true;
                            }
                            else if (txtMedicamente.Text.Length < ConstanteValidarePrescriptie.MINIM_CARACTERE_MEDICAMENTE)
                            {
                                lblMedicamente.ForeColor = Color.Red;
                                txtMedicamente.BackColor = Color.LightPink;
                                lblMesajEroare.Text = $"Lista de medicamente trebuie sa contina minim {ConstanteValidarePrescriptie.MINIM_CARACTERE_MEDICAMENTE} caractere!";
                                lblMesajEroare.Visible = true;
                            }
                            else if (txtMedicamente.Text.Length > ConstanteValidarePrescriptie.MAXIM_CARACTERE_MEDICAMENTE)
                            {
                                lblMedicamente.ForeColor = Color.Red;
                                txtMedicamente.BackColor = Color.LightPink;
                                lblMesajEroare.Text = $"Lista de medicamente nu poate depasi {ConstanteValidarePrescriptie.MAXIM_CARACTERE_MEDICAMENTE} caractere!";
                                lblMesajEroare.Visible = true;
                            }
                            else if (!Regex.IsMatch(txtMedicamente.Text, ConstanteValidarePrescriptie.PATTERN_MEDICAMENTE))
                            {
                                lblMedicamente.ForeColor = Color.Red;
                                txtMedicamente.BackColor = Color.LightPink;
                                lblMesajEroare.Text = "Formatul medicamentelor este invalid! Exemplu corect: Paracetamol 500mg; Ibuprofen 400mg";
                                lblMesajEroare.Visible = true;
                            }
                            else
                            {
                                lblMedicamente.ForeColor = SystemColors.ControlText;
                                txtMedicamente.BackColor = SystemColors.Window;
                                lblMesajEroare.Visible = false;
                            }
                        };

                        txtDiagnostic.Leave += (s, ev) =>
                        {
                            if (string.IsNullOrWhiteSpace(txtDiagnostic.Text))
                            {
                                lblDiagnostic.ForeColor = Color.Red;
                                txtDiagnostic.BackColor = Color.LightPink;
                                lblMesajEroare.Text = "Diagnosticul este obligatoriu!";
                                lblMesajEroare.Visible = true;
                            }
                            else if (txtDiagnostic.Text.Length < ConstanteValidarePrescriptie.MINIM_CARACTERE_DIAGNOSTIC)
                            {
                                lblDiagnostic.ForeColor = Color.Red;
                                txtDiagnostic.BackColor = Color.LightPink;
                                lblMesajEroare.Text = $"Diagnosticul trebuie sa contina minim {ConstanteValidarePrescriptie.MINIM_CARACTERE_DIAGNOSTIC} caractere!";
                                lblMesajEroare.Visible = true;
                            }
                            else if (txtDiagnostic.Text.Length > ConstanteValidarePrescriptie.MAXIM_CARACTERE_DIAGNOSTIC)
                            {
                                lblDiagnostic.ForeColor = Color.Red;
                                txtDiagnostic.BackColor = Color.LightPink;
                                lblMesajEroare.Text = $"Diagnosticul nu poate depasi {ConstanteValidarePrescriptie.MAXIM_CARACTERE_DIAGNOSTIC} caractere!";
                                lblMesajEroare.Visible = true;
                            }
                            else if (!Regex.IsMatch(txtDiagnostic.Text, ConstanteValidarePrescriptie.PATTERN_DIAGNOSTIC))
                            {
                                lblDiagnostic.ForeColor = Color.Red;
                                txtDiagnostic.BackColor = Color.LightPink;
                                lblMesajEroare.Text = "Formatul diagnosticului este invalid!";
                                lblMesajEroare.Visible = true;
                            }
                            else
                            {
                                lblDiagnostic.ForeColor = SystemColors.ControlText;
                                txtDiagnostic.BackColor = SystemColors.Window;
                                lblMesajEroare.Visible = false;
                            }
                        };

                        txtIndicatii.Leave += (s, ev) =>
                        {
                            if (!string.IsNullOrWhiteSpace(txtIndicatii.Text))
                            {
                                if (txtIndicatii.Text.Length < ConstanteValidarePrescriptie.MINIM_CARACTERE_INDICATII)
                                {
                                    lblIndicatii.ForeColor = Color.Red;
                                    txtIndicatii.BackColor = Color.LightPink;
                                    lblMesajEroare.Text = $"Indicatiile trebuie sa contina minim {ConstanteValidarePrescriptie.MINIM_CARACTERE_INDICATII} caractere!";
                                    lblMesajEroare.Visible = true;
                                }
                                else if (txtIndicatii.Text.Length > ConstanteValidarePrescriptie.MAXIM_CARACTERE_INDICATII)
                                {
                                    lblIndicatii.ForeColor = Color.Red;
                                    txtIndicatii.BackColor = Color.LightPink;
                                    lblMesajEroare.Text = $"Indicatiile nu pot depasi {ConstanteValidarePrescriptie.MAXIM_CARACTERE_INDICATII} caractere!";
                                    lblMesajEroare.Visible = true;
                                }
                                else
                                {
                                    lblIndicatii.ForeColor = SystemColors.ControlText;
                                    txtIndicatii.BackColor = SystemColors.Window;
                                    lblMesajEroare.Visible = false;
                                }
                            }
                            else
                            {
                                lblIndicatii.ForeColor = SystemColors.ControlText;
                                txtIndicatii.BackColor = SystemColors.Window;
                                lblMesajEroare.Visible = false;
                            }
                        };

                        Button btnSalveazaPrescriptie = new Button
                        {
                            Text = "Salveaza Prescriptia",
                            Location = new Point(10, 410),
                            Width = 460,
                            Height = 40,
                            Name = "btnSalveazaPrescriptie"
                        };

                        btnSalveazaPrescriptie.Click += (s, ev) =>
                        {
                            foreach (Label lbl in eticheteCampuri)
                            {
                                lbl.ForeColor = SystemColors.ControlText;
                            }
                            txtMedicamente.BackColor = SystemColors.Window;
                            txtDiagnostic.BackColor = SystemColors.Window;
                            txtIndicatii.BackColor = SystemColors.Window;
                            txtDescriere.BackColor = SystemColors.Window;
                            lblMesajEroare.Visible = false;

                            bool valid = true;
                            string mesajEroare = "";

                            if (string.IsNullOrWhiteSpace(txtMedicamente.Text))
                            {
                                lblMedicamente.ForeColor = Color.Red;
                                txtMedicamente.BackColor = Color.LightPink;
                                valid = false;
                                mesajEroare = "Lista de medicamente este obligatorie!";
                            }
                            else if (txtMedicamente.Text.Length < ConstanteValidarePrescriptie.MINIM_CARACTERE_MEDICAMENTE)
                            {
                                lblMedicamente.ForeColor = Color.Red;
                                txtMedicamente.BackColor = Color.LightPink;
                                valid = false;
                                mesajEroare = $"Lista de medicamente trebuie sa contina minim {ConstanteValidarePrescriptie.MINIM_CARACTERE_MEDICAMENTE} caractere!";
                            }
                            else if (txtMedicamente.Text.Length > ConstanteValidarePrescriptie.MAXIM_CARACTERE_MEDICAMENTE)
                            {
                                lblMedicamente.ForeColor = Color.Red;
                                txtMedicamente.BackColor = Color.LightPink;
                                valid = false;
                                mesajEroare = $"Lista de medicamente nu poate depasi {ConstanteValidarePrescriptie.MAXIM_CARACTERE_MEDICAMENTE} caractere!";
                            }
                            else if (!Regex.IsMatch(txtMedicamente.Text, ConstanteValidarePrescriptie.PATTERN_MEDICAMENTE))
                            {
                                lblMedicamente.ForeColor = Color.Red;
                                txtMedicamente.BackColor = Color.LightPink;
                                valid = false;
                                mesajEroare = "Formatul medicamentelor este invalid! Exemplu corect: Paracetamol 500mg; Ibuprofen 400mg";
                            }

                            if (string.IsNullOrWhiteSpace(txtDiagnostic.Text))
                            {
                                lblDiagnostic.ForeColor = Color.Red;
                                txtDiagnostic.BackColor = Color.LightPink;
                                valid = false;
                                mesajEroare = "Diagnosticul este obligatoriu!";
                            }
                            else if (txtDiagnostic.Text.Length < ConstanteValidarePrescriptie.MINIM_CARACTERE_DIAGNOSTIC)
                            {
                                lblDiagnostic.ForeColor = Color.Red;
                                txtDiagnostic.BackColor = Color.LightPink;
                                valid = false;
                                mesajEroare = $"Diagnosticul trebuie sa contina minim {ConstanteValidarePrescriptie.MINIM_CARACTERE_DIAGNOSTIC} caractere!";
                            }
                            else if (txtDiagnostic.Text.Length > ConstanteValidarePrescriptie.MAXIM_CARACTERE_DIAGNOSTIC)
                            {
                                lblDiagnostic.ForeColor = Color.Red;
                                txtDiagnostic.BackColor = Color.LightPink;
                                valid = false;
                                mesajEroare = $"Diagnosticul nu poate depasi {ConstanteValidarePrescriptie.MAXIM_CARACTERE_DIAGNOSTIC} caractere!";
                            }
                            else if (!Regex.IsMatch(txtDiagnostic.Text, ConstanteValidarePrescriptie.PATTERN_DIAGNOSTIC))
                            {
                                lblDiagnostic.ForeColor = Color.Red;
                                txtDiagnostic.BackColor = Color.LightPink;
                                valid = false;
                                mesajEroare = "Formatul diagnosticului este invalid!";
                            }

                            if (!string.IsNullOrWhiteSpace(txtIndicatii.Text))
                            {
                                if (txtIndicatii.Text.Length < ConstanteValidarePrescriptie.MINIM_CARACTERE_INDICATII)
                                {
                                    lblIndicatii.ForeColor = Color.Red;
                                    txtIndicatii.BackColor = Color.LightPink;
                                    valid = false;
                                    mesajEroare = $"Indicatiile trebuie sa contina minim {ConstanteValidarePrescriptie.MINIM_CARACTERE_INDICATII} caractere!";
                                }
                                else if (txtIndicatii.Text.Length > ConstanteValidarePrescriptie.MAXIM_CARACTERE_INDICATII)
                                {
                                    lblIndicatii.ForeColor = Color.Red;
                                    txtIndicatii.BackColor = Color.LightPink;
                                    valid = false;
                                    mesajEroare = $"Indicatiile nu pot depasi {ConstanteValidarePrescriptie.MAXIM_CARACTERE_INDICATII} caractere!";
                                }
                            }

                            if (dtpDataEmitere.Value.Date > DateTime.Now.Date)
                            {
                                lblDataEmitere.ForeColor = Color.Red;
                                valid = false;
                                mesajEroare = "Data emiterii nu poate fi in viitor!";
                            }

                            if (!valid)
                            {
                                lblMesajEroare.Text = mesajEroare;
                                lblMesajEroare.Visible = true;
                                return;
                            }

                            try
                            {
                                Prescriptie[] prescriptiiExistente = adminPrescriptii.GetPrescriptii(out int nrPrescriptii);
                                int nuoulId = nrPrescriptii > 0 ? prescriptiiExistente[nrPrescriptii - 1].IdPrescriptie + 1 : 1;

                                Prescriptie prescriptieNoua = new Prescriptie
                                {
                                    IdPrescriptie = nuoulId,
                                    IdPacient = pacientSelectat.IdPacient,
                                    IdMedic = medicSelectat.IdUser,
                                    Medicamente = txtMedicamente.Text.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries),
                                    Diagnostic = txtDiagnostic.Text.Trim(),
                                    Indicatii = txtIndicatii.Text.Trim(),
                                    Descriere = txtDescriere.Text.Trim(),
                                    DataEmitere = dtpDataEmitere.Value
                                };

                                adminPrescriptii.AddPrescriptie(prescriptieNoua, caleCompletaFisierPrescriptii);
                                MessageBox.Show($"Prescriptia pentru pacientul '{pacientSelectat.Nume} {pacientSelectat.Prenume}' a fost adaugata cu succes!",
                                    "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                formPrescriptie.DialogResult = DialogResult.OK;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Eroare la adaugarea prescriptiei: {ex.Message}",
                                    "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };

                        panelFormular.Controls.Add(lblTitluFormular);
                        panelFormular.Controls.Add(lblPacientInfo);
                        panelFormular.Controls.Add(lblMedicInfo);
                        foreach (Label lbl in eticheteCampuri)
                        {
                            panelFormular.Controls.Add(lbl);
                        }
                        panelFormular.Controls.Add(txtMedicamente);
                        panelFormular.Controls.Add(txtDiagnostic);
                        panelFormular.Controls.Add(txtIndicatii);
                        panelFormular.Controls.Add(txtDescriere);
                        panelFormular.Controls.Add(dtpDataEmitere);
                        panelFormular.Controls.Add(lblMesajEroare);
                        panelFormular.Controls.Add(btnSalveazaPrescriptie);

                        formPrescriptie.Controls.Add(panelFormular);
                        formPrescriptie.ShowDialog();
                    }
                }
            }
        }

        private void BtnVizualizarePrescriptii_Click(object sender, EventArgs e)
        {
            Prescriptie[] prescriptii = adminPrescriptii.GetPrescriptii(out int nrPrescriptii);

            using (Form formVizualizare = new Form())
            {
                formVizualizare.Text = "Toate Prescriptiile";
                formVizualizare.Size = new Size(700, 400);
                formVizualizare.StartPosition = FormStartPosition.CenterScreen;

                ListView listView = new ListView
                {
                    Dock = DockStyle.Fill,
                    View = View.Details
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

        private void BtnVizualizarePrescriptiiPacient_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");

        }

        private void BtnVizualizarePrescriptiiMedic_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");

        }

        private void BtnVizualizarePrescriptiiData_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");
        }

    }
}