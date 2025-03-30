using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LibrarieModele;
using NivelStocareDate;

namespace UI
{
    public partial class FormGestionareUtilizatori : Form
    {
        private AdministrareUser_FisierText adminUtilizatori;
        private AdministrareUser_Memorie adminMemorie;


        public FormGestionareUtilizatori(AdministrareUser_FisierText utilizatoriAdmin, AdministrareUser_Memorie utilizatoriAdminMemorie)
        {
            adminUtilizatori = utilizatoriAdmin;
            adminMemorie = utilizatoriAdminMemorie;

            ConfigureazaMeniu();
        }

        private void ConfigureazaMeniu()
        {
            this.Text = "Gestionare Utilizatori";
            this.Size = new System.Drawing.Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            FlowLayoutPanel panelMeniu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
            };

            Button btnAdaugareUtilizator = new Button { Text = "Adaugare utilizator nou", Size = new System.Drawing.Size(250, 40) };
            Button btnVizualizareUtilizatori = new Button { Text = "Vizualizare toti utilizatorii", Size = new System.Drawing.Size(250, 40) };
            Button btnModificareUtilizator = new Button { Text = "Modificare utilizator", Size = new System.Drawing.Size(250, 40) };
            Button btnInchide = new Button { Text = "Inchide", Size = new System.Drawing.Size(250, 40) };

            btnAdaugareUtilizator.Click += BtnAdaugareUtilizator_Click;
            btnVizualizareUtilizatori.Click += BtnVizualizareUtilizatori_Click;
            btnModificareUtilizator.Click += BtnModificareUtilizatori_Click;
            btnInchide.Click += (s, e) => this.Close();

            panelMeniu.Controls.Add(btnAdaugareUtilizator);
            panelMeniu.Controls.Add(btnVizualizareUtilizatori);
            panelMeniu.Controls.Add(btnModificareUtilizator);
            panelMeniu.Controls.Add(btnInchide);

            this.Controls.Add(panelMeniu);
        }

        private void BtnAdaugareUtilizator_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");

        }


        private void BtnVizualizareUtilizatori_Click(object sender, EventArgs e)
        {
            try
            {
                string[] potentialPaths = new string[]
                {
            Path.Combine(Directory.GetCurrentDirectory(), "User.txt"),
            Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "User.txt")
                };

                string caleCompletaFisier = potentialPaths.FirstOrDefault(File.Exists);
                if (string.IsNullOrEmpty(caleCompletaFisier))
                {
                    MessageBox.Show("Fisierul User.txt nu a fost gasit!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string[] continutFisier = File.ReadAllLines(caleCompletaFisier);
                if (continutFisier.Length == 0)
                {
                    MessageBox.Show("Fisierul este gol!", "Informatie");
                    return;
                }

                using (Form formVizualizare = new Form())
                {
                    formVizualizare.Text = "Lista Utilizatori";
                    formVizualizare.Size = new Size(700, 400);

                    ListView listViewUtilizatori = new ListView
                    {
                        Dock = DockStyle.Fill,
                        View = View.Details,
                        FullRowSelect = true
                    };

                    listViewUtilizatori.Columns.Add("ID Utilizator", 100);
                    listViewUtilizatori.Columns.Add("Nume", 150);
                    listViewUtilizatori.Columns.Add("Prenume", 150);
                    listViewUtilizatori.Columns.Add("Email", 200);
                    listViewUtilizatori.Columns.Add("Rang", 100);

                    foreach (string linie in continutFisier)
                    {
                        string[] detaliiUtilizator = linie.Split(',');

                        if (detaliiUtilizator.Length >= 6)
                        {
                            ListViewItem item = new ListViewItem(detaliiUtilizator[0]);
                            item.SubItems.Add(detaliiUtilizator[1]);
                            item.SubItems.Add(detaliiUtilizator[2]);
                            item.SubItems.Add(detaliiUtilizator[3]);
                            item.SubItems.Add(detaliiUtilizator[5]);

                            listViewUtilizatori.Items.Add(item);
                        }
                    }

                    formVizualizare.Controls.Add(listViewUtilizatori);

                    formVizualizare.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare: {ex.Message}\n\nDetalii: {ex.StackTrace}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnModificareUtilizatori_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");

        }
    }
}