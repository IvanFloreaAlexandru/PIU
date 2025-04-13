using System;

using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework.Controls;

namespace UI
{
    public partial class FormAutentificare : MetroForm
    {
        public FormAutentificare()
        {
            InitializeComponent();
        }

        public MetroTextBox metroTextBoxEmail { get { return metroTextBoxEmailInput; } }
        public MetroTextBox metroTextBoxParola { get { return metroTextBoxParolaInput; } }

        private void metroButtonAutentificare_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void metroButtonAnulare_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
