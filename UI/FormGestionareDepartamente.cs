﻿using MetroFramework.Forms;
using MetroFramework.Controls;
using System;
using System.Drawing;
using System.IO;
using LibrarieModele;
using NivelStocareDate;
using System.Windows.Forms;
using MetroFramework;
using System.Text.RegularExpressions;
using GestionareSpital;
using System.Collections.Generic;

namespace UI
{
    public static class ConstanteValidare
    {
        public const int MAXIM_CARACTERE_NUME_DEPARTAMENT = 50;
        public const int MINIM_CARACTERE_NUME_DEPARTAMENT = 3;
        public const int MAXIM_CARACTERE_DESCRIERE_DEPARTAMENT = 500;
        public const int MAXIM_CARACTERE_LOCATIE_DEPARTAMENT = 100;
        public const int MINIM_CARACTERE_LOCATIE_DEPARTAMENT = 5;
        public const string PATTERN_LOCATIE_DEPARTAMENT = @"^[A-Za-z0-9\s\.,\-]{5,100}$";

        public const string MESAJ_ERROARE_NUME_OBLIGATORIU = "Numele departamentului este obligatoriu!";
        public const string MESAJ_ERROARE_NUME_MINIM_CARACTERE = "Numele departamentului trebuie sa contina minim 3 caractere!";
        public const string MESAJ_ERROARE_NUME_MAXIM_CARACTERE = "Numele departamentului nu poate depasi 50 caractere!";
        public const string MESAJ_ERROARE_DESCRIERE_MAXIM_CARACTERE = "Descrierea departamentului nu poate depasi 500 caractere!";
        public const string MESAJ_ERROARE_LOCATIE_OBLIGATORIE = "Locatia departamentului este obligatorie!";
        public const string MESAJ_ERROARE_LOCATIE_MINIM_CARACTERE = "Locatia departamentului trebuie sa contina minim 5 caractere!";
        public const string MESAJ_ERROARE_LOCATIE_MAXIM_CARACTERE = "Locatia departamentului nu poate depasi 100 caractere!";
        public const string MESAJ_ERROARE_LOCATIE_FORMAT_INVALID = "Formatul locatiei este invalid! Exemplu corect: Cladire A, Etaj 2";

        public const string MESAJ_ERROARE_NUME_DUPLICAT = "Exista deja un departament cu acest nume!";
        public const string MESAJ_ERROARE_LOCATIE_DUPLICAT = "Exista deja un departament cu aceasta locatie!";
    }

    public class DepartamentValidator
    {
        private readonly TextBox txtNumeDepartament;
        private readonly TextBox txtDescriereDepartament;
        private readonly TextBox txtLocatieDepartament;
        private readonly Label lblNumeDepartament;
        private readonly Label lblDescriereDepartament;
        private readonly Label lblLocatieDepartament;
        private readonly Label lblMesajEroare;
        private readonly AdministrareDepartamente_FisierText adminDepartamente;

        public DepartamentValidator(
            TextBox txtNumeDepartament,
            TextBox txtDescriereDepartament,
            TextBox txtLocatieDepartament,
            Label lblNumeDepartament,
            Label lblDescriereDepartament,
            Label lblLocatieDepartament,
            Label lblMesajEroare,
            AdministrareDepartamente_FisierText adminDepartamente)
        {
            this.txtNumeDepartament = txtNumeDepartament;
            this.txtDescriereDepartament = txtDescriereDepartament;
            this.txtLocatieDepartament = txtLocatieDepartament;
            this.lblNumeDepartament = lblNumeDepartament;
            this.lblDescriereDepartament = lblDescriereDepartament;
            this.lblLocatieDepartament = lblLocatieDepartament;
            this.lblMesajEroare = lblMesajEroare;
            this.adminDepartamente = adminDepartamente;
        }

        public bool Valideaza(out string mesajEroare, int? idDepartamentCurent = null)
        {
            ResetValidare();

            if (string.IsNullOrWhiteSpace(txtNumeDepartament.Text))
            {
                MarcareCampInvalid(txtNumeDepartament, lblNumeDepartament);
                mesajEroare = ConstanteValidare.MESAJ_ERROARE_NUME_OBLIGATORIU;
                return false;
            }
            else if (txtNumeDepartament.Text.Length < ConstanteValidare.MINIM_CARACTERE_NUME_DEPARTAMENT)
            {
                MarcareCampInvalid(txtNumeDepartament, lblNumeDepartament);
                mesajEroare = ConstanteValidare.MESAJ_ERROARE_NUME_MINIM_CARACTERE;
                return false;
            }
            else if (txtNumeDepartament.Text.Length > ConstanteValidare.MAXIM_CARACTERE_NUME_DEPARTAMENT)
            {
                MarcareCampInvalid(txtNumeDepartament, lblNumeDepartament);
                mesajEroare = ConstanteValidare.MESAJ_ERROARE_NUME_MAXIM_CARACTERE;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtDescriereDepartament.Text) &&
                txtDescriereDepartament.Text.Length > ConstanteValidare.MAXIM_CARACTERE_DESCRIERE_DEPARTAMENT)
            {
                MarcareCampInvalid(txtDescriereDepartament, lblDescriereDepartament);
                mesajEroare = ConstanteValidare.MESAJ_ERROARE_DESCRIERE_MAXIM_CARACTERE;
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLocatieDepartament.Text))
            {
                MarcareCampInvalid(txtLocatieDepartament, lblLocatieDepartament);
                mesajEroare = ConstanteValidare.MESAJ_ERROARE_LOCATIE_OBLIGATORIE;
                return false;
            }
            else if (txtLocatieDepartament.Text.Length < ConstanteValidare.MINIM_CARACTERE_LOCATIE_DEPARTAMENT)
            {
                MarcareCampInvalid(txtLocatieDepartament, lblLocatieDepartament);
                mesajEroare = ConstanteValidare.MESAJ_ERROARE_LOCATIE_MINIM_CARACTERE;
                return false;
            }
            else if (txtLocatieDepartament.Text.Length > ConstanteValidare.MAXIM_CARACTERE_LOCATIE_DEPARTAMENT)
            {
                MarcareCampInvalid(txtLocatieDepartament, lblLocatieDepartament);
                mesajEroare = ConstanteValidare.MESAJ_ERROARE_LOCATIE_MAXIM_CARACTERE;
                return false;
            }
            else if (!Regex.IsMatch(txtLocatieDepartament.Text, ConstanteValidare.PATTERN_LOCATIE_DEPARTAMENT))
            {
                MarcareCampInvalid(txtLocatieDepartament, lblLocatieDepartament);
                mesajEroare = ConstanteValidare.MESAJ_ERROARE_LOCATIE_FORMAT_INVALID;
                return false;
            }

            if (!VerificaDuplicateNume(txtNumeDepartament.Text, idDepartamentCurent, out mesajEroare))
            {
                MarcareCampInvalid(txtNumeDepartament, lblNumeDepartament);
                return false;
            }

            if (!VerificaDuplicateLocatie(txtLocatieDepartament.Text, idDepartamentCurent, out mesajEroare))
            {
                MarcareCampInvalid(txtLocatieDepartament, lblLocatieDepartament);
                return false;
            }

            mesajEroare = string.Empty;
            return true;
        }

        public bool VerificaDuplicateNume(string nume, int? idDepartamentCurent, out string mesajEroare)
        {
            Departament[] departamente = adminDepartamente.GetDepartamente(out int nrDepartamente);

            foreach (var dept in departamente)
            {
                if (idDepartamentCurent.HasValue && dept.IdDepartament == idDepartamentCurent.Value)
                    continue;

                if (dept.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase))
                {
                    mesajEroare = ConstanteValidare.MESAJ_ERROARE_NUME_DUPLICAT;
                    return false;
                }
            }

            mesajEroare = string.Empty;
            return true;
        }

        public bool VerificaDuplicateLocatie(string locatie, int? idDepartamentCurent, out string mesajEroare)
        {
            Departament[] departamente = adminDepartamente.GetDepartamente(out int nrDepartamente);

            foreach (var dept in departamente)
            {
                if (idDepartamentCurent.HasValue && dept.IdDepartament == idDepartamentCurent.Value)
                    continue;

                if (dept.Locatie.Equals(locatie, StringComparison.OrdinalIgnoreCase))
                {
                    mesajEroare = ConstanteValidare.MESAJ_ERROARE_LOCATIE_DUPLICAT;
                    return false;
                }
            }

            mesajEroare = string.Empty;
            return true;
        }

        private void ResetValidare()
        {
            lblNumeDepartament.ForeColor = SystemColors.ControlText;
            lblDescriereDepartament.ForeColor = SystemColors.ControlText;
            lblLocatieDepartament.ForeColor = SystemColors.ControlText;

            txtNumeDepartament.BackColor = SystemColors.Window;
            txtDescriereDepartament.BackColor = SystemColors.Window;
            txtLocatieDepartament.BackColor = SystemColors.Window;

            lblMesajEroare.Visible = false;
        }

        private void MarcareCampInvalid(TextBox textBox, Label label)
        {
            label.ForeColor = Color.Red;
            textBox.BackColor = Color.LightPink;
            lblMesajEroare.Visible = true;
        }

        public void AfiseazaMesajEroare(string mesaj)
        {
            lblMesajEroare.Text = mesaj;
            lblMesajEroare.Visible = true;
        }
    }


    public class FormularDepartamentInitializer
    {
        private readonly Panel panelFormular;
        private readonly int labelX = 10;
        private readonly int inputX = 170;
        private readonly int controlWidth = 300;
        private readonly int labelWidth = 150;

        public MetroLabel LblTitluFormular { get; private set; }
        public MetroLabel LblNumeDepartament { get; private set; }
        public MetroLabel LblDescriereDepartament { get; private set; }
        public MetroLabel LblLocatieDepartament { get; private set; }
        public TextBox TxtNumeDepartament { get; private set; }
        public TextBox TxtDescriereDepartament { get; private set; }
        public TextBox TxtLocatieDepartament { get; private set; }
        public MetroLabel LblMesajEroare { get; private set; }
        public MetroButton BtnSalveazaDepartament { get; private set; }

        public FormularDepartamentInitializer(Panel panelFormular)
        {
            this.panelFormular = panelFormular;
        }

        public void InitializeazaControale(string titluFormular = "Adaugare departament nou", string textButon = "Salveaza Departamentul")
        {
            CreazaEtichete(titluFormular);
            CreazaCampuriText();
            CreazaButoane(textButon);
            AdaugaControleLaPanel();
        }

        private void CreazaEtichete(string titluFormular)
        {
            LblTitluFormular = new MetroLabel
            {
                Text = titluFormular,
                Location = new Point(10, 10),
                Width = 460,
                FontSize = MetroLabelSize.Tall,
                FontWeight = MetroLabelWeight.Bold,
                Name = "lblTitluFormular"
            };

            LblNumeDepartament = new MetroLabel
            {
                Text = "Nume Departament *:",
                Location = new Point(labelX, 50),
                Width = labelWidth,
                Name = "lblNumeDepartament"
            };

            LblDescriereDepartament = new MetroLabel
            {
                Text = "Descriere Departament:",
                Location = new Point(labelX, 80),
                Width = labelWidth,
                Name = "lblDescriereDepartament"
            };

            LblLocatieDepartament = new MetroLabel
            {
                Text = "Locatie Departament *:",
                Location = new Point(labelX, 150),
                Width = labelWidth,
                Name = "lblLocatieDepartament"
            };

            LblMesajEroare = new MetroLabel
            {
                Location = new Point(10, 210),
                Width = 460,
                Height = 40,
                ForeColor = Color.Red,
                Visible = false,
                Name = "lblMesajEroare"
            };
        }

        private void CreazaCampuriText()
        {
            TxtNumeDepartament = new TextBox
            {
                Location = new Point(inputX, 50),
                Width = controlWidth,
                Name = "txtNumeDepartament"
            };

            TxtDescriereDepartament = new TextBox
            {
                Location = new Point(inputX, 80),
                Width = controlWidth,
                Height = 60,
                Multiline = true,
                Name = "txtDescriereDepartament"
            };

            TxtLocatieDepartament = new TextBox
            {
                Location = new Point(inputX, 150),
                Width = controlWidth,
                Name = "txtLocatieDepartament"
            };
        }

        private void CreazaButoane(string textButon)
        {
            BtnSalveazaDepartament = new MetroButton
            {
                Text = textButon,
                Location = new Point(10, 260),
                Width = 460,
                Height = 40,
                Name = "btnSalveazaDepartament"
            };
        }

        private void AdaugaControleLaPanel()
        {
            panelFormular.Controls.Add(LblTitluFormular);
            panelFormular.Controls.Add(LblNumeDepartament);
            panelFormular.Controls.Add(LblDescriereDepartament);
            panelFormular.Controls.Add(LblLocatieDepartament);
            panelFormular.Controls.Add(TxtNumeDepartament);
            panelFormular.Controls.Add(TxtDescriereDepartament);
            panelFormular.Controls.Add(TxtLocatieDepartament);
            panelFormular.Controls.Add(LblMesajEroare);
            panelFormular.Controls.Add(BtnSalveazaDepartament);
        }

        public void CompletareCampuri(Departament departament)
        {
            if (departament == null) return;

            TxtNumeDepartament.Text = departament.Nume;
            TxtDescriereDepartament.Text = departament.Descriere;
            TxtLocatieDepartament.Text = departament.Locatie;
        }
    }



    public partial class FormGestionareDepartamente : MetroForm
    {
        private AdministrareDepartamente_FisierText adminDepartamente;
        private Label[] eticheteCampuri;
        private User utilizatorCurent;

        public FormGestionareDepartamente(AdministrareDepartamente_FisierText departamenteAdmin, User utilizatorCurent)
        {
            this.utilizatorCurent = utilizatorCurent;
            adminDepartamente = departamenteAdmin;
            InitializeComponent();
            ConfigureazaMeniu();

        }

        private void ConfigureazaMeniu()
        {
            this.Text = "Gestionare Departamente";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Style = MetroColorStyle.Black;
            this.Theme = MetroThemeStyle.Light;

            FlowLayoutPanel panelMeniu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
            };

            MetroButton btnAdaugareDepartament = new MetroButton
            {
                Text = "Adaugare departament nou",
                Size = new Size(250, 40),
            };

            MetroButton btnVizualizareDepartamente = new MetroButton
            {
                Text = "Vizualizare toate departamentele",
                Size = new Size(250, 40),
            };

            MetroButton btnModificareDepartament = new MetroButton
            {
                Text = "Modificare departament",
                Size = new Size(250, 40),
            };
            MetroButton btnCautareDepartament = new MetroButton
            {
                Text = "Cautare departamente",
                Size = new Size(250, 40),
            };

            MetroButton btnInchide = new MetroButton
            {
                Text = "Inchide",
                Size = new Size(250, 40),
            };

            btnAdaugareDepartament.Click += BtnAdaugareDepartament_Click;
            btnVizualizareDepartamente.Click += BtnVizualizareDepartamente_Click;
            btnModificareDepartament.Click += BtnModificareDepartament_Click;
            btnCautareDepartament.Click += BtnCautareDepartament_Click;

            btnInchide.Click += (s, e) => this.Close();

            panelMeniu.Controls.Add(btnAdaugareDepartament);
            panelMeniu.Controls.Add(btnVizualizareDepartamente);
            panelMeniu.Controls.Add(btnModificareDepartament);
            panelMeniu.Controls.Add(btnCautareDepartament);

            panelMeniu.Controls.Add(btnInchide);

            this.Controls.Add(panelMeniu);
        }


        private void BtnAdaugareDepartament_Click(object sender, EventArgs e)
        {
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisierDepartamente = Path.Combine(locatieFisierSolutie, "Departamente.txt");
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugareDepartamente))
            {
                using (Form formDepartament = new Form())
                {
                    formDepartament.Text = "Departament Nou";
                    formDepartament.StartPosition = FormStartPosition.CenterScreen;
                    formDepartament.FormBorderStyle = FormBorderStyle.FixedDialog;
                    formDepartament.MaximizeBox = false;
                    formDepartament.MinimizeBox = false;
                    formDepartament.Size = new Size(800, 450);
                    formDepartament.BackColor = Color.White;

                    Panel panelFormular = new Panel
                    {
                        Dock = DockStyle.Fill,
                        AutoScroll = true,
                        Name = "panelFormular"
                    };
                    formDepartament.Controls.Add(panelFormular);

                    var initializer = new FormularDepartamentInitializer(panelFormular);
                    initializer.InitializeazaControale("Adaugare departament nou", "Salveaza Departamentul");

                    var validator = new DepartamentValidator(
                            initializer.TxtNumeDepartament,
                            initializer.TxtDescriereDepartament,
                            initializer.TxtLocatieDepartament,
                            initializer.LblNumeDepartament,
                            initializer.LblDescriereDepartament,
                            initializer.LblLocatieDepartament,
                            initializer.LblMesajEroare,
                            adminDepartamente
                        );

                    initializer.BtnSalveazaDepartament.Click += (s, ev) =>
                    {
                        if (validator.Valideaza(out string mesajEroare))
                        {
                            try
                            {
                                Departament[] departamenteExistente = adminDepartamente.GetDepartamente(out int nrDepartamente);
                                int nuoulId = nrDepartamente > 0 ? departamenteExistente[nrDepartamente - 1].IdDepartament + 1 : 1;

                                Departament departamentNou = new Departament
                                {
                                    IdDepartament = nuoulId,
                                    Nume = initializer.TxtNumeDepartament.Text.Trim(),
                                    Descriere = initializer.TxtDescriereDepartament.Text.Trim(),
                                    Locatie = initializer.TxtLocatieDepartament.Text.Trim(),
                                };

                                adminDepartamente.AddDepartament(departamentNou);
                                MessageBox.Show($"Departamentul '{departamentNou.Nume}' a fost adaugat cu succes!",
                                    "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                formDepartament.DialogResult = DialogResult.OK;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Eroare la adaugarea departamentului: {ex.Message}",
                                    "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            validator.AfiseazaMesajEroare(mesajEroare);
                        }
                    };


                    formDepartament.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a adauga departamente.");
            }

        }

        private void BtnVizualizareDepartamente_Click(object sender, EventArgs e)
        {
            Departament[] departamente = adminDepartamente.GetDepartamente(out int nrDepartamente);
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareDepartamente))
            {
                using (Form formVizualizare = new Form())
                {
                    formVizualizare.Text = "Toate Departamentele";
                    formVizualizare.Size = new Size(600, 400);
                    formVizualizare.StartPosition = FormStartPosition.CenterScreen;

                    ListView listView = new ListView
                    {
                        Dock = DockStyle.Fill,
                        View = View.Details,
                        FullRowSelect = true
                    };

                    listView.Columns.Add("ID", 50);
                    listView.Columns.Add("Nume", 150);
                    listView.Columns.Add("Descriere", 200);
                    listView.Columns.Add("Locatie", 100);

                    foreach (var departament in departamente)
                    {
                        var item = new ListViewItem(departament.IdDepartament.ToString());
                        item.SubItems.Add(departament.Nume);
                        item.SubItems.Add(departament.Descriere);
                        item.SubItems.Add(departament.Locatie);
                        listView.Items.Add(item);
                    }

                    formVizualizare.Controls.Add(listView);
                    formVizualizare.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a vizualiza departamentele.");
            }
        }

        private void BtnModificareDepartament_Click(object sender, EventArgs e)
        {
            Departament[] departamente = adminDepartamente.GetDepartamente(out int nrDepartamente);
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.ModificareDepartamente))
            {
                using (MetroForm formSelectieDepartament = new MetroForm())
                {
                    formSelectieDepartament.Text = "Selecteaza Departament";
                    formSelectieDepartament.Size = new Size(800, 400);
                    formSelectieDepartament.StartPosition = FormStartPosition.CenterScreen;
                    formSelectieDepartament.Style = MetroFramework.MetroColorStyle.Black;

                    ListBox listDepartamente = new ListBox
                    {
                        Dock = DockStyle.Fill
                    };

                    foreach (var departament in departamente)
                    {
                        listDepartamente.Items.Add($"{departament.IdDepartament} - {departament.Nume}");
                    }

                    MetroButton btnModifica = new MetroButton
                    {
                        Text = "Modifica Departament",
                        Dock = DockStyle.Bottom,
                    };

                    btnModifica.Click += (s, ev) =>
                    {
                        if (listDepartamente.SelectedItem != null)
                        {
                            int idDepartamentSelectat = int.Parse(listDepartamente.SelectedItem.ToString().Split('-')[0].Trim());
                            Departament departamentSelectat = adminDepartamente.GetDepartamentDupaId(idDepartamentSelectat);

                            using (MetroForm formModificare = new MetroForm())
                            {
                                formModificare.Text = "Modificare Departament";
                                formModificare.Size = new Size(800, 400);
                                formModificare.StartPosition = FormStartPosition.CenterScreen;
                                formModificare.Style = MetroColorStyle.Black;

                                Panel panelFormular = new Panel
                                {
                                    Dock = DockStyle.Fill,
                                    Padding = new Padding(10)
                                };
                                formModificare.Controls.Add(panelFormular);

                                FormularDepartamentInitializer initializator = new FormularDepartamentInitializer(panelFormular);
                                initializator.InitializeazaControale("Modificare departament", "Salveaza Modificari");

                                initializator.CompletareCampuri(departamentSelectat);

                                MetroLabel lblMesajEroare = initializator.LblMesajEroare;
                                var validator = new DepartamentValidator(
                                        initializator.TxtNumeDepartament,
                                        initializator.TxtDescriereDepartament,
                                        initializator.TxtLocatieDepartament,
                                        initializator.LblNumeDepartament,
                                        initializator.LblDescriereDepartament,
                                        initializator.LblLocatieDepartament,
                                        initializator.LblMesajEroare,
                                        adminDepartamente
                                    );


                                initializator.BtnSalveazaDepartament.Click += (modS, modEv) =>
                                {
                                    string mesajEroare;
                                    if (validator.Valideaza(out mesajEroare))
                                    {
                                        Departament departamentModificat = new Departament
                                        {
                                            IdDepartament = departamentSelectat.IdDepartament,
                                            Nume = initializator.TxtNumeDepartament.Text,
                                            Descriere = initializator.TxtDescriereDepartament.Text,
                                            Locatie = initializator.TxtLocatieDepartament.Text
                                        };

                                        adminDepartamente.UpdateDepartament(departamentModificat);
                                        MessageBox.Show("Departament modificat cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        formModificare.DialogResult = DialogResult.OK;
                                    }
                                    else
                                    {
                                        validator.AfiseazaMesajEroare(mesajEroare);
                                    }
                                };

                                formModificare.ShowDialog();
                            }

                            formSelectieDepartament.DialogResult = DialogResult.OK;
                        }
                    };

                    formSelectieDepartament.Controls.Add(listDepartamente);
                    formSelectieDepartament.Controls.Add(btnModifica);

                    formSelectieDepartament.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a modifica departamentele.");
            }

        }


        private void BtnCautareDepartament_Click(object sender, EventArgs e)
        {
            if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareDepartamente))
            {
                using (MetroForm formCautare = new MetroForm())
                {
                    formCautare.Text = "Cautare Departamente";
                    formCautare.Size = new Size(600, 500);
                    formCautare.StartPosition = FormStartPosition.CenterScreen;
                    formCautare.Theme = MetroThemeStyle.Light;
                    formCautare.Style = MetroColorStyle.Black;

                    MetroPanel panelOptiuni = new MetroPanel
                    {
                        Dock = DockStyle.Top,
                        Height = 200,
                        AutoScroll = true,
                        Padding = new Padding(10)
                    };

                    MetroLabel lblTitlu = new MetroLabel
                    {
                        Text = "Selectati criteriul de cautare:",
                        FontWeight = MetroLabelWeight.Bold,
                        Location = new Point(10, 10),
                        Size = new Size(400, 30)
                    };
                    panelOptiuni.Controls.Add(lblTitlu);

                    GroupBox groupTipCautare = new GroupBox
                    {
                        Text = "Tip de cautare",
                        Location = new Point(10, 40),
                        Size = new Size(250, 120)
                    };

                    RadioButton radioExact = new RadioButton
                    {
                        Text = "Potrivire exacta",
                        Location = new Point(10, 20),
                        Checked = true
                    };
                    RadioButton radioPartial = new RadioButton
                    {
                        Text = "Potrivire partiala",
                        Location = new Point(10, 45)
                    };

                    groupTipCautare.Controls.Add(radioExact);
                    groupTipCautare.Controls.Add(radioPartial);
                    panelOptiuni.Controls.Add(groupTipCautare);

                    GroupBox groupCampuri = new GroupBox
                    {
                        Text = "Criteriu de cautare",
                        Location = new Point(270, 40),
                        Size = new Size(300, 120)
                    };

                    RadioButton radioIdDepartament = new RadioButton { Text = "ID Departament", Location = new Point(10, 20), Checked = true };
                    RadioButton radioNume = new RadioButton { Text = "Nume", Location = new Point(10, 45) };
                    RadioButton radioLocatie = new RadioButton { Text = "Locatie", Location = new Point(10, 70) };
                    RadioButton radioDescriere = new RadioButton { Text = "Descriere", Location = new Point(150, 20) };

                    groupCampuri.Controls.Add(radioIdDepartament);
                    groupCampuri.Controls.Add(radioNume);
                    groupCampuri.Controls.Add(radioLocatie);
                    groupCampuri.Controls.Add(radioDescriere);
                    panelOptiuni.Controls.Add(groupCampuri);

                    MetroLabel lblTermenCautare = new MetroLabel
                    {
                        Text = "Termen de cautare:",
                        Location = new Point(10, 245),
                        Size = new Size(150, 30)
                    };
                    formCautare.Controls.Add(lblTermenCautare);

                    MetroTextBox txtTermenCautare = new MetroTextBox
                    {
                        Location = new Point(160, 245),
                        Size = new Size(250, 30),
                    };
                    formCautare.Controls.Add(txtTermenCautare);

                    MetroLabel lblRezultate = new MetroLabel
                    {
                        Text = "Rezultatele cautarii:",
                        Location = new Point(10, 280),
                        Size = new Size(150, 30)
                    };
                    formCautare.Controls.Add(lblRezultate);

                    ListView listViewRezultate = new ListView
                    {
                        Location = new Point(10, 310),
                        Size = new Size(570, 130),
                        View = View.Details,
                        FullRowSelect = true,
                        GridLines = true
                    };

                    listViewRezultate.Columns.Add("ID", 50);
                    listViewRezultate.Columns.Add("Nume", 150);
                    listViewRezultate.Columns.Add("Locatie", 150);
                    listViewRezultate.Columns.Add("Descriere", 220);
                    formCautare.Controls.Add(listViewRezultate);

                    MetroButton btnCauta = new MetroButton
                    {
                        Text = "Cauta",
                        Location = new Point(430, 245),
                        Size = new Size(100, 30)
                    };
                    formCautare.Controls.Add(btnCauta);

                    btnCauta.Click += (s, ev) =>
                    {
                        if (string.IsNullOrWhiteSpace(txtTermenCautare.Text))
                        {
                            MessageBox.Show("Introduceti un termen de cautare!", "Atentie",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        CautaDepartamente(
                            listViewRezultate,
                            txtTermenCautare.Text,
                            radioPartial.Checked,
                            radioIdDepartament.Checked,
                            radioNume.Checked,
                            radioLocatie.Checked,
                            radioDescriere.Checked
                        );
                    };

                    listViewRezultate.DoubleClick += (s, ev) =>
                    {
                        if (listViewRezultate.SelectedItems.Count > 0)
                        {
                            int idDepartament = int.Parse(listViewRezultate.SelectedItems[0].Text);
                            AfiseazaDetaliiDepartament(idDepartament);
                        }
                    };

                    formCautare.Controls.Add(panelOptiuni);
                    formCautare.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nu aveti permisiunea de a cauta departamente.",
                    "Acces restrictionat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CautaDepartamente(
            ListView listViewRezultate,
            string termenCautare,
            bool cautarePartiala,
            bool cautaDupaId,
            bool cautaDupaNume,
            bool cautaDupaLocatie,
            bool cautaDupaDescriere)
        {
            listViewRezultate.Items.Clear();
            Departament[] departamente = adminDepartamente.GetDepartamente(out int nrDepartamente);
            List<Departament> rezultate = new List<Departament>();



            foreach (var departament in departamente)
            {
                bool potrivire = false;

                if (cautaDupaId)
                {
                    if (departament.IdDepartament.ToString().Contains(termenCautare))
                    {
                        potrivire = true;
                    }
                }
                else if (cautaDupaNume)
                {
                    if (cautarePartiala)
                    {
                        potrivire = departament.Nume.Contains(termenCautare);
                    }
                    else
                    {
                        potrivire = departament.Nume.Equals(termenCautare);
                    }
                }
                else if (cautaDupaLocatie)
                {
                    if (cautarePartiala)
                    {
                        potrivire = departament.Locatie.Contains(termenCautare);
                    }
                    else
                    {
                        potrivire = departament.Locatie.Equals(termenCautare);
                    }
                }
                else if (cautaDupaDescriere && !string.IsNullOrEmpty(departament.Descriere))
                {
                    if (cautarePartiala)
                    {
                        potrivire = departament.Descriere.Contains(termenCautare);
                    }
                    else
                    {
                        potrivire = departament.Descriere.Equals(termenCautare);
                    }
                }

                if (potrivire)
                {
                    rezultate.Add(departament);
                }
            }

            foreach (var departament in rezultate)
            {
                ListViewItem item = new ListViewItem(departament.IdDepartament.ToString());
                item.SubItems.Add(departament.Nume);
                item.SubItems.Add(departament.Locatie);
                item.SubItems.Add(departament.Descriere ?? "N/A");

                listViewRezultate.Items.Add(item);
            }

            if (rezultate.Count == 0)
            {
                MessageBox.Show("Nu s-au gasit departamente care sa corespunda criteriului selectat.",
                    "Rezultat cautare", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AddRowToTable(TableLayoutPanel table, int rowIndex, string label, string value)
        {
            Label lblLabel = new Label
            {
                Text = label,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Padding = new Padding(5)
            };

            Label lblValue = new Label
            {
                Text = value,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                AutoSize = false
            };

            table.Controls.Add(lblLabel, 0, rowIndex);
            table.Controls.Add(lblValue, 1, rowIndex);
        }
        private void AfiseazaDetaliiDepartament(int idDepartament)
        {
            Departament departament = GetDepartamentDupaId(idDepartament);
            if (departament == null)
            {
                MessageBox.Show("Nu s-a gasit departamentul selectat!", "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MetroForm formDetalii = new MetroForm())
            {
                formDetalii.Text = $"Detalii Departament #{departament.IdDepartament}";
                formDetalii.Size = new Size(500, 300);
                formDetalii.StartPosition = FormStartPosition.CenterScreen;
                formDetalii.Theme = MetroThemeStyle.Light;
                formDetalii.Style = MetroColorStyle.Black;
                formDetalii.Padding = new Padding(20);

                TableLayoutPanel tableLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 3,
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
                };

                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

                AddRowToTable(tableLayout, 0, "ID Departament:", departament.IdDepartament.ToString());
                AddRowToTable(tableLayout, 1, "Nume:", departament.Nume);
                AddRowToTable(tableLayout, 2, "Locatie:", departament.Locatie);
                AddRowToTable(tableLayout, 3, "Descriere:", departament.Descriere ?? "N/A");

                formDetalii.Controls.Add(tableLayout);
                formDetalii.ShowDialog();
            }
        }

        private Departament GetDepartamentDupaId(int idDepartament)
        {
            Departament[] departamente = adminDepartamente.GetDepartamente(out int nrDepartamente);
            foreach (var departament in departamente)
            {
                if (departament.IdDepartament == idDepartament)
                    return departament;
            }
            return null;
        }


    }
}
