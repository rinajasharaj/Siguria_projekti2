namespace Projekt2
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loggedin = new System.Windows.Forms.Label();
            this.txtpassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtconfirmpass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblExit = new System.Windows.Forms.Label();
            this.lblClearFields = new System.Windows.Forms.Label();
            this.Registerbutton = new System.Windows.Forms.Button();
            this.txtusername = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.vhoosecertificate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // loggedin
            //
            this.loggedin.AutoSize = true;
            this.loggedin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loggedin.Location = new System.Drawing.Point(115, 270);
            this.loggedin.Name = "loggedin";
            this.loggedin.Size = new System.Drawing.Size(200, 15);
            this.loggedin.TabIndex = 37;
            this.loggedin.Text = "Already have an account. Log me in";
            this.loggedin.Click += new System.EventHandler(this.loggedin_Click_1);
            //
            // txtpassword
            //
            this.txtpassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpassword.Location = new System.Drawing.Point(128, 133);
            this.txtpassword.Name = "txtpassword";
            this.txtpassword.Size = new System.Drawing.Size(174, 21);
            this.txtpassword.TabIndex = 36;
            this.txtpassword.UseSystemPasswordChar = true;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(125, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 15);
            this.label2.TabIndex = 35;
            this.label2.Text = "Password";
            //
            // txtconfirmpass
            //
            this.txtconfirmpass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtconfirmpass.Location = new System.Drawing.Point(128, 184);
            this.txtconfirmpass.Name = "txtconfirmpass";
            this.txtconfirmpass.Size = new System.Drawing.Size(174, 21);
            this.txtconfirmpass.TabIndex = 34;
            this.txtconfirmpass.UseSystemPasswordChar = true;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(125, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 15);
            this.label3.TabIndex = 33;
            this.label3.Text = "Confirm Password";
            //
            // lblExit
            //
            this.lblExit.AutoSize = true;
            this.lblExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExit.Location = new System.Drawing.Point(196, 296);
            this.lblExit.Name = "lblExit";
            this.lblExit.Size = new System.Drawing.Size(27, 15);
            this.lblExit.TabIndex = 32;
            this.lblExit.Text = "Exit";
            this.lblExit.Click += new System.EventHandler(this.lblExit_Click);
            //
            // lblClearFields
            //
            this.lblClearFields.AutoSize = true;
            this.lblClearFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClearFields.Location = new System.Drawing.Point(230, 208);
            this.lblClearFields.Name = "lblClearFields";
            this.lblClearFields.Size = new System.Drawing.Size(72, 15);
            this.lblClearFields.TabIndex = 31;
            this.lblClearFields.Text = "Clear Fields";
            this.lblClearFields.Click += new System.EventHandler(this.lblClearFields_Click);
            //
            // Registerbutton
            //
            this.Registerbutton.BackColor = System.Drawing.Color.RosyBrown;
            this.Registerbutton.Location = new System.Drawing.Point(171, 226);
            this.Registerbutton.Name = "Registerbutton";
            this.Registerbutton.Size = new System.Drawing.Size(82, 32);
            this.Registerbutton.TabIndex = 30;
            this.Registerbutton.Text = "Register";
            this.Registerbutton.UseVisualStyleBackColor = false;
            this.Registerbutton.Click += new System.EventHandler(this.Registerbutton_Click_1);
            //
            // txtusername
            //
            this.txtusername.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtusername.Location = new System.Drawing.Point(128, 78);
            this.txtusername.Name = "txtusername";
            this.txtusername.Size = new System.Drawing.Size(174, 21);
            this.txtusername.TabIndex = 29;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(125, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 28;
            this.label1.Text = "Username";
            //
            // vhoosecertificate
            //
            this.vhoosecertificate.AutoSize = true;
            this.vhoosecertificate.Location = new System.Drawing.Point(12, 9);
            this.vhoosecertificate.Name = "vhoosecertificate";
            this.vhoosecertificate.Size = new System.Drawing.Size(54, 13);
            this.vhoosecertificate.TabIndex = 38;
            this.vhoosecertificate.Text = "Certificate";
            this.vhoosecertificate.Click += new System.EventHandler(this.vhoosecertificate_Click);
            //
            // Form2
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(430, 370);
            this.Controls.Add(this.vhoosecertificate);
            this.Controls.Add(this.loggedin);
            this.Controls.Add(this.txtpassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtconfirmpass);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblExit);
            this.Controls.Add(this.lblClearFields);
            this.Controls.Add(this.Registerbutton);
            this.Controls.Add(this.txtusername);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "Register";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label loggedin;
        private System.Windows.Forms.TextBox txtpassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtconfirmpass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblExit;
        private System.Windows.Forms.Label lblClearFields;
        private System.Windows.Forms.Button Registerbutton;
        private System.Windows.Forms.TextBox txtusername;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label vhoosecertificate;
    }
}