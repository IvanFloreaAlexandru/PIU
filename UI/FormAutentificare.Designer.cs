using System;
using System.Windows.Forms;
using System.Drawing;
using MetroFramework.Forms;
using MetroFramework.Controls;
using MetroFramework;

namespace UI
{
    partial class FormAutentificare
    {
        private System.ComponentModel.IContainer components = null;
        private MetroTextBox metroTextBoxEmailInput;
        private MetroTextBox metroTextBoxParolaInput;
        private MetroButton metroButtonAutentificare;
        private MetroButton metroButtonAnulare;
        private MetroLabel metroLabelEmail;
        private MetroLabel metroLabelParola;
        private MetroPanel metroPanel;
        private MetroLabel metroLabelTitlu;

        private void InitializeComponent()
        {
            this.metroTextBoxEmailInput = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBoxParolaInput = new MetroFramework.Controls.MetroTextBox();
            this.metroButtonAutentificare = new MetroFramework.Controls.MetroButton();
            this.metroButtonAnulare = new MetroFramework.Controls.MetroButton();
            this.metroLabelEmail = new MetroFramework.Controls.MetroLabel();
            this.metroLabelParola = new MetroFramework.Controls.MetroLabel();
            this.metroPanel = new MetroFramework.Controls.MetroPanel();
            this.metroLabelTitlu = new MetroFramework.Controls.MetroLabel();

            this.metroPanel.SuspendLayout();
            this.SuspendLayout();

            this.metroLabelTitlu.AutoSize = true;
            this.metroLabelTitlu.Location = new System.Drawing.Point(350, 110);
            this.metroLabelTitlu.Name = "metroLabelTitlu";
            this.metroLabelTitlu.Size = new System.Drawing.Size(200, 19);
            this.metroLabelTitlu.TabIndex = 0;
            this.metroLabelTitlu.Text = "Autentificare utilizator";

            this.metroLabelTitlu.Font = new Font("Segoe UI", 24, FontStyle.Bold);

            this.metroTextBoxEmailInput.Location = new System.Drawing.Point(120, 20);
            this.metroTextBoxEmailInput.Name = "metroTextBoxEmailInput";
            this.metroTextBoxEmailInput.Size = new System.Drawing.Size(220, 23);
            this.metroTextBoxEmailInput.TabIndex = 1;

            this.metroTextBoxParolaInput.Location = new System.Drawing.Point(120, 60);
            this.metroTextBoxParolaInput.Name = "metroTextBoxParolaInput";
            this.metroTextBoxParolaInput.PasswordChar = '●';
            this.metroTextBoxParolaInput.Size = new System.Drawing.Size(220, 23);
            this.metroTextBoxParolaInput.TabIndex = 3;
            this.metroTextBoxParolaInput.UseSystemPasswordChar = true;


            this.metroButtonAutentificare.Location = new System.Drawing.Point(120, 110);
            this.metroButtonAutentificare.Name = "metroButtonAutentificare";
            this.metroButtonAutentificare.Size = new System.Drawing.Size(100, 30);
            this.metroButtonAutentificare.TabIndex = 4;
            this.metroButtonAutentificare.Text = "Autentificare";
            this.metroButtonAutentificare.Click += new System.EventHandler(this.metroButtonAutentificare_Click);

            this.metroButtonAnulare.Location = new System.Drawing.Point(240, 110);
            this.metroButtonAnulare.Name = "metroButtonAnulare";
            this.metroButtonAnulare.Size = new System.Drawing.Size(100, 30);
            this.metroButtonAnulare.TabIndex = 5;
            this.metroButtonAnulare.Text = "Anulare";
            this.metroButtonAnulare.Click += new System.EventHandler(this.metroButtonAnulare_Click);

            this.metroLabelEmail.AutoSize = true;
            this.metroLabelEmail.Location = new System.Drawing.Point(20, 20);
            this.metroLabelEmail.Name = "metroLabelEmail";
            this.metroLabelEmail.Size = new System.Drawing.Size(44, 19);
            this.metroLabelEmail.TabIndex = 0;
            this.metroLabelEmail.Text = "Email:";

            this.metroLabelParola.AutoSize = true;
            this.metroLabelParola.Location = new System.Drawing.Point(20, 60);
            this.metroLabelParola.Name = "metroLabelParola";
            this.metroLabelParola.Size = new System.Drawing.Size(49, 19);
            this.metroLabelParola.TabIndex = 2;
            this.metroLabelParola.Text = "Parola:";

            this.metroPanel.Controls.Add(this.metroLabelEmail);
            this.metroPanel.Controls.Add(this.metroTextBoxEmailInput);
            this.metroPanel.Controls.Add(this.metroLabelParola);
            this.metroPanel.Controls.Add(this.metroTextBoxParolaInput);
            this.metroPanel.Controls.Add(this.metroButtonAutentificare);
            this.metroPanel.Controls.Add(this.metroButtonAnulare);
            this.metroPanel.HorizontalScrollbarBarColor = true;
            this.metroPanel.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel.HorizontalScrollbarSize = 10;
            this.metroPanel.Location = new System.Drawing.Point(220, 130);
            this.metroPanel.Name = "metroPanel";
            this.metroPanel.Size = new System.Drawing.Size(360, 180);
            this.metroPanel.TabIndex = 0;
            this.metroPanel.VerticalScrollbarBarColor = true;
            this.metroPanel.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel.VerticalScrollbarSize = 10;

            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.metroLabelTitlu);
            this.Controls.Add(this.metroPanel);
            this.Name = "FormAutentificare";
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Black;
            this.Theme = MetroFramework.MetroThemeStyle.Light;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.metroPanel.ResumeLayout(false);
            this.metroPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
