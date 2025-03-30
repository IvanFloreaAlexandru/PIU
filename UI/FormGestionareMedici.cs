using System;
using System.Windows.Forms;
using System.Drawing;
using LibrarieModele;
using NivelStocareDate;
using System.IO;
using System.Configuration;

namespace UI
{
    public partial class FormGestionareMedici : Form
    {
        private FlowLayoutPanel panelMeniu;
        private Button btnVizualizareMedici;
        private Button btnVizualizareMediciSpecialitate;
        private Button btnVizualizareMediciDepartament;
        private Button btnInapoi;

        private AdministrareMedici_FisierText adminMedici;
        private AdministrareDepartamente_FisierText adminDepartamente;

        public FormGestionareMedici(AdministrareMedici_FisierText adminMedici, AdministrareDepartamente_FisierText adminDepartamente)
        {
            InitializeComponent();
            this.adminMedici = adminMedici;
            this.adminDepartamente = adminDepartamente;

            this.StartPosition = FormStartPosition.CenterScreen;

            panelMeniu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
            };

            this.Controls.Add(panelMeniu);

            ConfigureazaMeniu();

            btnInapoi.Click += BtnInapoi_Click;
            btnVizualizareMedici.Click += BtnVizualizareMedici_Click;
            btnVizualizareMediciSpecialitate.Click += BtnVizualizareMediciSpecialitate_Click;
            btnVizualizareMediciDepartament.Click += BtnVizualizareMediciDepartament_Click;
        }

        private void ConfigureazaMeniu()
        {
            panelMeniu.Controls.Clear();

            btnVizualizareMedici = new Button { Text = "Vizualizare medici" };
            btnVizualizareMediciSpecialitate = new Button { Text = "Vizualizare medici dupa specialitate" };
            btnVizualizareMediciDepartament = new Button { Text = "Vizualizare medici dupa departament" };
            btnInapoi = new Button { Text = "Inapoi" };

            btnVizualizareMedici.Size =
            btnVizualizareMediciSpecialitate.Size =
            btnVizualizareMediciDepartament.Size =
            btnInapoi.Size = new Size(200, 40);

            panelMeniu.Controls.Add(btnVizualizareMedici);
            panelMeniu.Controls.Add(btnVizualizareMediciSpecialitate);
            panelMeniu.Controls.Add(btnVizualizareMediciDepartament);
            panelMeniu.Controls.Add(btnInapoi);
        }

        private void BtnInapoi_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnVizualizareMedici_Click(object sender, EventArgs e)
        {
            Medic[] medici = adminMedici.GetMedici(out int nrMedici);

            string mediciInfo = "";
            foreach (var medic in medici)
            {
                Departament departament = adminDepartamente.GetDepartamentDupaId(medic.IdDepartament);
                string numeDepartament = departament != null ? departament.Nume : "Necunoscut";

                mediciInfo += $"ID: {medic.IdUser}, Nume: {medic.Nume}, Specialitate: {medic.Specialitate}, Departament: {numeDepartament}\n";
            }

            MessageBox.Show(mediciInfo, "Lista Medici");
        }

        private void BtnVizualizareMediciSpecialitate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitatea in curs de implementare.", "De implementat");

        }

        private void BtnVizualizareMediciDepartament_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitatea in curs de implementare.", "De implementat");

        }
    }
}