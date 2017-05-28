namespace Cliente.Formularios
{
    partial class Inicio
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
            this.pnlLogin = new System.Windows.Forms.Panel();
            this.btnConectar = new System.Windows.Forms.Button();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlAdmin = new System.Windows.Forms.Panel();
            this.txtAdminSenha2 = new System.Windows.Forms.TextBox();
            this.txtAdminSenha = new System.Windows.Forms.TextBox();
            this.txtAdminNome = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCriarUsuario = new System.Windows.Forms.Button();
            this.pnlLogin.SuspendLayout();
            this.pnlAdmin.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLogin
            // 
            this.pnlLogin.Controls.Add(this.btnConectar);
            this.pnlLogin.Controls.Add(this.txtSenha);
            this.pnlLogin.Controls.Add(this.txtNome);
            this.pnlLogin.Controls.Add(this.label2);
            this.pnlLogin.Controls.Add(this.label1);
            this.pnlLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLogin.Location = new System.Drawing.Point(0, 0);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Size = new System.Drawing.Size(470, 146);
            this.pnlLogin.TabIndex = 0;
            this.pnlLogin.Visible = false;
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new System.Drawing.Point(336, 41);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(75, 23);
            this.btnConectar.TabIndex = 1;
            this.btnConectar.Text = "Conectar";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(181, 69);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.PasswordChar = '*';
            this.txtSenha.Size = new System.Drawing.Size(100, 20);
            this.txtSenha.TabIndex = 3;
            // 
            // txtNome
            // 
            this.txtNome.Location = new System.Drawing.Point(181, 43);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(100, 20);
            this.txtNome.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Nome:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Senha:";
            // 
            // pnlAdmin
            // 
            this.pnlAdmin.Controls.Add(this.txtAdminSenha2);
            this.pnlAdmin.Controls.Add(this.txtAdminSenha);
            this.pnlAdmin.Controls.Add(this.txtAdminNome);
            this.pnlAdmin.Controls.Add(this.label5);
            this.pnlAdmin.Controls.Add(this.label4);
            this.pnlAdmin.Controls.Add(this.label3);
            this.pnlAdmin.Controls.Add(this.btnCriarUsuario);
            this.pnlAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAdmin.Location = new System.Drawing.Point(0, 0);
            this.pnlAdmin.Name = "pnlAdmin";
            this.pnlAdmin.Size = new System.Drawing.Size(470, 146);
            this.pnlAdmin.TabIndex = 1;
            this.pnlAdmin.Visible = false;
            this.pnlAdmin.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlAdmin_Paint);
            // 
            // txtAdminSenha2
            // 
            this.txtAdminSenha2.Location = new System.Drawing.Point(120, 88);
            this.txtAdminSenha2.Name = "txtAdminSenha2";
            this.txtAdminSenha2.Size = new System.Drawing.Size(100, 20);
            this.txtAdminSenha2.TabIndex = 6;
            // 
            // txtAdminSenha
            // 
            this.txtAdminSenha.Location = new System.Drawing.Point(120, 58);
            this.txtAdminSenha.Name = "txtAdminSenha";
            this.txtAdminSenha.Size = new System.Drawing.Size(100, 20);
            this.txtAdminSenha.TabIndex = 5;
            // 
            // txtAdminNome
            // 
            this.txtAdminNome.Location = new System.Drawing.Point(120, 32);
            this.txtAdminNome.Name = "txtAdminNome";
            this.txtAdminNome.Size = new System.Drawing.Size(100, 20);
            this.txtAdminNome.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Repetir senha";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Senha:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(67, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Nome:";
            // 
            // btnCriarUsuario
            // 
            this.btnCriarUsuario.Location = new System.Drawing.Point(269, 30);
            this.btnCriarUsuario.Name = "btnCriarUsuario";
            this.btnCriarUsuario.Size = new System.Drawing.Size(126, 23);
            this.btnCriarUsuario.TabIndex = 0;
            this.btnCriarUsuario.Text = "Cadastrar usuario";
            this.btnCriarUsuario.UseVisualStyleBackColor = true;
            this.btnCriarUsuario.Click += new System.EventHandler(this.btnCriarUsuario_Click);
            // 
            // Inicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 146);
            this.Controls.Add(this.pnlAdmin);
            this.Controls.Add(this.pnlLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Inicio";
            this.Text = "Inicio";
            this.Load += new System.EventHandler(this.Inicio_Load);
            this.pnlLogin.ResumeLayout(false);
            this.pnlLogin.PerformLayout();
            this.pnlAdmin.ResumeLayout(false);
            this.pnlAdmin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLogin;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.Panel pnlAdmin;
        private System.Windows.Forms.TextBox txtAdminSenha2;
        private System.Windows.Forms.TextBox txtAdminSenha;
        private System.Windows.Forms.TextBox txtAdminNome;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCriarUsuario;
    }
}