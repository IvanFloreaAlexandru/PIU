using System;
using System.IO;
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
            this.Size = new System.Drawing.Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            FlowLayoutPanel panelMeniu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
            };

            Button btnAdaugareDepartament = new Button { Text = "Adaugare departament nou", Size = new System.Drawing.Size(250, 40) };
            Button btnVizualizareDepartamente = new Button { Text = "Vizualizare toate departamentele", Size = new System.Drawing.Size(250, 40) };
            Button btnModificareDepartament = new Button { Text = "Modificare departament", Size = new System.Drawing.Size(250, 40) };
            Button btnInchide = new Button { Text = "Inchide", Size = new System.Drawing.Size(250, 40) };

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

        private void BtnAdaugareDepartament_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");

        }

        private void BtnVizualizareDepartamente_Click(object sender, EventArgs e)
        {
            Departament[] departamente = adminDepartamente.GetDepartamente(out int nrDepartamente);

            using (Form formVizualizare = new Form())
            {
                formVizualizare.Text = "Toate Departamentele";
                formVizualizare.Size = new System.Drawing.Size(600, 400);
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