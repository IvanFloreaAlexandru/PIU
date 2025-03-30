using System;
using System.Windows.Forms;
using System.Drawing;
using LibrarieModele;
using NivelStocareDate;
using System.IO;

namespace UI
{
    public partial class FormPrincipal : Form
    {
        private FlowLayoutPanel panelMeniu;
        private Button btnGestionarePacienti;
        private Button btnGestionareMedici;
        private Button btnGestionareProgramari;
        private Button btnGestionarePrescriptii;
        private Button btnGestionareDepartamente;
        private Button btnGestionareUtilizatori;

        private Button btnLogout;
        private AdministrarePacienti_FisierText adminPacienti;
        private AdministrareMedici_FisierText adminMedici;
        private AdministrareDepartamente_FisierText adminDepartamente;
        private AdministrareProgramari_FisierText adminProgramari;
        private AdministrarePrescriptii_FisierText adminPrescriptii;
        private AdministrareUser_FisierText adminUser;
        private AdministrareUser_Memorie adminUserMemorie;


        private User utilizatorCurent;

        public FormPrincipal(User utilizator)
        {
            InitializeComponent();
            utilizatorCurent = utilizator;

            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisierPacienti = Path.Combine(locatieFisierSolutie, "Pacienti.txt");
            string caleCompletaFisierMedici = Path.Combine(locatieFisierSolutie, "Medici.txt");
            string caleCompletaFisierDepartamente = Path.Combine(locatieFisierSolutie, "Departamente.txt");
            string caleCompletaFisierProgramari = Path.Combine(locatieFisierSolutie, "Programari.txt");
            string caleCompletaFisierPrescriptii = Path.Combine(locatieFisierSolutie, "Prescriptii.txt");
            string caleCompletaFisierUser = Path.Combine(locatieFisierSolutie, "User.txt");


            adminPacienti = new AdministrarePacienti_FisierText(caleCompletaFisierPacienti);
            adminMedici = new AdministrareMedici_FisierText(caleCompletaFisierMedici);
            adminDepartamente = new AdministrareDepartamente_FisierText(caleCompletaFisierDepartamente);
            adminProgramari = new AdministrareProgramari_FisierText(caleCompletaFisierProgramari);
            adminPrescriptii = new AdministrarePrescriptii_FisierText(caleCompletaFisierPrescriptii);
            adminUser = new AdministrareUser_FisierText(caleCompletaFisierPrescriptii);
            adminUserMemorie = new AdministrareUser_Memorie();

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

            btnLogout.Click += BtnInchideAplicatia_Click;
            btnGestionarePacienti.Click += btnGestionarePacienti_Click;
            btnGestionareMedici.Click += btnGestionareMedici_Click;
            btnGestionareProgramari.Click += btnGestionareProgramari_Click;
            btnGestionarePrescriptii.Click += btnGestionarePrescriptii_Click;
            btnGestionareDepartamente.Click += btnGestionareDepartamente_Click;
            btnGestionareUtilizatori.Click += btnGestionareUser_Click;
        }
        
        private void ConfigureazaMeniu()
        {
            panelMeniu.Controls.Clear();

            btnGestionarePacienti = new Button { Text = "Gestionare pacienti" };
            btnGestionareMedici = new Button { Text = "Gestionare medici" };
            btnGestionareProgramari = new Button { Text = "Gestionare programari" };
            btnGestionarePrescriptii = new Button { Text = "Gestionare prescriptii" };
            btnGestionareDepartamente = new Button { Text = "Gestionare departamente" };
            btnGestionareUtilizatori = new Button { Text = "Gestionare utilizatori" };
            btnLogout = new Button { Text = "Inchide aplicatia" };

            btnGestionarePacienti.Size =
            btnGestionareMedici.Size =
            btnGestionareProgramari.Size =
            btnGestionarePrescriptii.Size =
            btnGestionareDepartamente.Size =
            btnGestionareUtilizatori.Size =
            btnLogout.Size = new Size(200, 40);

            panelMeniu.Controls.Add(btnGestionarePacienti);
            panelMeniu.Controls.Add(btnGestionareMedici);
            panelMeniu.Controls.Add(btnGestionareProgramari);
            panelMeniu.Controls.Add(btnGestionarePrescriptii);
            panelMeniu.Controls.Add(btnGestionareDepartamente);
            panelMeniu.Controls.Add(btnGestionareUtilizatori);
            panelMeniu.Controls.Add(btnLogout);
        }

        private void btnGestionareMedici_Click(object sender, EventArgs e)
        {
            FormGestionareMedici formGestionareMedici = new FormGestionareMedici(adminMedici, adminDepartamente);
            formGestionareMedici.ShowDialog();
        }

        private void BtnInapoi_Click(object sender, EventArgs e)
        {
            ConfigureazaMeniu();
        }


        private void BtnInchideAplicatia_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnGestionareProgramari_Click(object sender, EventArgs e)
        {
            FormGestionareProgramari formGestionareProgramari = new FormGestionareProgramari(
                adminProgramari,
                adminPacienti,
                adminMedici
            );
            formGestionareProgramari.ShowDialog();
        }
        private void btnGestionarePrescriptii_Click(object sender, EventArgs e)
        {
            FormGestionarePrescriptii formGestionarePrescriptii = new FormGestionarePrescriptii(
                adminPrescriptii,
                adminPacienti,
                adminMedici
            );
            formGestionarePrescriptii.ShowDialog();
        }
        private void btnGestionareDepartamente_Click(object sender, EventArgs e)
        {
            FormGestionareDepartamente formGestionareDepartamente = new FormGestionareDepartamente(
                adminDepartamente
            );
            formGestionareDepartamente.ShowDialog();
        }
        private void btnGestionareUser_Click(object sender, EventArgs e)
        {
            FormGestionareUtilizatori formGestionareUtilizatori = new FormGestionareUtilizatori(
                adminUser,adminUserMemorie
            );
            formGestionareUtilizatori.ShowDialog();
        }
        private void btnGestionarePacienti_Click(object sender, EventArgs e)
        {
            FormGestionarePacienti formGestionarePacienti = new FormGestionarePacienti(
                utilizatorCurent,
                this.Size
            );
            formGestionarePacienti.ShowDialog();
        }
    }
}