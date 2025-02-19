namespace ArenasProyect3.Visualizadores
{
    partial class VisualizarRequerimientoVenta
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualizarRequerimientoVenta));
            this.CrvVisualizarRequerimientoVenta = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CrvVisualizarRequerimientoVenta
            // 
            this.CrvVisualizarRequerimientoVenta.ActiveViewIndex = -1;
            this.CrvVisualizarRequerimientoVenta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrvVisualizarRequerimientoVenta.Cursor = System.Windows.Forms.Cursors.Default;
            this.CrvVisualizarRequerimientoVenta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrvVisualizarRequerimientoVenta.Location = new System.Drawing.Point(0, 0);
            this.CrvVisualizarRequerimientoVenta.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CrvVisualizarRequerimientoVenta.Name = "CrvVisualizarRequerimientoVenta";
            this.CrvVisualizarRequerimientoVenta.Size = new System.Drawing.Size(1320, 647);
            this.CrvVisualizarRequerimientoVenta.TabIndex = 1;
            this.CrvVisualizarRequerimientoVenta.ToolPanelWidth = 267;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(1053, 11);
            this.lblCodigo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(50, 17);
            this.lblCodigo.TabIndex = 3;
            this.lblCodigo.Text = "codigo";
            // 
            // VisualizarRequerimientoVenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 647);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.CrvVisualizarRequerimientoVenta);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "VisualizarRequerimientoVenta";
            this.Text = "Visualizar Requerimiento de Viajes";
            this.Load += new System.EventHandler(this.VisualizarRequerimientoVenta_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrvVisualizarRequerimientoVenta;
        public System.Windows.Forms.Label lblCodigo;
    }
}