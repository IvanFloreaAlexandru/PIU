using System;
using System.Windows.Forms;
using System.Drawing;

namespace UI
{
    partial class FormAutentificare
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox textBoxEmailInput;
        private TextBox textBoxParolaInput;
        private Button btnAutentificare;
        private Button btnAnulare;

        private void InitializeComponent()
        {
            this.textBoxEmailInput = new TextBox();
            this.textBoxParolaInput = new TextBox();
            this.btnAutentificare = new Button();
            this.btnAnulare = new Button();

            this.SuspendLayout();

            this.textBoxEmailInput.Location = new Point(80, 30);
            this.textBoxEmailInput.Name = "textBoxEmailInput";
            this.textBoxEmailInput.Size = new Size(200, 20);

            this.textBoxParolaInput.Location = new Point(80, 70);
            this.textBoxParolaInput.Name = "textBoxParolaInput";
            this.textBoxParolaInput.PasswordChar = '*';
            this.textBoxParolaInput.Size = new Size(200, 20);

            this.btnAutentificare.Text = "Autentificare";
            this.btnAutentificare.Location = new Point(80, 110);
            this.btnAutentificare.Click += new EventHandler(this.btnAutentificare_Click);

            this.btnAnulare.Text = "Anulare";
            this.btnAnulare.Location = new Point(200, 110);
            this.btnAnulare.Click += new EventHandler(this.btnAnulare_Click);

            this.ClientSize = new Size(320, 160);
            this.Controls.Add(this.textBoxEmailInput);
            this.Controls.Add(this.textBoxParolaInput);
            this.Controls.Add(this.btnAutentificare);
            this.Controls.Add(this.btnAnulare);
            this.Name = "FormAutentificare";
            this.Text = "Autentificare";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}


