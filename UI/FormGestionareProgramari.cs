﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LibrarieModele;
using NivelStocareDate;

namespace UI
{
    public partial class FormGestionareProgramari : Form
    {
        private AdministrareProgramari_FisierText adminProgramari;
        private AdministrarePacienti_FisierText adminPacienti;
        private AdministrareMedici_FisierText adminMedici;

        public FormGestionareProgramari(
            AdministrareProgramari_FisierText programariAdmin,
            AdministrarePacienti_FisierText pacientiAdmin,
            AdministrareMedici_FisierText mediciAdmin)
        {
            adminProgramari = programariAdmin;
            adminPacienti = pacientiAdmin;
            adminMedici = mediciAdmin;

            ConfigureazaMeniu();
        }


        private void ConfigureazaMeniu()
        {
            this.Text = "Gestionare Programari";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            FlowLayoutPanel panelMeniu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
            };

            Button btnAdaugareProgramare = new Button { Text = "Adaugare programare noua", Size = new Size(250, 40) };
            Button btnVizualizareProgramari = new Button { Text = "Vizualizare toate programarile", Size = new Size(250, 40) };
            Button btnVizualizareProgramariPacient = new Button { Text = "Programari pacient", Size = new Size(250, 40) };
            Button btnVizualizareProgramariMedic = new Button { Text = "Programari medic", Size = new Size(250, 40) };
            Button btnVizualizareProgramariData = new Button { Text = "Programari dupa data", Size = new Size(250, 40) };
            Button btnInchide = new Button { Text = "Inchide", Size = new Size(250, 40) };

            btnAdaugareProgramare.Click += BtnAdaugareProgramare_Click;
            btnVizualizareProgramari.Click += BtnVizualizareProgramari_Click;
            btnVizualizareProgramariPacient.Click += BtnVizualizareProgramariPacient_Click;
            btnVizualizareProgramariMedic.Click += BtnVizualizareProgramariMedic_Click;
            btnVizualizareProgramariData.Click += BtnVizualizareProgramariData_Click;
            btnInchide.Click += (s, e) => this.Close();

            panelMeniu.Controls.Add(btnAdaugareProgramare);
            panelMeniu.Controls.Add(btnVizualizareProgramari);
            panelMeniu.Controls.Add(btnVizualizareProgramariPacient);
            panelMeniu.Controls.Add(btnVizualizareProgramariMedic);
            panelMeniu.Controls.Add(btnVizualizareProgramariData);
            panelMeniu.Controls.Add(btnInchide);

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

            using (Form formSelectiePacient = new Form())
            {
                formSelectiePacient.Text = "Selecteaza Pacient";
                formSelectiePacient.Size = new Size(400, 450);
                formSelectiePacient.StartPosition = FormStartPosition.CenterScreen;
                formSelectiePacient.FormBorderStyle = FormBorderStyle.FixedDialog;
                formSelectiePacient.MaximizeBox = false;
                formSelectiePacient.MinimizeBox = false;

                Label lblInstructiunePacient = new Label
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

                Button btnSelecteazaPacient = new Button
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

                formSelectiePacient.Controls.Add(lblInstructiunePacient);
                formSelectiePacient.Controls.Add(lstPacienti);
                formSelectiePacient.Controls.Add(btnSelecteazaPacient);

                if (formSelectiePacient.ShowDialog() != DialogResult.OK)
                    return;

                using (Form formSelectieMedic = new Form())
                {
                    formSelectieMedic.Text = "Selecteaza Medic";
                    formSelectieMedic.Size = new Size(400, 450);
                    formSelectieMedic.StartPosition = FormStartPosition.CenterScreen;
                    formSelectieMedic.FormBorderStyle = FormBorderStyle.FixedDialog;
                    formSelectieMedic.MaximizeBox = false;
                    formSelectieMedic.MinimizeBox = false;

                    Label lblInstructiuneMedic = new Label
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

                    Button btnSelecteazaMedic = new Button
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

                    using (Form formProgramare = new Form())
                    {
                        formProgramare.Text = "Programare Noua";
                        formProgramare.Size = new Size(500, 450);
                        formProgramare.StartPosition = FormStartPosition.CenterScreen;
                        formProgramare.FormBorderStyle = FormBorderStyle.FixedDialog;
                        formProgramare.MaximizeBox = false;
                        formProgramare.MinimizeBox = false;

                        Panel panelFormular = new Panel
                        {
                            Dock = DockStyle.Fill,
                            AutoScroll = true,
                            Name = "panelFormular"
                        };

                        Label lblInfoProgramare = new Label
                        {
                            Text = $"Programare: {pacientSelectat.Nume} {pacientSelectat.Prenume} la Dr. {medicSelectat.Nume} {medicSelectat.Prenume}",
                            Location = new Point(10, 10),
                            Width = 460,
                            Font = new Font(SystemFonts.DefaultFont.FontFamily, 10, FontStyle.Bold),
                            Name = "lblInfoProgramare"
                        };

                        Label lblMotieProgramare = new Label { Text = "Motiv programare *:", Location = new Point(10, 50), Width = 150, Name = "lblMotivProgramare" };
                        Label lblStatusProgramare = new Label { Text = "Status programare *:", Location = new Point(10, 120), Width = 150, Name = "lblStatusProgramare" };
                        Label lblDataOraProgramare = new Label { Text = "Data si Ora *:", Location = new Point(10, 150), Width = 150, Name = "lblDataOraProgramare" };
                        Label lblDurataProgramare = new Label { Text = "Durata (minute) *:", Location = new Point(10, 180), Width = 150, Name = "lblDurataProgramare" };

                        Label[] eticheteCampuri = new Label[] {
                    lblMotieProgramare,
                    lblStatusProgramare,
                    lblDataOraProgramare,
                    lblDurataProgramare
                };

                        TextBox txtMotivProgramare = new TextBox
                        {
                            Location = new Point(170, 50),
                            Width = 300,
                            Height = 60,
                            Multiline = true,
                            Name = "txtMotivProgramare"
                        };

                        ComboBox cboStatusProgramare = new ComboBox
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

                        Label lblMesajEroare = new Label
                        {
                            Location = new Point(10, 210),
                            Width = 460,
                            Height = 40,
                            ForeColor = Color.Red,
                            Visible = false,
                            Name = "lblMesajEroare"
                        };

                        txtMotivProgramare.Leave += (s, ev) =>
                        {
                            if (!string.IsNullOrWhiteSpace(txtMotivProgramare.Text) &&
                                txtMotivProgramare.Text.Length < ConstanteValidare.MINIM_CARACTERE_MOTIV)
                            {
                                lblMotieProgramare.ForeColor = Color.Red;
                                txtMotivProgramare.BackColor = Color.LightPink;
                                lblMesajEroare.Text = $"Motivul programarii trebuie sa contina minim {ConstanteValidare.MINIM_CARACTERE_MOTIV} caractere!";
                                lblMesajEroare.Visible = true;
                            }
                            else if (!string.IsNullOrWhiteSpace(txtMotivProgramare.Text) &&
                                     txtMotivProgramare.Text.Length > ConstanteValidare.MAXIM_CARACTERE_MOTIV)
                            {
                                lblMotieProgramare.ForeColor = Color.Red;
                                txtMotivProgramare.BackColor = Color.LightPink;
                                lblMesajEroare.Text = $"Motivul programarii nu poate depasi {ConstanteValidare.MAXIM_CARACTERE_MOTIV} caractere!";
                                lblMesajEroare.Visible = true;
                            }
                            else
                            {
                                lblMotieProgramare.ForeColor = SystemColors.ControlText;
                                txtMotivProgramare.BackColor = SystemColors.Window;
                                lblMesajEroare.Visible = false;
                            }
                        };

                        dtpDataOraProgramare.ValueChanged += (s, ev) =>
                        {
                            if (dtpDataOraProgramare.Value.Date < DateTime.Now.Date)
                            {
                                lblDataOraProgramare.ForeColor = Color.Red;
                                lblMesajEroare.Text = "Data programarii nu poate fi in trecut!";
                                lblMesajEroare.Visible = true;
                            }
                            else if (dtpDataOraProgramare.Value.Date > DateTime.Now.Date.AddDays(ConstanteValidare.ZILE_MAXIM_PROGRAMARE))
                            {
                                lblDataOraProgramare.ForeColor = Color.Red;
                                lblMesajEroare.Text = $"Programarea nu poate fi facuta cu mai mult de {ConstanteValidare.ZILE_MAXIM_PROGRAMARE} zile in avans!";
                                lblMesajEroare.Visible = true;
                            }
                            else if (dtpDataOraProgramare.Value.Hour < 8 || dtpDataOraProgramare.Value.Hour >= 20)
                            {
                                lblDataOraProgramare.ForeColor = Color.Red;
                                lblMesajEroare.Text = "Programarile se pot face doar intre orele 8:00 si 20:00!";
                                lblMesajEroare.Visible = true;
                            }
                            else
                            {
                                lblDataOraProgramare.ForeColor = SystemColors.ControlText;
                                lblMesajEroare.Visible = false;
                            }
                        };

                        nudDurataProgramare.ValueChanged += (s, ev) =>
                        {
                            if (nudDurataProgramare.Value < ConstanteValidare.DURATA_MINIMA_PROGRAMARE ||
                                nudDurataProgramare.Value > ConstanteValidare.DURATA_MAXIMA_PROGRAMARE)
                            {
                                lblDurataProgramare.ForeColor = Color.Red;
                                lblMesajEroare.Text = $"Durata programarii trebuie sa fie intre {ConstanteValidare.DURATA_MINIMA_PROGRAMARE} si {ConstanteValidare.DURATA_MAXIMA_PROGRAMARE} minute!";
                                lblMesajEroare.Visible = true;
                            }
                            else if (nudDurataProgramare.Value % ConstanteValidare.INTERVAL_DURATA != 0)
                            {
                                lblDurataProgramare.ForeColor = Color.Red;
                                lblMesajEroare.Text = $"Durata programarii trebuie sa fie in intervale de {ConstanteValidare.INTERVAL_DURATA} minute!";
                                lblMesajEroare.Visible = true;
                            }
                            else
                            {
                                lblDurataProgramare.ForeColor = SystemColors.ControlText;
                                lblMesajEroare.Visible = false;
                            }
                        };

                        Button btnSalveazaProgramare = new Button
                        {
                            Text = "Salveaza Programarea",
                            Location = new Point(10, 260),
                            Width = 460,
                            Height = 40,
                            Name = "btnSalveazaProgramare"
                        };

                        btnSalveazaProgramare.Click += (s, ev) =>
                        {
                            foreach (Label lbl in eticheteCampuri)
                            {
                                lbl.ForeColor = SystemColors.ControlText;
                            }
                            txtMotivProgramare.BackColor = SystemColors.Window;
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
                        foreach (Label lbl in eticheteCampuri)
                        {
                            panelFormular.Controls.Add(lbl);
                        }
                        panelFormular.Controls.Add(txtMotivProgramare);
                        panelFormular.Controls.Add(cboStatusProgramare);
                        panelFormular.Controls.Add(dtpDataOraProgramare);
                        panelFormular.Controls.Add(nudDurataProgramare);
                        panelFormular.Controls.Add(lblMesajEroare);
                        panelFormular.Controls.Add(btnSalveazaProgramare);

                        formProgramare.Controls.Add(panelFormular);
                        formProgramare.ShowDialog();
                    }
                }
            }
        }

        private void BtnVizualizareProgramari_Click(object sender, EventArgs e)
        {
            Programare[] programari = adminProgramari.GetProgramari(out int nrProgramari);

            using (Form formVizualizare = new Form())
            {
                formVizualizare.Text = "Toate Programarile";
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

        private void BtnVizualizareProgramariPacient_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");

        }

        private void BtnVizualizareProgramariMedic_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");

        }

        private void BtnVizualizareProgramariData_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");

        }
    }
}