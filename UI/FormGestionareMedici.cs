using System;
using System.Windows.Forms;
using System.Drawing;
using LibrarieModele;
using NivelStocareDate;
using MetroFramework.Forms;
using MetroFramework.Controls;
using MetroFramework.Components;
using GestionareSpital;
using System.Collections.Generic;
using System.Linq;

namespace UI
{
    public partial class FormGestionareMedici : MetroForm
    {
        private FlowLayoutPanel panelMeniu;
        private MetroButton btnVizualizareMedici;
        private MetroButton btnVizualizareMediciSpecialitate;
        private MetroButton btnVizualizareMediciDepartament;
        private MetroButton btnInapoi;
        private MetroButton btnVizualizareMediciSearch;

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
            btnVizualizareMediciSearch.Click += BtnVizualizareMediciSearch_Click;
            this.utilizatorCurent = utilizatorCurent;
        }

        private void ConfigureazaMeniu()
        {
            panelMeniu.Controls.Clear();

            btnVizualizareMedici = new MetroButton { Text = "Vizualizare medici", Size = new Size(200, 40) };
            btnVizualizareMediciSpecialitate = new MetroButton { Text = "Vizualizare medici dupa specialitate", Size = new Size(200, 40) };
            btnVizualizareMediciDepartament = new MetroButton { Text = "Vizualizare medici dupa departament", Size = new Size(200, 40) };
            btnVizualizareMediciSearch = new MetroButton { Text = "Vizualizare medici dupa criteriu", Size = new Size(200, 40) };
            btnInapoi = new MetroButton { Text = "Inapoi", Size = new Size(200, 40) };

            panelMeniu.Controls.Add(btnVizualizareMedici);
            panelMeniu.Controls.Add(btnVizualizareMediciSpecialitate);
            panelMeniu.Controls.Add(btnVizualizareMediciDepartament);
            panelMeniu.Controls.Add(btnVizualizareMediciSearch);
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

        private void BtnVizualizareMediciSearch_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareMedici))
            {
                Medic[] medici = adminMedici.GetMedici(out int nrMedici);

                MetroForm formVizualizare = new MetroForm
                {
                    Text = "Lista Medici",
                    Size = new Size(1400, 600),
                    StartPosition = FormStartPosition.CenterScreen,
                    MaximizeBox = false,
                    MinimizeBox = true,
                    ControlBox = true,
                    Resizable = false,
                    Theme = MetroFramework.MetroThemeStyle.Light,
                    Style = MetroFramework.MetroColorStyle.Black
                };

                MetroPanel searchPanel = new MetroPanel
                {
                    Dock = DockStyle.Top,
                    Height = 130,
                    Padding = new Padding(10)
                };

                MetroButton btnCautareMedici = new MetroButton
                {
                    Text = "Cauta Medic",
                    Size = new Size(120, 30),
                    Location = new Point(1050, 35),
                    Theme = MetroFramework.MetroThemeStyle.Light
                };

                MetroTextBox txtCautare = new MetroTextBox
                {
                    Width = 250,
                    Location = new Point(780, 35),
                    PromptText = "Introduceti textul pentru cautare"
                };

                MetroLabel lblCautare = new MetroLabel
                {
                    Text = "Cauta dupa:",
                    Location = new Point(10, 10),
                    AutoSize = true,
                    FontWeight = MetroFramework.MetroLabelWeight.Bold
                };

                MetroCheckBox chkID = new MetroCheckBox
                {
                    Text = "ID",
                    Location = new Point(20, 35),
                    Checked = true
                };

                MetroCheckBox chkNume = new MetroCheckBox
                {
                    Text = "Nume",
                    Location = new Point(140, 35)
                };

                MetroCheckBox chkPrenume = new MetroCheckBox
                {
                    Text = "Prenume",
                    Location = new Point(260, 35)
                };

                MetroCheckBox chkSpecialitate = new MetroCheckBox
                {
                    Text = "Specialitate",
                    Location = new Point(380, 35)
                };

                MetroCheckBox chkIDDepartament = new MetroCheckBox
                {
                    Text = "ID Departament",
                    Location = new Point(500, 35)
                };

                MetroCheckBox chkNumeDepartament = new MetroCheckBox
                {
                    Text = "Nume Dep.",
                    Location = new Point(660, 35)
                };

                List<MetroCheckBox> criteriaCheckboxes = new List<MetroCheckBox>
        {
            chkID, chkNume, chkPrenume, chkSpecialitate, chkIDDepartament, chkNumeDepartament
        };

                foreach (var chk in criteriaCheckboxes)
                {
                    chk.CheckedChanged += (s, args) =>
                    {
                        if (((MetroCheckBox)s).Checked)
                        {
                            foreach (var otherChk in criteriaCheckboxes)
                            {
                                if (otherChk != s && otherChk.Checked)
                                {
                                    otherChk.Checked = false;
                                }
                            }
                        }
                        else
                        {
                            bool anyChecked = criteriaCheckboxes.Any(c => c.Checked);
                            if (!anyChecked)
                            {
                                ((MetroCheckBox)s).Checked = true;
                            }
                        }
                    };
                }

                MetroCheckBox chkCautareExacta = new MetroCheckBox
                {
                    Text = "Potrivire exacta",
                    Location = new Point(20, 65)
                };


                MetroButton btnResetare = new MetroButton
                {
                    Text = "Resetare",
                    Size = new Size(100, 30),
                    Location = new Point(20, 95),
                    Theme = MetroFramework.MetroThemeStyle.Light
                };

                MetroButton btnSortare = new MetroButton
                {
                    Text = "Sortare alfabetica",
                    Size = new Size(120, 30),
                    Location = new Point(150, 95),
                    Theme = MetroFramework.MetroThemeStyle.Light
                };

                searchPanel.Controls.AddRange(new Control[] {
            lblCautare,
            chkID, chkNume, chkPrenume, chkSpecialitate, chkIDDepartament, chkNumeDepartament,
            txtCautare, btnCautareMedici, chkCautareExacta,
            btnResetare, btnSortare
        });

                ListView listView = new ListView
                {
                    Dock = DockStyle.Fill,
                    View = View.Details,
                    FullRowSelect = true,
                    GridLines = true,
                    Font = new Font("Segoe UI", 10)
                };

                listView.Columns.Add("ID", 60);
                listView.Columns.Add("Nume", 150);
                listView.Columns.Add("Prenume", 150);
                listView.Columns.Add("Specialitate", 150);
                listView.Columns.Add("ID Departament", 100);
                listView.Columns.Add("Nume Departament", 180);

                MetroPanel mainPanel = new MetroPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true
                };

                PopulateMediciListView(listView, medici);
                mainPanel.Controls.Add(listView);

                formVizualizare.Controls.Add(mainPanel);
                formVizualizare.Controls.Add(searchPanel);

                btnResetare.Click += (s, args) =>
                {
                    txtCautare.Text = string.Empty;
                    chkCautareExacta.Checked = false;

                    chkID.Checked = true;
                    chkNume.Checked = false;
                    chkPrenume.Checked = false;
                    chkSpecialitate.Checked = false;
                    chkIDDepartament.Checked = false;
                    chkNumeDepartament.Checked = false;

                    PopulateMediciListView(listView, medici);
                };

                btnSortare.Click += (s, args) =>
                {
                    var displayedMedici = GetDisplayedMedici(listView, medici);

                    var sortedMedici = displayedMedici.OrderBy(m => m.Nume).ToArray();

                    PopulateMediciListView(listView, sortedMedici);
                };

                btnCautareMedici.Click += (s, args) =>
                {
                    string searchText = txtCautare.Text.Trim();
                    bool exactMatch = chkCautareExacta.Checked;


                    int departmentId = 0;
                    string departmentName = "";


                    string searchCriteria = "";
                    if (chkID.Checked) searchCriteria = "ID";
                    else if (chkNume.Checked) searchCriteria = "Nume";
                    else if (chkPrenume.Checked) searchCriteria = "Prenume";
                    else if (chkSpecialitate.Checked) searchCriteria = "Specialitate";
                    else if (chkIDDepartament.Checked) searchCriteria = "IDDepartament";
                    else if (chkNumeDepartament.Checked) searchCriteria = "NumeDepartament";

                    Medic[] filteredMedici = SearchMedici(
                        medici,
                        searchCriteria,
                        searchText,
                        exactMatch,
                        departmentId,
                        departmentName
                    );

                    PopulateMediciListView(listView, filteredMedici);
                };

                formVizualizare.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a vizualiza medici.");
            }
        }

        private void PopulateMediciListView(ListView listView, Medic[] medici)
        {
            listView.Items.Clear();

            if (medici == null || medici.Length == 0)
            {
                MessageBox.Show("Nu au fost gasiti medici care sa corespunda criteriilor de cautare.",
                    "Cautare", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var medic in medici)
            {
                Departament departament = adminDepartamente.GetDepartamentDupaId(medic.IdDepartament);
                string numeDepartament = departament != null ? departament.Nume : "Necunoscut";

                ListViewItem item = new ListViewItem(medic.IdUser.ToString());
                item.SubItems.Add(medic.Nume);
                item.SubItems.Add(medic.Prenume);
                item.SubItems.Add(medic.Specialitate);
                item.SubItems.Add(medic.IdDepartament.ToString());
                item.SubItems.Add(numeDepartament);

                listView.Items.Add(item);
            }
        }

        private Medic[] GetDisplayedMedici(ListView listView, Medic[] allMedici)
        {
            List<Medic> displayedMedici = new List<Medic>();

            foreach (ListViewItem item in listView.Items)
            {
                int idMedic = int.Parse(item.SubItems[0].Text);
                var medic = Array.Find(allMedici, m => m.IdUser == idMedic);
                if (medic != null)
                {
                    displayedMedici.Add(medic);
                }
            }

            return displayedMedici.ToArray();
        }

        private Medic[] SearchMedici(
     Medic[] allMedici,
     string searchCriteria,
     string searchText,
     bool exactMatch,
     int departmentId,
     string departmentName)
        {
            bool filterByDepartmentId = departmentId != -1;
            bool filterByDepartmentName = !string.IsNullOrWhiteSpace(departmentName);

            if (string.IsNullOrWhiteSpace(searchText))
                return Array.Empty<Medic>();
            List<Medic> results = new List<Medic>();
            StringComparison comparison = exactMatch ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            foreach (var medic in allMedici)
            {
                bool isMatch = false;

                if (filterByDepartmentId && medic.IdDepartament != departmentId)
                    continue;

                if (filterByDepartmentName)
                {
                    Departament departament = adminDepartamente.GetDepartamentDupaId(medic.IdDepartament);
                    string numeDep = departament != null ? departament.Nume : "Necunoscut";

                    bool departmentNameMatch = exactMatch
                        ? numeDep.Equals(departmentName, comparison)
                        : numeDep.Contains(departmentName, comparison);

                    if (!departmentNameMatch)
                        continue;
                }

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    isMatch = true;
                }
                else
                {
                    switch (searchCriteria)
                    {
                        case "ID":
                            isMatch = exactMatch
                                ? medic.IdUser.ToString().Equals(searchText, comparison)
                                : medic.IdUser.ToString().Contains(searchText, comparison);
                            break;
                        case "Nume":
                            isMatch = exactMatch
                                ? medic.Nume.Equals(searchText, comparison)
                                : medic.Nume.Contains(searchText, comparison);
                            break;
                        case "Prenume":
                            isMatch = exactMatch
                                ? medic.Prenume.Equals(searchText, comparison)
                                : medic.Prenume.Contains(searchText, comparison);
                            break;
                        case "Specialitate":
                            isMatch = exactMatch
                                ? medic.Specialitate.Equals(searchText, comparison)
                                : medic.Specialitate.Contains(searchText, comparison);
                            break;
                        case "IDDepartament":
                            isMatch = exactMatch
                                ? medic.IdDepartament.ToString().Equals(searchText, comparison)
                                : medic.IdDepartament.ToString().Contains(searchText, comparison);
                            break;
                        case "NumeDepartament":
                            Departament dept = adminDepartamente.GetDepartamentDupaId(medic.IdDepartament);
                            string numeDepartament = dept != null ? dept.Nume : "Necunoscut";
                            isMatch = exactMatch
                                ? numeDepartament.Equals(searchText, comparison)
                                : numeDepartament.Contains(searchText, comparison);
                            break;
                    }
                }

                if (isMatch)
                    results.Add(medic);
            }

            return results.ToArray();
        }

    }
}
