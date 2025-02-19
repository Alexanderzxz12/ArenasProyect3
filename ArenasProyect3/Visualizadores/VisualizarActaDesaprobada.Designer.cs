
namespace ArenasProyect3.Visualizadores
{
    partial class VisualizarActaDesaprobada
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
            this.CrvVisualizarActaVisitaDesaprobada = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CrvVisualizarActaVisitaDesaprobada
            // 
            this.CrvVisualizarActaVisitaDesaprobada.ActiveViewIndex = -1;
            this.CrvVisualizarActaVisitaDesaprobada.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrvVisualizarActaVisitaDesaprobada.Cursor = System.Windows.Forms.Cursors.Default;
            this.CrvVisualizarActaVisitaDesaprobada.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrvVisualizarActaVisitaDesaprobada.Location = new System.Drawing.Point(0, 0);
            this.CrvVisualizarActaVisitaDesaprobada.Margin = new System.Windows.Forms.Padding(4);
            this.CrvVisualizarActaVisitaDesaprobada.Name = "CrvVisualizarActaVisitaDesaprobada";
            this.CrvVisualizarActaVisitaDesaprobada.Size = new System.Drawing.Size(1320, 647);
            this.CrvVisualizarActaVisitaDesaprobada.TabIndex = 5;
            this.CrvVisualizarActaVisitaDesaprobada.ToolPanelWidth = 267;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(1094, 9);
            this.lblCodigo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(50, 17);
            this.lblCodigo.TabIndex = 7;
            this.lblCodigo.Text = "codigo";
            // 
            // VisualizarActaDesaprobada
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 647);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.CrvVisualizarActaVisitaDesaprobada);
            this.Name = "VisualizarActaDesaprobada";
            this.Text = "Visualizar Acta Desaprobada";
            this.Load += new System.EventHandler(this.VisualizarActaDesaprobada_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrvVisualizarActaVisitaDesaprobada;
        public System.Windows.Forms.Label lblCodigo;
    }
}