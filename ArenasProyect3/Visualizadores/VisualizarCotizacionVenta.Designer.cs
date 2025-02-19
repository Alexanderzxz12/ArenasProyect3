namespace ArenasProyect3.Visualizadores
{
    partial class VisualizarCotizacionVenta
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
            this.lblCodigo = new System.Windows.Forms.Label();
            this.CrvVisualizarActaVisita = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(1087, 9);
            this.lblCodigo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(50, 17);
            this.lblCodigo.TabIndex = 6;
            this.lblCodigo.Text = "codigo";
            // 
            // CrvVisualizarActaVisita
            // 
            this.CrvVisualizarActaVisita.ActiveViewIndex = -1;
            this.CrvVisualizarActaVisita.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrvVisualizarActaVisita.Cursor = System.Windows.Forms.Cursors.Default;
            this.CrvVisualizarActaVisita.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrvVisualizarActaVisita.Location = new System.Drawing.Point(0, 0);
            this.CrvVisualizarActaVisita.Margin = new System.Windows.Forms.Padding(4);
            this.CrvVisualizarActaVisita.Name = "CrvVisualizarActaVisita";
            this.CrvVisualizarActaVisita.Size = new System.Drawing.Size(1320, 647);
            this.CrvVisualizarActaVisita.TabIndex = 7;
            this.CrvVisualizarActaVisita.ToolPanelWidth = 267;
            // 
            // VisualizarCotizacionVenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 647);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.CrvVisualizarActaVisita);
            this.Name = "VisualizarCotizacionVenta";
            this.Text = "VisualizarCotizacionVenta";
            this.Load += new System.EventHandler(this.VisualizarCotizacionVenta_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblCodigo;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrvVisualizarActaVisita;
    }
}