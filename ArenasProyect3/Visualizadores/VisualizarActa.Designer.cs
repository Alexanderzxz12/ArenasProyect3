
namespace ArenasProyect3.Visualizadores
{
    partial class VisualizarActa
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
            this.CrvVisualizarActaVisita = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CrvVisualizarActaVisita
            // 
            this.CrvVisualizarActaVisita.ActiveViewIndex = -1;
            this.CrvVisualizarActaVisita.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrvVisualizarActaVisita.Cursor = System.Windows.Forms.Cursors.Default;
            this.CrvVisualizarActaVisita.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrvVisualizarActaVisita.Location = new System.Drawing.Point(0, 0);
            this.CrvVisualizarActaVisita.Name = "CrvVisualizarActaVisita";
            this.CrvVisualizarActaVisita.Size = new System.Drawing.Size(990, 526);
            this.CrvVisualizarActaVisita.TabIndex = 3;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(833, 9);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(39, 13);
            this.lblCodigo.TabIndex = 5;
            this.lblCodigo.Text = "codigo";
            // 
            // VisualizarActa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 526);
            this.Controls.Add(this.CrvVisualizarActaVisita);
            this.Controls.Add(this.lblCodigo);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "VisualizarActa";
            this.Text = "Visualizar Acta";
            this.Load += new System.EventHandler(this.VisualizarActa_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrvVisualizarActaVisita;
        public System.Windows.Forms.Label lblCodigo;
    }
}