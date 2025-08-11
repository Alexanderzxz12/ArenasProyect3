namespace ArenasProyect3.Visualizadores
{
    partial class VisualizarRequerimientoAprobado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualizarRequerimientoAprobado));
            this.CrvVisualizarRequerimientoAprobado = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CrvVisualizarRequerimientoAprobado
            // 
            this.CrvVisualizarRequerimientoAprobado.ActiveViewIndex = -1;
            this.CrvVisualizarRequerimientoAprobado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrvVisualizarRequerimientoAprobado.Cursor = System.Windows.Forms.Cursors.Default;
            this.CrvVisualizarRequerimientoAprobado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrvVisualizarRequerimientoAprobado.Location = new System.Drawing.Point(0, 0);
            this.CrvVisualizarRequerimientoAprobado.Name = "CrvVisualizarRequerimientoAprobado";
            this.CrvVisualizarRequerimientoAprobado.Size = new System.Drawing.Size(990, 526);
            this.CrvVisualizarRequerimientoAprobado.TabIndex = 0;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(786, 9);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(39, 13);
            this.lblCodigo.TabIndex = 3;
            this.lblCodigo.Text = "codigo";
            // 
            // VisualizarRequerimientoAprobado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 526);
            this.Controls.Add(this.CrvVisualizarRequerimientoAprobado);
            this.Controls.Add(this.lblCodigo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VisualizarRequerimientoAprobado";
            this.Text = "Visualizar Requerimiento Aprobado";
            this.Load += new System.EventHandler(this.VisualizarRequerimientoAprobado_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrvVisualizarRequerimientoAprobado;
        public System.Windows.Forms.Label lblCodigo;
    }
}