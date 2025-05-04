using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LibrarieModele;
using NivelStocareDate;
using MetroFramework.Forms;
using MetroFramework.Controls;
using System.Collections.Generic;
using GestionareSpital;

namespace UI
{
    public partial class FormGestionareProgramari : MetroForm
    {
        private AdministrareProgramari_FisierText adminProgramari;
        private AdministrarePacienti_FisierText adminPacienti;
        private AdministrareMedici_FisierText adminMedici;

        private User utilizatorCurent;

        private Label lblMesajEroare;
        private List<Label> eticheteCampuri;

        public FormGestionareProgramari(
            AdministrareProgramari_FisierText programariAdmin,
            AdministrarePacienti_FisierText pacientiAdmin,
            AdministrareMedici_FisierText mediciAdmin,
            User utilizatorCurent)
        {
            this.utilizatorCurent = utilizatorCurent;
            this.adminProgramari = programariAdmin;
            this.adminPacienti = pacientiAdmin;
            this.adminMedici = mediciAdmin;

            ConfigureazaMeniu();

        }



        private void ConfigureazaMeniu()
        {
            this.Text = "Gestionare Programari";
            this.Size = new Size(800, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Style = MetroFramework.MetroColorStyle.Black;
            this.MaximizeBox = false;
            this.Resizable = false;

            MetroPanel panelMeniu = new MetroPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20),
                Style = MetroFramework.MetroColorStyle.Blue,
                Theme = MetroFramework.MetroThemeStyle.Light
            };

            FlowLayoutPanel flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
                Padding = new Padding(10),
            };

            MetroButton CreateMetroButton(string text)
            {
                return new MetroButton
                {
                    Text = text,
                    Width = 250,
                    Height = 40,
                    Theme = MetroFramework.MetroThemeStyle.Light,
                    Style = MetroFramework.MetroColorStyle.Blue
                };
            }

            var btnAdaugareProgramare = CreateMetroButton("Adaugare programare noua");
            var btnVizualizareProgramari = CreateMetroButton("Vizualizare toate programarile");
            var btnVizualizareProgramariPacient = CreateMetroButton("Programari pacient");
            var btnVizualizareProgramariMedic = CreateMetroButton("Programari medic");
            var btnVizualizareProgramariData = CreateMetroButton("Programari dupa data");
            var btnCautareProgramari = CreateMetroButton("Cautare programari");
            var btnInchide = CreateMetroButton("inchide");

            btnAdaugareProgramare.Click += BtnAdaugareProgramare_Click;
            btnVizualizareProgramari.Click += BtnVizualizareProgramari_Click;
            btnVizualizareProgramariPacient.Click += BtnVizualizareProgramariPacient_Click;
            btnVizualizareProgramariMedic.Click += BtnVizualizareProgramariMedic_Click;
            btnVizualizareProgramariData.Click += BtnVizualizareProgramariData_Click;
            btnCautareProgramari.Click += BtnCautareProgramari_Click;
            btnInchide.Click += (s, e) => this.Close();

            flowPanel.Controls.Add(btnAdaugareProgramare);
            flowPanel.Controls.Add(btnVizualizareProgramari);
            flowPanel.Controls.Add(btnVizualizareProgramariPacient);
            flowPanel.Controls.Add(btnVizualizareProgramariMedic);
            flowPanel.Controls.Add(btnVizualizareProgramariData);
            flowPanel.Controls.Add(btnCautareProgramari);
            flowPanel.Controls.Add(btnInchide);

            panelMeniu.Controls.Add(flowPanel);
            this.Controls.Add(panelMeniu);
        }


        public static class ConstanteValidare
        {
            public const int MINIM_CARACTERE_MOTIV = 5;
            public const int MAXIM_CARACTERE_MOTIV = 200;
            public const int DURATA_MINIMA_PROGRAMARE = 15;
            public const int DURATA_MAXIMA_PROGRAMARE = 120;
            public const int INTERVAL_DURATA = 15;
            public const int ZILE_MINIM_PROGRAMARE = 0;
            public const int ZILE_MAXIM_PROGRAMARE = 60;
        }

        public enum StatusProgramare
        {
            Programat,
            Confirmat,
            InAsteptare,
            Anulat,
            Finalizat
        }

        private void BtnAdaugareProgramare_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugareProgramari))
            {
                string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                string caleCompletaFisierProgramari = Path.Combine(locatieFisierSolutie, "Programari.txt");

                Pacient[] pacienti = adminPacienti.GetPacienti(out int nrPacienti);
                Medic[] medici = adminMedici.GetMedici(out int nrMedici);

                if (nrPacienti == 0 || nrMedici == 0)
                {
                    MessageBox.Show("Nu exista suficienti pacienti sau medici pentru a crea o programare!", "Eroare",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (MetroForm formSelectiePacient = new MetroForm())
                {
                    formSelectiePacient.Size = new Size(1080, 1024);
                    formSelectiePacient.Style = MetroFramework.MetroColorStyle.Black;
                    formSelectiePacient.StartPosition = FormStartPosition.CenterScreen;
                    formSelectiePacient.FormBorderStyle = FormBorderStyle.FixedDialog;
                    formSelectiePacient.MaximizeBox = false;
                    formSelectiePacient.MinimizeBox = false;
                    List<Label> eticheteCampuri = new List<Label>();

                    MetroLabel lblInstructiunePacient = new MetroLabel
                    {
                        Text = "Selectati pacientul pentru programare:",
                        Location = new Point(10, 10),
                        Width = 380,
                        Font = new Font(SystemFonts.DefaultFont.FontFamily, 10, FontStyle.Bold),
                        Name = "lblInstructiunePacient"
                    };

                    ListBox lstPacienti = new ListBox
                    {
                        Location = new Point(10, 40),
                        Width = 380,
                        Height = 320,
                        Name = "lstPacienti"
                    };

                    foreach (var pacient in pacienti)
                    {
                        lstPacienti.Items.Add($"{pacient.IdPacient} - {pacient.Nume} {pacient.Prenume}");
                    }

                    MetroButton btnSelecteazaPacient = new MetroButton
                    {
                        Text = "Selecteaza Pacient",
                        Location = new Point(10, 370),
                        Width = 380,
                        Height = 35,
                        Name = "btnSelecteazaPacient"
                    };

                    Pacient pacientSelectat = null;
                    btnSelecteazaPacient.Click += (s, ev) =>
                    {
                        if (lstPacienti.SelectedItem != null)
                        {
                            int idPacientSelectat = int.Parse(lstPacienti.SelectedItem.ToString().Split('-')[0].Trim());
                            pacientSelectat = adminPacienti.GetPacientDupaId(idPacientSelectat);
                            formSelectiePacient.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show("Va rugam selectati un pacient din lista!", "Atentie",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    };

                    formSelectiePacient.Controls.Add(btnSelecteazaPacient);
                    formSelectiePacient.Controls.Add(lblInstructiunePacient);
                    formSelectiePacient.Controls.Add(lstPacienti);
                    formSelectiePacient.Controls.Add(btnSelecteazaPacient);

                    if (formSelectiePacient.ShowDialog() != DialogResult.OK)
                        return;

                    using (MetroForm formSelectieMedic = new MetroForm())
                    {
                        formSelectieMedic.Size = new Size(1080, 1024);
                        formSelectieMedic.StartPosition = FormStartPosition.CenterScreen;
                        formSelectieMedic.FormBorderStyle = FormBorderStyle.FixedDialog;
                        formSelectieMedic.MaximizeBox = false;
                        formSelectieMedic.MinimizeBox = false;

                        MetroLabel lblInstructiuneMedic = new MetroLabel
                        {
                            Text = "Selectati medicul pentru programare:",
                            Location = new Point(10, 10),
                            Width = 380,
                            Font = new Font(SystemFonts.DefaultFont.FontFamily, 10, FontStyle.Bold),
                            Name = "lblInstructiuneMedic"
                        };

                        ListBox lstMedici = new ListBox
                        {
                            Location = new Point(10, 40),
                            Width = 380,
                            Height = 320,
                            Name = "lstMedici"
                        };

                        foreach (var medic in medici)
                        {
                            lstMedici.Items.Add($"{medic.IdUser} - Dr. {medic.Nume} {medic.Prenume} ({medic.Specialitate})");
                        }

                        MetroButton btnSelecteazaMedic = new MetroButton
                        {
                            Text = "Selecteaza Medic",
                            Location = new Point(10, 370),
                            Width = 380,
                            Height = 35,
                            Name = "btnSelecteazaMedic"
                        };

                        Medic medicSelectat = null;
                        btnSelecteazaMedic.Click += (s, ev) =>
                        {
                            if (lstMedici.SelectedItem != null)
                            {
                                int idMedicSelectat = int.Parse(lstMedici.SelectedItem.ToString().Split('-')[0].Trim());
                                medicSelectat = adminMedici.GetMedicDupaId(idMedicSelectat);
                                formSelectieMedic.DialogResult = DialogResult.OK;
                            }
                            else
                            {
                                MessageBox.Show("Va rugam selectati un medic din lista!", "Atentie",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        };

                        formSelectieMedic.Controls.Add(lblInstructiuneMedic);
                        formSelectieMedic.Controls.Add(lstMedici);
                        formSelectieMedic.Controls.Add(btnSelecteazaMedic);

                        if (formSelectieMedic.ShowDialog() != DialogResult.OK)
                            return;

                        using (MetroForm formProgramare = new MetroForm())
                        {
                            formProgramare.Size = new Size(800, 400);
                            formProgramare.StartPosition = FormStartPosition.CenterScreen;
                            formProgramare.Style = MetroFramework.MetroColorStyle.Black;
                            MetroPanel panelFormular = new MetroPanel
                            {
                                Dock = DockStyle.Fill,
                                AutoScroll = true,
                                Name = "panelFormular"
                            };

                            MetroLabel lblInfoProgramare = new MetroLabel
                            {
                                Text = $"Programare: {pacientSelectat.Nume} {pacientSelectat.Prenume} la Dr. {medicSelectat.Nume} {medicSelectat.Prenume}",
                                Location = new Point(10, 10),
                                Width = 460,
                                Font = new Font(SystemFonts.DefaultFont.FontFamily, 10, FontStyle.Bold),
                                Name = "lblInfoProgramare"
                            };

                            MetroLabel lblMotieProgramare = new MetroLabel { Text = "Motiv programare *:", Location = new Point(10, 50), Width = 150, Name = "lblMotivProgramare" };
                            MetroLabel lblStatusProgramare = new MetroLabel { Text = "Status programare *:", Location = new Point(10, 120), Width = 150, Name = "lblStatusProgramare" };
                            MetroLabel lblDataOraProgramare = new MetroLabel { Text = "Data si Ora *:", Location = new Point(10, 150), Width = 150, Name = "lblDataOraProgramare" };
                            MetroLabel lblDurataProgramare = new MetroLabel { Text = "Durata (minute) *:", Location = new Point(10, 180), Width = 150, Name = "lblDurataProgramare" };

                            TextBox txtMotivProgramare = new TextBox
                            {
                                Location = new Point(170, 50),
                                Width = 300,
                                Height = 60,
                                Multiline = true,
                                Name = "txtMotivProgramare"
                            };

                            MetroComboBox cboStatusProgramare = new MetroComboBox
                            {
                                Location = new Point(170, 120),
                                Width = 300,
                                DropDownStyle = ComboBoxStyle.DropDownList,
                                Name = "cboStatusProgramare"
                            };

                            foreach (StatusProgramare status in Enum.GetValues(typeof(StatusProgramare)))
                            {
                                cboStatusProgramare.Items.Add(status);
                            }
                            cboStatusProgramare.SelectedIndex = 0;

                            DateTimePicker dtpDataOraProgramare = new DateTimePicker
                            {
                                Format = DateTimePickerFormat.Custom,
                                CustomFormat = "dd MMMM yyyy HH:mm",
                                Location = new Point(170, 150),
                                Width = 300,
                                MinDate = DateTime.Now.Date,
                                MaxDate = DateTime.Now.Date.AddDays(ConstanteValidare.ZILE_MAXIM_PROGRAMARE),
                                Name = "dtpDataOraProgramare"
                            };


                            NumericUpDown nudDurataProgramare = new NumericUpDown
                            {
                                Location = new Point(170, 180),
                                Width = 300,
                                Minimum = ConstanteValidare.DURATA_MINIMA_PROGRAMARE,
                                Maximum = ConstanteValidare.DURATA_MAXIMA_PROGRAMARE,
                                Increment = ConstanteValidare.INTERVAL_DURATA,
                                Value = 30,
                                Name = "nudDurataProgramare"
                            };

                            MetroButton btnSalveazaProgramare = new MetroButton
                            {
                                Text = "Salveaza Programarea",
                                Location = new Point(10, 260),
                                Width = 460,
                                Height = 40,
                                Name = "btnSalveazaProgramare"
                            };

                            btnSalveazaProgramare.Click += (s, ev) =>
                            {

                                foreach (MetroLabel lbl in eticheteCampuri)
                                {
                                    lbl.ForeColor = SystemColors.ControlText;
                                }
                                txtMotivProgramare.BackColor = SystemColors.Window;
                                if (lblMesajEroare == null)
                                {
                                    lblMesajEroare = new Label();
                                    lblMesajEroare.Text = "Mesajul de eroare";
                                    lblMesajEroare.Location = new Point(100, 100);
                                    lblMesajEroare.Size = new Size(200, 30);
                                    this.Controls.Add(lblMesajEroare);
                                }
                                lblMesajEroare.Visible = false;

                                bool valid = true;
                                string mesajEroare = "";

                                if (string.IsNullOrWhiteSpace(txtMotivProgramare.Text))
                                {
                                    lblMotieProgramare.ForeColor = Color.Red;
                                    txtMotivProgramare.BackColor = Color.LightPink;
                                    valid = false;
                                    mesajEroare = "Motivul programarii este obligatoriu!";
                                }
                                else if (txtMotivProgramare.Text.Length < ConstanteValidare.MINIM_CARACTERE_MOTIV)
                                {
                                    lblMotieProgramare.ForeColor = Color.Red;
                                    txtMotivProgramare.BackColor = Color.LightPink;
                                    valid = false;
                                    mesajEroare = $"Motivul programarii trebuie sa contina minim {ConstanteValidare.MINIM_CARACTERE_MOTIV} caractere!";
                                }
                                else if (txtMotivProgramare.Text.Length > ConstanteValidare.MAXIM_CARACTERE_MOTIV)
                                {
                                    lblMotieProgramare.ForeColor = Color.Red;
                                    txtMotivProgramare.BackColor = Color.LightPink;
                                    valid = false;
                                    mesajEroare = $"Motivul programarii nu poate depasi {ConstanteValidare.MAXIM_CARACTERE_MOTIV} caractere!";
                                }

                                if (cboStatusProgramare.SelectedItem == null)
                                {
                                    lblStatusProgramare.ForeColor = Color.Red;
                                    valid = false;
                                    mesajEroare = "Statusul programarii este obligatoriu!";
                                }

                                if (dtpDataOraProgramare.Value.Date < DateTime.Now.Date)
                                {
                                    lblDataOraProgramare.ForeColor = Color.Red;
                                    valid = false;
                                    mesajEroare = "Data programarii nu poate fi in trecut!";
                                }
                                else if (dtpDataOraProgramare.Value.Date > DateTime.Now.Date.AddDays(ConstanteValidare.ZILE_MAXIM_PROGRAMARE))
                                {
                                    lblDataOraProgramare.ForeColor = Color.Red;
                                    valid = false;
                                    mesajEroare = $"Programarea nu poate fi facuta cu mai mult de {ConstanteValidare.ZILE_MAXIM_PROGRAMARE} zile in avans!";
                                }
                                else if (dtpDataOraProgramare.Value.Hour < 8 || dtpDataOraProgramare.Value.Hour >= 20)
                                {
                                    lblDataOraProgramare.ForeColor = Color.Red;
                                    valid = false;
                                    mesajEroare = "Programarile se pot face doar intre orele 8:00 si 20:00!";
                                }

                                if (nudDurataProgramare.Value < ConstanteValidare.DURATA_MINIMA_PROGRAMARE ||
                                    nudDurataProgramare.Value > ConstanteValidare.DURATA_MAXIMA_PROGRAMARE)
                                {
                                    lblDurataProgramare.ForeColor = Color.Red;
                                    valid = false;
                                    mesajEroare = $"Durata programarii trebuie sa fie intre {ConstanteValidare.DURATA_MINIMA_PROGRAMARE} si {ConstanteValidare.DURATA_MAXIMA_PROGRAMARE} minute!";
                                }
                                else if (nudDurataProgramare.Value % ConstanteValidare.INTERVAL_DURATA != 0)
                                {
                                    lblDurataProgramare.ForeColor = Color.Red;
                                    valid = false;
                                    mesajEroare = $"Durata programarii trebuie sa fie in intervale de {ConstanteValidare.INTERVAL_DURATA} minute!";
                                }

                                if (!valid)
                                {
                                    lblMesajEroare.Text = mesajEroare;
                                    lblMesajEroare.Visible = true;
                                    return;
                                }

                                try
                                {
                                    Programare programareNoua = new Programare
                                    {
                                        IdPacient = pacientSelectat.IdPacient,
                                        IdMedic = medicSelectat.IdUser,
                                        DataOra = dtpDataOraProgramare.Value,
                                        Durata = TimeSpan.FromMinutes((double)nudDurataProgramare.Value),
                                        Motiv = txtMotivProgramare.Text.Trim(),
                                        Status = cboStatusProgramare.SelectedItem.ToString()
                                    };

                                    adminProgramari.AddProgramare(programareNoua, caleCompletaFisierProgramari);
                                    MessageBox.Show($"Programare adaugata cu succes pentru pacientul {pacientSelectat.Nume} {pacientSelectat.Prenume} la Dr. {medicSelectat.Nume} {medicSelectat.Prenume}!",
                                        "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    formProgramare.DialogResult = DialogResult.OK;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Eroare la adaugarea programarii: {ex.Message}",
                                        "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            };


                            panelFormular.Controls.Add(lblInfoProgramare);
                            foreach (MetroLabel lbl in eticheteCampuri)
                            {
                                panelFormular.Controls.Add(lbl);
                            }


                            panelFormular.Controls.Add(lblInfoProgramare);
                            panelFormular.Controls.Add(lblMotieProgramare);
                            panelFormular.Controls.Add(lblStatusProgramare);
                            panelFormular.Controls.Add(lblDataOraProgramare);
                            panelFormular.Controls.Add(lblDurataProgramare);
                            panelFormular.Controls.Add(txtMotivProgramare);
                            panelFormular.Controls.Add(cboStatusProgramare);
                            panelFormular.Controls.Add(dtpDataOraProgramare);
                            panelFormular.Controls.Add(nudDurataProgramare);
                            panelFormular.Controls.Add(btnSalveazaProgramare);

                            formProgramare.Controls.Add(panelFormular);
                            formProgramare.ShowDialog();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a adauga programari!");
            }
        }


        private void BtnVizualizareProgramari_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareProgramari))
            {
                Programare[] programari = adminProgramari.GetProgramari(out int nrProgramari);

                using (MetroForm formVizualizare = new MetroForm())
                {
                    formVizualizare.Text = "Toate Programarile";
                    formVizualizare.Size = new Size(700, 400);
                    formVizualizare.StartPosition = FormStartPosition.CenterScreen;
                    formVizualizare.Style = MetroFramework.MetroColorStyle.Black;

                    ListView listView = new ListView
                    {
                        Dock = DockStyle.Fill,
                        View = View.Details
                    };

                    listView.Columns.Add("ID", 50);
                    listView.Columns.Add("Pacient", 150);
                    listView.Columns.Add("Medic", 150);
                    listView.Columns.Add("Data", 150);
                    listView.Columns.Add("Durata", 100);
                    listView.Columns.Add("Motiv", 150);
                    listView.Columns.Add("Status", 100);

                    foreach (var programare in programari)
                    {
                        Pacient pacient = adminPacienti.GetPacientDupaId(programare.IdPacient);
                        Medic medic = adminMedici.GetMedicDupaId(programare.IdMedic);

                        ListViewItem item = new ListViewItem(programare.IdProgramare.ToString());
                        item.SubItems.Add($"{pacient.IdPacient} - {pacient.Nume} {pacient.Prenume}");
                        item.SubItems.Add($"{medic.IdUser} - Dr. {medic.Nume} {medic.Prenume}");
                        item.SubItems.Add(programare.DataOra.ToString("yyyy-MM-dd HH:mm"));
                        item.SubItems.Add(programare.Durata.ToString(@"hh\:mm"));
                        item.SubItems.Add(programare.Motiv);
                        item.SubItems.Add(programare.Status);

                        listView.Items.Add(item);
                    }

                    formVizualizare.Controls.Add(listView);
                    formVizualizare.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a vizualiza programari!");
            }
        }


        private void BtnVizualizareProgramariPacient_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareProgramari))
            {
                Pacient[] pacienti = adminPacienti.GetPacienti(out int nrPacienti);

                using (MetroForm formSelectiePacient = new MetroForm())
                {
                    formSelectiePacient.Size = new Size(800, 400);
                    formSelectiePacient.StartPosition = FormStartPosition.CenterScreen;
                    formSelectiePacient.Style = MetroFramework.MetroColorStyle.Black;

                    ListBox listPacienti = new ListBox
                    {
                        Dock = DockStyle.Fill
                    };

                    foreach (var pacient in pacienti)
                    {
                        listPacienti.Items.Add($"{pacient.IdPacient} - {pacient.Nume} {pacient.Prenume}");
                    }

                    MetroButton btnVizualizeaza = new MetroButton
                    {
                        Text = "Vizualizeaza Programari",
                        Dock = DockStyle.Bottom
                    };

                    btnVizualizeaza.Click += (s, ev) =>
                    {
                        if (listPacienti.SelectedItem != null)
                        {
                            int idPacientSelectat = int.Parse(listPacienti.SelectedItem.ToString().Split('-')[0].Trim());
                            Programare[] programariPacient = adminProgramari.GetProgramariDupaPacient(idPacientSelectat);

                            using (MetroForm formVizualizare = new MetroForm())
                            {
                                formVizualizare.Text = $"Programari Pacient";
                                formVizualizare.Size = new Size(700, 400);
                                formVizualizare.StartPosition = FormStartPosition.CenterScreen;
                                formVizualizare.Style = MetroFramework.MetroColorStyle.Black;

                                ListView listView = new ListView
                                {
                                    Dock = DockStyle.Fill,
                                    View = View.Details
                                };

                                listView.Columns.Add("ID", 50);
                                listView.Columns.Add("Medic", 200);
                                listView.Columns.Add("Data", 150);
                                listView.Columns.Add("Durata", 100);
                                listView.Columns.Add("Motiv", 150);
                                listView.Columns.Add("Status", 100);

                                foreach (var programare in programariPacient)
                                {
                                    Medic medic = adminMedici.GetMedicDupaId(programare.IdMedic);

                                    ListViewItem item = new ListViewItem(programare.IdProgramare.ToString());
                                    item.SubItems.Add($"Dr. {medic.Nume} {medic.Prenume}");
                                    item.SubItems.Add(programare.DataOra.ToString("yyyy-MM-dd HH:mm"));
                                    item.SubItems.Add(programare.Durata.ToString(@"hh\:mm"));
                                    item.SubItems.Add(programare.Motiv);
                                    item.SubItems.Add(programare.Status);

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
                MessageBox.Show("Nu aveti permisiunea de a vizualiza programari!");
            }
        }


        private void BtnVizualizareProgramariMedic_Click(object sender, EventArgs e)
        {

            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareProgramari))
            {


                Medic[] medici = adminMedici.GetMedici(out int nrMedici);

                using (MetroForm formSelectieMedic = new MetroForm())
                {
                    formSelectieMedic.Size = new Size(800, 400);
                    formSelectieMedic.StartPosition = FormStartPosition.CenterScreen;
                    formSelectieMedic.Style = MetroFramework.MetroColorStyle.Black;

                    ListBox listMedici = new ListBox
                    {
                        Dock = DockStyle.Fill
                    };

                    foreach (var medic in medici)
                    {
                        listMedici.Items.Add($"{medic.IdUser} - Dr. {medic.Nume} {medic.Prenume}");
                    }

                    MetroButton btnVizualizeaza = new MetroButton
                    {
                        Text = "Vizualizeaza Programari",
                        Dock = DockStyle.Bottom
                    };

                    btnVizualizeaza.Click += (s, ev) =>
                    {
                        if (listMedici.SelectedItem != null)
                        {
                            int idMedicSelectat = int.Parse(listMedici.SelectedItem.ToString().Split('-')[0].Trim());
                            Programare[] programariMedic = adminProgramari.GetProgramariDupaMedic(idMedicSelectat);

                            using (MetroForm formVizualizare = new MetroForm())
                            {
                                formVizualizare.Text = $"Programari Medic";
                                formVizualizare.Size = new Size(700, 400);
                                formVizualizare.StartPosition = FormStartPosition.CenterScreen;
                                formVizualizare.Style = MetroFramework.MetroColorStyle.Black;

                                ListView listView = new ListView
                                {
                                    Dock = DockStyle.Fill,
                                    View = View.Details
                                };

                                listView.Columns.Add("ID", 50);
                                listView.Columns.Add("Pacient", 200);
                                listView.Columns.Add("Data", 150);
                                listView.Columns.Add("Durata", 100);
                                listView.Columns.Add("Motiv", 150);
                                listView.Columns.Add("Status", 100);

                                foreach (var programare in programariMedic)
                                {
                                    Pacient pacient = adminPacienti.GetPacientDupaId(programare.IdPacient);

                                    ListViewItem item = new ListViewItem(programare.IdProgramare.ToString());
                                    item.SubItems.Add($"{pacient.Nume} {pacient.Prenume}");
                                    item.SubItems.Add(programare.DataOra.ToString("yyyy-MM-dd HH:mm"));
                                    item.SubItems.Add(programare.Durata.ToString(@"hh\:mm"));
                                    item.SubItems.Add(programare.Motiv);
                                    item.SubItems.Add(programare.Status);

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
                MessageBox.Show("Nu aveti permisiunea de a vizualiza programari!");
            }
        }


        private void BtnVizualizareProgramariData_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareProgramari))
            {
                using (MetroForm formSelectieData = new MetroForm())
                {
                    formSelectieData.Text = "Selecteaza Data";
                    formSelectieData.Size = new Size(300, 250);
                    formSelectieData.StartPosition = FormStartPosition.CenterScreen;
                    formSelectieData.Style = MetroFramework.MetroColorStyle.Black;

                    DateTimePicker dtpData = new DateTimePicker
                    {
                        Format = DateTimePickerFormat.Short,
                        Dock = DockStyle.Top
                    };

                    MetroButton btnVizualizeaza = new MetroButton
                    {
                        Text = "Vizualizeaza Programari",
                        Dock = DockStyle.Bottom
                    };

                    btnVizualizeaza.Click += (s, ev) =>
                    {
                        Programare[] programariData = adminProgramari.GetProgramariDupaData(dtpData.Value);

                        using (MetroForm formVizualizare = new MetroForm())
                        {
                            formVizualizare.Text = $"Programari din {dtpData.Value.ToShortDateString()}";
                            formVizualizare.Size = new Size(700, 400);
                            formVizualizare.StartPosition = FormStartPosition.CenterScreen;
                            formVizualizare.Style = MetroFramework.MetroColorStyle.Black;

                            ListView listView = new ListView
                            {
                                Dock = DockStyle.Fill,
                                View = View.Details
                            };

                            listView.Columns.Add("ID", 50);
                            listView.Columns.Add("Pacient", 150);
                            listView.Columns.Add("Medic", 150);
                            listView.Columns.Add("Data", 150);
                            listView.Columns.Add("Durata", 100);
                            listView.Columns.Add("Motiv", 150);
                            listView.Columns.Add("Status", 100);

                            foreach (var programare in programariData)
                            {
                                Pacient pacient = adminPacienti.GetPacientDupaId(programare.IdPacient);
                                Medic medic = adminMedici.GetMedicDupaId(programare.IdMedic);

                                ListViewItem item = new ListViewItem(programare.IdProgramare.ToString());
                                item.SubItems.Add($"{pacient.Nume} {pacient.Prenume}");
                                item.SubItems.Add($"Dr. {medic.Nume} {medic.Prenume}");
                                item.SubItems.Add(programare.DataOra.ToString("yyyy-MM-dd HH:mm"));
                                item.SubItems.Add(programare.Durata.ToString(@"hh\:mm"));
                                item.SubItems.Add(programare.Motiv);
                                item.SubItems.Add(programare.Status);

                                listView.Items.Add(item);
                            }

                            formVizualizare.Controls.Add(listView);
                            formVizualizare.ShowDialog();
                        }

                        formSelectieData.DialogResult = DialogResult.OK;
                    };

                    formSelectieData.Controls.Add(dtpData);
                    formSelectieData.Controls.Add(btnVizualizeaza);

                    formSelectieData.ShowDialog();
                }
            }

            else
            {
                MessageBox.Show("Nu aveti permisiunea de a vizualiza programari!");
            }
        }


        private void BtnCautareProgramari_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareProgramari))
            {
                using (MetroForm formCautare = new MetroForm())
                {
                    formCautare.Size = new Size(1200, 600);
                    formCautare.StartPosition = FormStartPosition.CenterScreen;
                    formCautare.Style = MetroFramework.MetroColorStyle.Black;
                    formCautare.MaximizeBox = false;
                    formCautare.MinimizeBox = false;

                    MetroPanel panelPrincipal = new MetroPanel
                    {
                        Dock = DockStyle.Fill,
                        AutoScroll = true,
                        Padding = new Padding(20)
                    };

                    MetroLabel lblTitlu = new MetroLabel
                    {
                        Text = "Selectati criteriile de cautare:",
                        Location = new Point(20, 20),
                        Size = new Size(300, 30),
                        FontWeight = MetroFramework.MetroLabelWeight.Bold,
                        FontSize = MetroFramework.MetroLabelSize.Medium
                    };

                    GroupBox gbCriterii = new GroupBox
                    {
                        Text = "Campuri de cautare",
                        Location = new Point(20, 60),
                        Size = new Size(250, 200),
                        Font = new Font(SystemFonts.DefaultFont.FontFamily, 9)
                    };

                    CheckBox chkPacient = new CheckBox { Text = "Pacient", Location = new Point(20, 30), AutoSize = true };
                    CheckBox chkMedic = new CheckBox { Text = "Medic", Location = new Point(20, 60), AutoSize = true };
                    CheckBox chkData = new CheckBox { Text = "Data programarii", Location = new Point(20, 90), AutoSize = true };
                    CheckBox chkMotiv = new CheckBox { Text = "Motiv", Location = new Point(20, 120), AutoSize = true };
                    CheckBox chkStatus = new CheckBox { Text = "Status", Location = new Point(20, 150), AutoSize = true };

                    gbCriterii.Controls.AddRange(new Control[] { chkPacient, chkMedic, chkData, chkMotiv, chkStatus });

                    GroupBox gbPotrivire = new GroupBox
                    {
                        Text = "Tip de potrivire",
                        Location = new Point(300, 60),
                        Size = new Size(250, 120),
                        Font = new Font(SystemFonts.DefaultFont.FontFamily, 9)
                    };

                    RadioButton rbExact = new RadioButton { Text = "Potrivire exacta", Location = new Point(20, 30), AutoSize = true, Checked = true };
                    RadioButton rbPartial = new RadioButton { Text = "Potrivire partiala", Location = new Point(20, 60), AutoSize = true };

                    gbPotrivire.Controls.AddRange(new Control[] { rbExact, rbPartial });

                    MetroLabel lblValoareCautare = new MetroLabel
                    {
                        Text = "Introduceti termenii de cautare:",
                        Location = new Point(20, 280),
                        Size = new Size(250, 30)
                    };

                    TextBox txtValoareCautare = new TextBox
                    {
                        Location = new Point(20, 310),
                        Size = new Size(530, 30)
                    };

                    MetroButton btnCauta = new MetroButton
                    {
                        Text = "Cauta",
                        Location = new Point(20, 360),
                        Size = new Size(530, 40)
                    };

                    btnCauta.Click += (s, ev) =>
                    {
                        if (!chkPacient.Checked && !chkMedic.Checked && !chkData.Checked && !chkMotiv.Checked && !chkStatus.Checked)
                        {
                            MessageBox.Show("Selectati cel putin un criteriu de cautare!", "Atentie",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(txtValoareCautare.Text))
                        {
                            MessageBox.Show("Introduceti termenii de cautare!", "Atentie",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        Programare[] toateProgramarile = adminProgramari.GetProgramari(out int nrProgramari);
                        List<Programare> rezultateCautare = new List<Programare>();

                        string termenCautare = txtValoareCautare.Text.ToLower();
                        bool estePotrivireExacta = rbExact.Checked;

                        foreach (Programare programare in toateProgramarile)
                        {
                            bool adaugaProgramare = false;

                            if (chkPacient.Checked)
                            {
                                Pacient pacient = adminPacienti.GetPacientDupaId(programare.IdPacient);
                                string numePacient = $"{pacient.Nume} {pacient.Prenume}".ToLower();

                                if (estePotrivireExacta)
                                {
                                    if (numePacient.Equals(termenCautare))
                                        adaugaProgramare = true;
                                }
                                else
                                {
                                    if (numePacient.Contains(termenCautare))
                                        adaugaProgramare = true;
                                }
                            }

                            if (chkMedic.Checked && !adaugaProgramare)
                            {
                                Medic medic = adminMedici.GetMedicDupaId(programare.IdMedic);
                                string numeMedic = $"{medic.Nume} {medic.Prenume}".ToLower();

                                if (estePotrivireExacta)
                                {
                                    if (numeMedic.Equals(termenCautare))
                                        adaugaProgramare = true;
                                }
                                else
                                {
                                    if (numeMedic.Contains(termenCautare))
                                        adaugaProgramare = true;
                                }
                            }

                            if (chkData.Checked && !adaugaProgramare)
                            {
                                string dataProgramare = programare.DataOra.ToString("yyyy-MM-dd").ToLower();

                                if (estePotrivireExacta)
                                {
                                    if (dataProgramare.Equals(termenCautare))
                                        adaugaProgramare = true;
                                }
                                else
                                {
                                    if (dataProgramare.Contains(termenCautare))
                                        adaugaProgramare = true;
                                }
                            }

                            if (chkMotiv.Checked && !adaugaProgramare)
                            {
                                string motiv = programare.Motiv.ToLower();

                                if (estePotrivireExacta)
                                {
                                    if (motiv.Equals(termenCautare))
                                        adaugaProgramare = true;
                                }
                                else
                                {
                                    if (motiv.Contains(termenCautare))
                                        adaugaProgramare = true;
                                }
                            }

                            if (chkStatus.Checked && !adaugaProgramare)
                            {
                                string status = programare.Status.ToLower();

                                if (estePotrivireExacta)
                                {
                                    if (status.Equals(termenCautare))
                                        adaugaProgramare = true;
                                }
                                else
                                {
                                    if (status.Contains(termenCautare))
                                        adaugaProgramare = true;
                                }
                            }

                            if (adaugaProgramare)
                            {
                                rezultateCautare.Add(programare);
                            }
                        }

                        AfiseazaRezultateCautare(rezultateCautare.ToArray());
                    };

                    panelPrincipal.Controls.Add(lblTitlu);
                    panelPrincipal.Controls.Add(gbCriterii);
                    panelPrincipal.Controls.Add(gbPotrivire);
                    panelPrincipal.Controls.Add(lblValoareCautare);
                    panelPrincipal.Controls.Add(txtValoareCautare);
                    panelPrincipal.Controls.Add(btnCauta);

                    formCautare.Controls.Add(panelPrincipal);
                    formCautare.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a cauta programari!", "Acces interzis",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AfiseazaRezultateCautare(Programare[] programari)
        {
            using (MetroForm formRezultate = new MetroForm())
            {
                formRezultate.Text = "Rezultate Cautare";
                formRezultate.Size = new Size(800, 500);
                formRezultate.StartPosition = FormStartPosition.CenterScreen;
                formRezultate.Style = MetroFramework.MetroColorStyle.Black;

                MetroLabel lblInfo = new MetroLabel
                {
                    Text = $"S-au gasit {programari.Length} programari conform criteriilor selectate.",
                    Dock = DockStyle.Top,
                    Height = 30,
                    FontWeight = MetroFramework.MetroLabelWeight.Bold
                };

                ListView listViewRezultate = new ListView
                {
                    Dock = DockStyle.Fill,
                    View = View.Details,
                    FullRowSelect = true,
                    GridLines = true
                };

                listViewRezultate.Columns.Add("ID", 50);
                listViewRezultate.Columns.Add("Pacient", 150);
                listViewRezultate.Columns.Add("Medic", 150);
                listViewRezultate.Columns.Add("Data", 120);
                listViewRezultate.Columns.Add("Ora", 80);
                listViewRezultate.Columns.Add("Durata", 80);
                listViewRezultate.Columns.Add("Motiv", 150);
                listViewRezultate.Columns.Add("Status", 100);

                foreach (var programare in programari)
                {
                    Pacient pacient = adminPacienti.GetPacientDupaId(programare.IdPacient);
                    Medic medic = adminMedici.GetMedicDupaId(programare.IdMedic);

                    ListViewItem item = new ListViewItem(programare.IdProgramare.ToString());
                    item.SubItems.Add($"{pacient.Nume} {pacient.Prenume}");
                    item.SubItems.Add($"Dr. {medic.Nume} {medic.Prenume}");
                    item.SubItems.Add(programare.DataOra.ToString("yyyy-MM-dd"));
                    item.SubItems.Add(programare.DataOra.ToString("HH:mm"));
                    item.SubItems.Add(programare.Durata.ToString(@"hh\:mm"));
                    item.SubItems.Add(programare.Motiv);
                    item.SubItems.Add(programare.Status);

                    if (programare.Status == StatusProgramare.Anulat.ToString())
                        item.BackColor = Color.LightPink;
                    else if (programare.Status == StatusProgramare.Confirmat.ToString())
                        item.BackColor = Color.LightGreen;
                    else if (programare.Status == StatusProgramare.InAsteptare.ToString())
                        item.BackColor = Color.LightYellow;
                    else if (programare.Status == StatusProgramare.Finalizat.ToString())
                        item.BackColor = Color.LightBlue;

                    listViewRezultate.Items.Add(item);
                }

                Panel panelFiltrare = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 60
                };

                Label lblFilterStatus = new Label
                {
                    Text = "Filtrare rapida dupa status:",
                    Location = new Point(10, 20),
                    AutoSize = true
                };

                ComboBox cboFilterStatus = new ComboBox
                {
                    Location = new Point(160, 17),
                    Width = 150,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                cboFilterStatus.Items.Add("Toate statusurile");
                foreach (StatusProgramare status in Enum.GetValues(typeof(StatusProgramare)))
                {
                    cboFilterStatus.Items.Add(status);
                }
                cboFilterStatus.SelectedIndex = 0;


                cboFilterStatus.SelectedIndexChanged += (s, e) =>
                {
                    listViewRezultate.Items.Clear();

                    foreach (var programare in programari)
                    {
                        bool afiseazaProgramare = false;
                        if (cboFilterStatus.SelectedIndex == 0)
                        {
                            afiseazaProgramare = true;
                        }
                        else
                        {
                            string statusSelectat = cboFilterStatus.SelectedItem.ToString();
                            afiseazaProgramare = programare.Status == statusSelectat;
                        }

                        if (afiseazaProgramare)
                        {
                            Pacient pacient = adminPacienti.GetPacientDupaId(programare.IdPacient);
                            Medic medic = adminMedici.GetMedicDupaId(programare.IdMedic);

                            ListViewItem item = new ListViewItem(programare.IdProgramare.ToString());
                            item.SubItems.Add($"{pacient.Nume} {pacient.Prenume}");
                            item.SubItems.Add($"Dr. {medic.Nume} {medic.Prenume}");
                            item.SubItems.Add(programare.DataOra.ToString("yyyy-MM-dd"));
                            item.SubItems.Add(programare.DataOra.ToString("HH:mm"));
                            item.SubItems.Add(programare.Durata.ToString(@"hh\:mm"));
                            item.SubItems.Add(programare.Motiv);
                            item.SubItems.Add(programare.Status);

                            if (programare.Status == StatusProgramare.Anulat.ToString())
                                item.BackColor = Color.LightPink;
                            else if (programare.Status == StatusProgramare.Confirmat.ToString())
                                item.BackColor = Color.LightGreen;
                            else if (programare.Status == StatusProgramare.InAsteptare.ToString())
                                item.BackColor = Color.LightYellow;
                            else if (programare.Status == StatusProgramare.Finalizat.ToString())
                                item.BackColor = Color.LightBlue;

                            listViewRezultate.Items.Add(item);
                        }
                    }
                };

                panelFiltrare.Controls.Add(lblFilterStatus);
                panelFiltrare.Controls.Add(cboFilterStatus);

                formRezultate.Controls.Add(listViewRezultate);
                formRezultate.Controls.Add(lblInfo);
                formRezultate.Controls.Add(panelFiltrare);

                formRezultate.ShowDialog();
            }
        }


    }
}