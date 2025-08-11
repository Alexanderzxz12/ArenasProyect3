
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
            this.CrvVisualizarActaVisitaDesaprobada.Name = "CrvVisualizarActaVisitaDesaprobada";
            this.CrvVisualizarActaVisitaDesaprobada.Size = new System.Drawing.Size(990, 526);
            this.CrvVisualizarActaVisitaDesaprobada.TabIndex = 5;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(820, 7);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(39, 13);
            this.lblCodigo.TabIndex = 7;
            this.lblCodigo.Text = "codigo";
            // 
            // VisualizarActaDesaprobada
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 526);
            this.Controls.Add(this.CrvVisualizarActaVisitaDesaprobada);
            this.Controls.Add(this.lblCodigo);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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