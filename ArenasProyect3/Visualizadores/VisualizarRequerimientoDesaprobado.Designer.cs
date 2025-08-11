namespace ArenasProyect3.Visualizadores
{
    partial class VisualizarRequerimientoDesaprobado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualizarRequerimientoDesaprobado));
            this.CrvVisualizarRequerimientoDesaprobado = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CrvVisualizarRequerimientoDesaprobado
            // 
            this.CrvVisualizarRequerimientoDesaprobado.ActiveViewIndex = -1;
            this.CrvVisualizarRequerimientoDesaprobado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrvVisualizarRequerimientoDesaprobado.Cursor = System.Windows.Forms.Cursors.Default;
            this.CrvVisualizarRequerimientoDesaprobado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrvVisualizarRequerimientoDesaprobado.Location = new System.Drawing.Point(0, 0);
            this.CrvVisualizarRequerimientoDesaprobado.Name = "CrvVisualizarRequerimientoDesaprobado";
            this.CrvVisualizarRequerimientoDesaprobado.Size = new System.Drawing.Size(990, 526);
            this.CrvVisualizarRequerimientoDesaprobado.TabIndex = 2;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(786, 9);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(39, 13);
            this.lblCodigo.TabIndex = 4;
            this.lblCodigo.Text = "codigo";
            // 
            // VisualizarRequerimientoDesaprobado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 526);
            this.Controls.Add(this.CrvVisualizarRequerimientoDesaprobado);
            this.Controls.Add(this.lblCodigo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VisualizarRequerimientoDesaprobado";
            this.Text = "Visualizar Requerimiento Desaprobado";
            this.Load += new System.EventHandler(this.VisualizarRequerimientoDesaprobado_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrvVisualizarRequerimientoDesaprobado;
        public System.Windows.Forms.Label lblCodigo;
    }
}