using System;
using System.Windows.Forms;
using System.Drawing;
using LibrarieModele;
using NivelStocareDate;
using MetroFramework.Forms;
using MetroFramework.Controls;
using MetroFramework.Components;
using GestionareSpital;

namespace UI
{
    public partial class FormGestionareMedici : MetroForm
    {
        private FlowLayoutPanel panelMeniu;
        private MetroButton btnVizualizareMedici;
        private MetroButton btnVizualizareMediciSpecialitate;
        private MetroButton btnVizualizareMediciDepartament;
        private MetroButton btnInapoi;

        private AdministrareMedici_FisierText adminMedici;
        private AdministrareDepartamente_FisierText adminDepartamente;

        private MetroStyleManager metroStyleManager1;
        private User utilizatorCurent;

        public FormGestionareMedici(AdministrareMedici_FisierText adminMedici, AdministrareDepartamente_FisierText adminDepartamente, User utilizatorCurent)
        {
            InitializeComponent();
            this.adminMedici = adminMedici;
            this.adminDepartamente = adminDepartamente;

            this.StartPosition = FormStartPosition.CenterScreen;

            this.metroStyleManager1 = new MetroStyleManager();
            this.metroStyleManager1.Owner = this;
            this.metroStyleManager1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroStyleManager1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.utilizatorCurent = utilizatorCurent;

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
            this.utilizatorCurent = utilizatorCurent;
        }

        private void ConfigureazaMeniu()
        {
            panelMeniu.Controls.Clear();

            btnVizualizareMedici = new MetroButton { Text = "Vizualizare medici", Size = new Size(200, 40) };
            btnVizualizareMediciSpecialitate = new MetroButton { Text = "Vizualizare medici dupa specialitate", Size = new Size(200, 40) };
            btnVizualizareMediciDepartament = new MetroButton { Text = "Vizualizare medici dupa departament", Size = new Size(200, 40) };
            btnInapoi = new MetroButton { Text = "Inapoi", Size = new Size(200, 40) };

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
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareMedici))
            {
                Medic[] medici = adminMedici.GetMedici(out int nrMedici);
                using (MetroForm formVizualizare = new MetroForm())
                {
                    formVizualizare.Text = "Lista Medici";
                    formVizualizare.Size = new Size(700, 400);
                    formVizualizare.StartPosition = FormStartPosition.CenterScreen;
                    formVizualizare.Style = MetroFramework.MetroColorStyle.Black;

                    ListView listView = new ListView
                    {
                        Dock = DockStyle.Fill,
                        View = View.Details,
                        FullRowSelect = true
                    };

                    listView.Columns.Add("ID", 50);
                    listView.Columns.Add("Nume", 150);
                    listView.Columns.Add("Prenume", 150);
                    listView.Columns.Add("Specialitate", 150);
                    listView.Columns.Add("Departament", 150);

                    foreach (var medic in medici)
                    {
                        Departament departament = adminDepartamente.GetDepartamentDupaId(medic.IdDepartament);
                        string numeDepartament = departament != null ? departament.Nume : "Necunoscut";

                        ListViewItem item = new ListViewItem(medic.IdUser.ToString());
                        item.SubItems.Add(medic.Nume);
                        item.SubItems.Add(medic.Prenume);
                        item.SubItems.Add(medic.Specialitate);
                        item.SubItems.Add(numeDepartament);

                        listView.Items.Add(item);
                    }

                    formVizualizare.Controls.Add(listView);
                    formVizualizare.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a vizualiza medici.");
            }
        }

        private void BtnVizualizareMediciSpecialitate_Click(object sender, EventArgs e)
        {
            string specialitate = Microsoft.VisualBasic.Interaction.InputBox("Introduceti specialitatea:", "Specialitate");
            Medic[] mediciSpecialitate = adminMedici.GetMediciDupaSpecialitate(specialitate);
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareMedici))
            {
                if (mediciSpecialitate != null && mediciSpecialitate.Length > 0)
                {
                    using (MetroForm formVizualizare = new MetroForm())
                    {
                        formVizualizare.Text = $"Medici cu specialitatea {specialitate}";
                        formVizualizare.Size = new Size(700, 400);
                        formVizualizare.StartPosition = FormStartPosition.CenterScreen;
                        formVizualizare.Style = MetroFramework.MetroColorStyle.Black;

                        ListView listView = new ListView
                        {
                            Dock = DockStyle.Fill,
                            View = View.Details,
                            FullRowSelect = true
                        };

                        listView.Columns.Add("ID", 50);
                        listView.Columns.Add("Nume", 150);
                        listView.Columns.Add("Prenume", 150);
                        listView.Columns.Add("Specialitate", 150);
                        listView.Columns.Add("Departament", 150);

                        foreach (var medic in mediciSpecialitate)
                        {
                            Departament departament = adminDepartamente.GetDepartamentDupaId(medic.IdDepartament);
                            string numeDepartament = departament != null ? departament.Nume : "Necunoscut";

                            ListViewItem item = new ListViewItem(medic.IdUser.ToString());
                            item.SubItems.Add(medic.Nume);
                            item.SubItems.Add(medic.Prenume);
                            item.SubItems.Add(medic.Specialitate);
                            item.SubItems.Add(numeDepartament);

                            listView.Items.Add(item);
                        }

                        formVizualizare.Controls.Add(listView);
                        formVizualizare.ShowDialog();
                    }
                }

                else
                {
                    MessageBox.Show(this, $"Nu exista medici cu specialitatea {specialitate}!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a vizualiza medici.");
            }
        }

        private void BtnVizualizareMediciDepartament_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Introduceti ID-ul departamentului:", "Departament");
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.StergerePacienti))
            {
                if (int.TryParse(input, out int idDepartament))
                {
                    Medic[] mediciDepartament = adminMedici.GetMediciDupaDepartament(idDepartament);
                    Departament departament = adminDepartamente.GetDepartamentDupaId(idDepartament);
                    string numeDepartament = departament != null ? departament.Nume : "Necunoscut";

                    if (mediciDepartament != null && mediciDepartament.Length > 0)
                    {
                        using (MetroForm formVizualizare = new MetroForm())
                        {
                            formVizualizare.Text = $"Medici din departamentul {numeDepartament}";
                            formVizualizare.Style = MetroFramework.MetroColorStyle.Black;
                            formVizualizare.Size = new Size(800, 400);
                            formVizualizare.StartPosition = FormStartPosition.CenterScreen;

                            ListView listView = new ListView
                            {
                                Dock = DockStyle.Fill,
                                View = View.Details,
                                FullRowSelect = true
                            };

                            listView.Columns.Add("ID", 50);
                            listView.Columns.Add("Nume", 150);
                            listView.Columns.Add("Prenume", 150);
                            listView.Columns.Add("Specialitate", 150);
                            listView.Columns.Add("Departament", 150);

                            foreach (var medic in mediciDepartament)
                            {
                                ListViewItem item = new ListViewItem(medic.IdUser.ToString());
                                item.SubItems.Add(medic.Nume);
                                item.SubItems.Add(medic.Prenume);
                                item.SubItems.Add(medic.Specialitate);
                                item.SubItems.Add(numeDepartament);

                                listView.Items.Add(item);
                            }

                            formVizualizare.Controls.Add(listView);
                            formVizualizare.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, $"Nu exista medici in departamentul {numeDepartament}!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(this, "ID-ul departamentului este invalid!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a vizualiza medici.");
            }
        }
    }
}
