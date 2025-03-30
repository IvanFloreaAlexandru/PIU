using System;
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
            this.Size = new System.Drawing.Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            FlowLayoutPanel panelMeniu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
            };

            Button btnAdaugareProgramare = new Button { Text = "Adaugare programare noua", Size = new System.Drawing.Size(250, 40) };
            Button btnVizualizareProgramari = new Button { Text = "Vizualizare toate programarile", Size = new System.Drawing.Size(250, 40) };
            Button btnVizualizareProgramariPacient = new Button { Text = "Programari pacient", Size = new System.Drawing.Size(250, 40) };
            Button btnVizualizareProgramariMedic = new Button { Text = "Programari medic", Size = new System.Drawing.Size(250, 40) };
            Button btnVizualizareProgramariData = new Button { Text = "Programari dupa data", Size = new System.Drawing.Size(250, 40) };
            Button btnInchide = new Button { Text = "Inchide", Size = new System.Drawing.Size(250, 40) };

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

        private void BtnAdaugareProgramare_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");

        }

        private void BtnVizualizareProgramari_Click(object sender, EventArgs e)
        {
            Programare[] programari = adminProgramari.GetProgramari(out int nrProgramari);

            using (Form formVizualizare = new Form())
            {
                formVizualizare.Text = "Toate Programarile";
                formVizualizare.Size = new System.Drawing.Size(700, 400);
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