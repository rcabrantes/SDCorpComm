namespace Cliente.Formularios
{
    partial class Principal
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.txtMensagem = new System.Windows.Forms.TextBox();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.txtHistorico = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(657, 584);
            this.splitContainer1.SplitterDistance = 177;
            this.splitContainer1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.txtHistorico);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btnEnviar);
            this.splitContainer2.Panel2.Controls.Add(this.txtMensagem);
            this.splitContainer2.Size = new System.Drawing.Size(474, 582);
            this.splitContainer2.SplitterDistance = 521;
            this.splitContainer2.TabIndex = 0;
            // 
            // txtMensagem
            // 
            this.txtMensagem.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtMensagem.Location = new System.Drawing.Point(0, 0);
            this.txtMensagem.Multiline = true;
            this.txtMensagem.Name = "txtMensagem";
            this.txtMensagem.Size = new System.Drawing.Size(357, 57);
            this.txtMensagem.TabIndex = 0;
            // 
            // btnEnviar
            // 
            this.btnEnviar.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEnviar.Location = new System.Drawing.Point(363, 0);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(111, 57);
            this.btnEnviar.TabIndex = 1;
            this.btnEnviar.Text = "Enviar";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // txtHistorico
            // 
            this.txtHistorico.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHistorico.Location = new System.Drawing.Point(0, 0);
            this.txtHistorico.Multiline = true;
            this.txtHistorico.Name = "txtHistorico";
            this.txtHistorico.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHistorico.Size = new System.Drawing.Size(474, 521);
            this.txtHistorico.TabIndex = 2;
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 584);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Principal";
            this.Text = "Principal";
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox txtHistorico;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.TextBox txtMensagem;
    }
}