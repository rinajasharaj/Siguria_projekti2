namespace Projekt2
{
    partial class Fatura
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtmuaji = new System.Windows.Forms.ComboBox();
            this.txtlloji = new System.Windows.Forms.TextBox();
            this.txtviti = new System.Windows.Forms.TextBox();
            this.txtqmimi = new System.Windows.Forms.TextBox();
            this.txttvsh = new System.Windows.Forms.TextBox();
            this.savebtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtuser = new System.Windows.Forms.TextBox();
            this.certificatelabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lloji";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Viti";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Muaji";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Qmimi";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Qmimi me tvsh";
            //
            // txtmuaji
            //
            this.txtmuaji.FormattingEnabled = true;
            this.txtmuaji.Location = new System.Drawing.Point(114, 132);
            this.txtmuaji.Name = "txtmuaji";
            this.txtmuaji.Size = new System.Drawing.Size(100, 21);
            this.txtmuaji.TabIndex = 5;
            //
            // txtlloji
            //
            this.txtlloji.Location = new System.Drawing.Point(114, 50);
            this.txtlloji.Name = "txtlloji";
            this.txtlloji.Size = new System.Drawing.Size(100, 20);
            this.txtlloji.TabIndex = 6;
            this.txtlloji.TextChanged += new System.EventHandler(this.txtlloji_TextChanged);
            //
            // txtviti
            //
            this.txtviti.Location = new System.Drawing.Point(114, 93);
            this.txtviti.Name = "txtviti";
            this.txtviti.Size = new System.Drawing.Size(100, 20);
            this.txtviti.TabIndex = 7;
            //
            // txtqmimi
            //
            this.txtqmimi.Location = new System.Drawing.Point(114, 175);
            this.txtqmimi.Name = "txtqmimi";
            this.txtqmimi.Size = new System.Drawing.Size(100, 20);
            this.txtqmimi.TabIndex = 8;
            //
            // txttvsh
            //
            this.txttvsh.Location = new System.Drawing.Point(114, 206);
            this.txttvsh.Name = "txttvsh";
            this.txttvsh.ReadOnly = true;
            this.txttvsh.Size = new System.Drawing.Size(100, 20);
            this.txttvsh.TabIndex = 9;
            this.txttvsh.TextChanged += new System.EventHandler(this.txttvsh_TextChanged);
            //
            // savebtn
            //
            this.savebtn.Location = new System.Drawing.Point(84, 252);
            this.savebtn.Name = "savebtn";
            this.savebtn.Size = new System.Drawing.Size(75, 23);
            this.savebtn.TabIndex = 10;
            this.savebtn.Text = "Save";
            this.savebtn.UseVisualStyleBackColor = true;
            this.savebtn.Click += new System.EventHandler(this.savebtn_Click);
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "User Id";
            //
            // txtuser
            //
            this.txtuser.Location = new System.Drawing.Point(114, 17);
            this.txtuser.Name = "txtuser";
            this.txtuser.Size = new System.Drawing.Size(100, 20);
            this.txtuser.TabIndex = 12;
            //
            // certificatelabel
            //
            this.certificatelabel.AutoSize = true;
            this.certificatelabel.Location = new System.Drawing.Point(0, -2);
            this.certificatelabel.Name = "certificatelabel";
            this.certificatelabel.Size = new System.Drawing.Size(54, 13);
            this.certificatelabel.TabIndex = 13;
            this.certificatelabel.Text = "Certificate";
            this.certificatelabel.Click += new System.EventHandler(this.certificatelabel_Click);
            //
            // Fatura
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 298);
            this.Controls.Add(this.certificatelabel);
            this.Controls.Add(this.txtuser);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.savebtn);
            this.Controls.Add(this.txttvsh);
            this.Controls.Add(this.txtqmimi);
            this.Controls.Add(this.txtviti);
            this.Controls.Add(this.txtlloji);
            this.Controls.Add(this.txtmuaji);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Fatura";
            this.Text = "Fatura";
            this.Load += new System.EventHandler(this.Fatura_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox txtmuaji;
        private System.Windows.Forms.TextBox txtlloji;
        private System.Windows.Forms.TextBox txtviti;
        private System.Windows.Forms.TextBox txtqmimi;
        private System.Windows.Forms.TextBox txttvsh;
        private System.Windows.Forms.Button savebtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtuser;
        private System.Windows.Forms.Label certificatelabel;
    }
}