namespace ArenasProyect3.Modulos.Mantenimientos
{
    partial class PanelCarga
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblRecopilar = new System.Windows.Forms.Label();
            this.lblPreparandoConexion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.progressBar1.Location = new System.Drawing.Point(18, 41);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(330, 19);
            this.progressBar1.TabIndex = 0;
            // 
            // lblRecopilar
            // 
            this.lblRecopilar.AutoSize = true;
            this.lblRecopilar.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecopilar.Location = new System.Drawing.Point(153, 9);
            this.lblRecopilar.Name = "lblRecopilar";
            this.lblRecopilar.Size = new System.Drawing.Size(114, 14);
            this.lblRecopilar.TabIndex = 1;
            this.lblRecopilar.Text = "Recopilando los datos";
            // 
            // lblPreparandoConexion
            // 
            this.lblPreparandoConexion.AutoSize = true;
            this.lblPreparandoConexion.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreparandoConexion.Location = new System.Drawing.Point(15, 9);
            this.lblPreparandoConexion.Name = "lblPreparandoConexion";
            this.lblPreparandoConexion.Size = new System.Drawing.Size(122, 14);
            this.lblPreparandoConexion.TabIndex = 2;
            this.lblPreparandoConexion.Text = "Preparando la conexión";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(273, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "Preparando la conexión";
            // 
            // PanelCarga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(366, 76);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblPreparandoConexion);
            this.Controls.Add(this.lblRecopilar);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PanelCarga";
            this.Text = "PanelCarga";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblRecopilar;
        private System.Windows.Forms.Label lblPreparandoConexion;
        private System.Windows.Forms.Label label1;
    }
}