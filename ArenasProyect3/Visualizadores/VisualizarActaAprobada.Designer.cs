
namespace ArenasProyect3.Visualizadores
{
    partial class VisualizarActaAprobada
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
            this.CrvVisualizarActaVisitaAprobada = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CrvVisualizarActaVisitaAprobada
            // 
            this.CrvVisualizarActaVisitaAprobada.ActiveViewIndex = -1;
            this.CrvVisualizarActaVisitaAprobada.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrvVisualizarActaVisitaAprobada.Cursor = System.Windows.Forms.Cursors.Default;
            this.CrvVisualizarActaVisitaAprobada.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrvVisualizarActaVisitaAprobada.Location = new System.Drawing.Point(0, 0);
            this.CrvVisualizarActaVisitaAprobada.Margin = new System.Windows.Forms.Padding(4);
            this.CrvVisualizarActaVisitaAprobada.Name = "CrvVisualizarActaVisitaAprobada";
            this.CrvVisualizarActaVisitaAprobada.Size = new System.Drawing.Size(1320, 647);
            this.CrvVisualizarActaVisitaAprobada.TabIndex = 4;
            this.CrvVisualizarActaVisitaAprobada.ToolPanelWidth = 267;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(1066, 9);
            this.lblCodigo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(50, 17);
            this.lblCodigo.TabIndex = 6;
            this.lblCodigo.Text = "codigo";
            // 
            // VisualizarActaAprobada
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 647);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.CrvVisualizarActaVisitaAprobada);
            this.Name = "VisualizarActaAprobada";
            this.Text = "Visualizar Acta Aprobada";
            this.Load += new System.EventHandler(this.VisualizarActaAprobada_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrvVisualizarActaVisitaAprobada;
        public System.Windows.Forms.Label lblCodigo;
    }
}