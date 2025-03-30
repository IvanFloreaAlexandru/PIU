using System;
using System.Windows.Forms;
using System.Drawing;
using GestionareSpital;
using LibrarieModele;
using NivelStocareDate;
using System.Linq;

namespace UI
{
    public partial class FormGestionarePacienti : Form
    {
        private Button btnAdaugarePacient;
        private Button btnModificarePacient;
        private Button btnStergerePacient;
        private Button btnVizualizarePacienti;
        private Button btnInapoi;

        private AdministrarePacienti_FisierText adminPacienti;
        private User utilizatorCurent;

        public FormGestionarePacienti(User utilizator, Size size)
        {
            InitializeComponent();
            utilizatorCurent = utilizator;
            adminPacienti = new AdministrarePacienti_FisierText("Pacienti.txt");

            this.Size = size;
            this.MinimumSize = new Size(1080, 400); 
            this.StartPosition = FormStartPosition.CenterScreen;

            btnAdaugarePacient = new Button { Text = "Adaugare pacient" };
            btnModificarePacient = new Button { Text = "Modificare pacient" };
            btnStergerePacient = new Button { Text = "Stergere pacient" };
            btnVizualizarePacienti = new Button { Text = "Vizualizare pacienti" };
            btnInapoi = new Button { Text = "Inapoi" };

            btnAdaugarePacient.Size = new Size(200, 40);
            btnModificarePacient.Size = new Size(200, 40);
            btnStergerePacient.Size = new Size(200, 40);
            btnVizualizarePacienti.Size = new Size(200, 40);
            btnInapoi.Size = new Size(200, 40);

            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(10)
            };

            panel.Controls.Add(btnAdaugarePacient);
            panel.Controls.Add(btnModificarePacient);
            panel.Controls.Add(btnStergerePacient);
            panel.Controls.Add(btnVizualizarePacienti);
            panel.Controls.Add(btnInapoi);
            this.Controls.Add(panel);

            btnAdaugarePacient.Click += BtnAdaugarePacient_Click;
            btnModificarePacient.Click += BtnModificarePacient_Click;
            btnStergerePacient.Click += BtnStergerePacient_Click;
            btnVizualizarePacienti.Click += BtnVizualizarePacienti_Click;
            btnInapoi.Click += BtnInapoi_Click;
        }

        private void BtnAdaugarePacient_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");
        }

        private void BtnModificarePacient_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");
        }

        private void BtnStergerePacient_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Functionalitate in curs de implementare.", "De implementat");
        }

        private void BtnInapoi_Click(object sender, EventArgs e)
        {
            this.Hide();

            FormPrincipal formPrincipal = Application.OpenForms.OfType<FormPrincipal>().FirstOrDefault();

            if (formPrincipal != null)
            {
                formPrincipal.Show();
            }
            else
            {
                Application.Exit();
            }
        }

        private void BtnVizualizarePacienti_Click(object sender, EventArgs e)
        {
            Pacient[] pacienti = adminPacienti.GetPacienti(out int nrPacienti);

            using (Form formVizualizare = new Form())
            {
                formVizualizare.Text = "Toti Pacientii";
                formVizualizare.Size = new Size(700, 400);
                formVizualizare.StartPosition = FormStartPosition.CenterScreen;

                ListView listView = new ListView
                {
                    Dock = DockStyle.Fill,
                    View = View.Details,
                    FullRowSelect = true,
                    GridLines = true
                };

                listView.Columns.Add("ID", 50);
                listView.Columns.Add("Nume", 150);
                listView.Columns.Add("CNP", 120);
                listView.Columns.Add("Adresa", 150);
                listView.Columns.Add("Telefon", 100);
                listView.Columns.Add("Data Nasterii", 100);
                listView.Columns.Add("Gen", 50);

                foreach (var pacient in pacienti)
                {
                    ListViewItem item = new ListViewItem(pacient.IdPacient.ToString());
                    item.SubItems.Add(pacient.Nume);
                    item.SubItems.Add(pacient.CNP);
                    item.SubItems.Add(pacient.Adresa);
                    item.SubItems.Add(pacient.Telefon);
                    item.SubItems.Add(pacient.DataNasterii.ToShortDateString());
                    item.SubItems.Add(pacient.Gen);

                    listView.Items.Add(item);
                }

                formVizualizare.Controls.Add(listView);
                formVizualizare.ShowDialog();
            }
        }
    }
}
