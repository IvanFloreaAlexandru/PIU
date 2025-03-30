using System;
using System.IO;
using System.Windows.Forms;
using LibrarieModele;
using NivelStocareDate;

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
            this.Size = new System.Drawing.Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            FlowLayoutPanel panelMeniu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
            };

            Button btnAdaugarePrescriptie = new Button { Text = "Adaugare prescriptie noua", Size = new System.Drawing.Size(250, 40) };
            Button btnVizualizarePrescriptii = new Button { Text = "Vizualizare toate prescriptiile", Size = new System.Drawing.Size(250, 40) };
            Button btnVizualizarePrescriptiiPacient = new Button { Text = "Prescriptii pacient", Size = new System.Drawing.Size(250, 40) };
            Button btnVizualizarePrescriptiiMedic = new Button { Text = "Prescriptii emise de medic", Size = new System.Drawing.Size(250, 40) };
            Button btnVizualizarePrescriptiiData = new Button { Text = "Prescriptii dupa data", Size = new System.Drawing.Size(250, 40) };
            Button btnInchide = new Button { Text = "Inchide", Size = new System.Drawing.Size(250, 40) };

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

        private void BtnAdaugarePrescriptie_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");

        }

        private void BtnVizualizarePrescriptii_Click(object sender, EventArgs e)
        {
            Prescriptie[] prescriptii = adminPrescriptii.GetPrescriptii(out int nrPrescriptii);

            using (Form formVizualizare = new Form())
            {
                formVizualizare.Text = "Toate Prescriptiile";
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