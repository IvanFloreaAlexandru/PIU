using System;
using System.Windows.Forms;
using System.Drawing;
using LibrarieModele;
using NivelStocareDate;
using System.IO;
using MetroFramework.Forms;
using MetroFramework.Controls;


namespace UI
{
    public partial class FormPrincipal : MetroForm
    {
        private FlowLayoutPanel panelMeniu;
        private MetroButton btnGestionarePacienti;
        private MetroButton btnGestionareMedici;
        private MetroButton btnGestionareProgramari;
        private MetroButton btnGestionarePrescriptii;
        private MetroButton btnGestionareDepartamente;
        private MetroButton btnGestionareUtilizatori;
        private MetroButton btnLogout;
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

            panelMeniu.FlowDirection = FlowDirection.TopDown;
            panelMeniu.WrapContents = true; 

            btnGestionarePacienti = new MetroButton { Text = "Gestionare pacienti" };
            btnGestionareMedici = new MetroButton { Text = "Gestionare medici" };
            btnGestionareProgramari = new MetroButton { Text = "Gestionare programari" };
            btnGestionarePrescriptii = new MetroButton { Text = "Gestionare prescriptii" };
            btnGestionareDepartamente = new MetroButton { Text = "Gestionare departamente" };
            btnGestionareUtilizatori = new MetroButton { Text = "Gestionare utilizatori" };
            btnLogout = new MetroButton { Text = "Inchide aplicatia" };

            int buttonWidth = 200;
            int buttonHeight = 40;

            btnGestionarePacienti.Size = new Size(buttonWidth, buttonHeight);
            btnGestionareMedici.Size = new Size(buttonWidth, buttonHeight);
            btnGestionareProgramari.Size = new Size(buttonWidth, buttonHeight);
            btnGestionarePrescriptii.Size = new Size(buttonWidth, buttonHeight);
            btnGestionareDepartamente.Size = new Size(buttonWidth, buttonHeight);
            btnGestionareUtilizatori.Size = new Size(buttonWidth, buttonHeight);
            btnLogout.Size = new Size(buttonWidth, buttonHeight);


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
            FormGestionareMedici formGestionareMedici = new FormGestionareMedici(adminMedici, adminDepartamente,utilizatorCurent);
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
                adminMedici,
                utilizatorCurent
            );
            formGestionareProgramari.ShowDialog();
        }

        private void btnGestionarePrescriptii_Click(object sender, EventArgs e)
        {
            FormGestionarePrescriptii formGestionarePrescriptii = new FormGestionarePrescriptii(
                adminPrescriptii,
                adminPacienti,
                adminMedici,
                utilizatorCurent
            );
            formGestionarePrescriptii.ShowDialog();
        }

        private void btnGestionareDepartamente_Click(object sender, EventArgs e)
        {
            FormGestionareDepartamente formGestionareDepartamente = new FormGestionareDepartamente(
                adminDepartamente,utilizatorCurent
            );
            formGestionareDepartamente.ShowDialog();
        }

        private void btnGestionareUser_Click(object sender, EventArgs e)
        {
            FormGestionareUtilizatori formGestionareUtilizatori = new FormGestionareUtilizatori(
                adminUser, adminUserMemorie,utilizatorCurent
            );
            formGestionareUtilizatori.ShowDialog();
        }
        private void btnGestionarePacienti_Click(object sender, EventArgs e)
        {
            FormGestionarePacienti formGestionarePacienti = new FormGestionarePacienti(
                adminPacienti,
                utilizatorCurent
            );
            formGestionarePacienti.ShowDialog();
        }

    }
}
