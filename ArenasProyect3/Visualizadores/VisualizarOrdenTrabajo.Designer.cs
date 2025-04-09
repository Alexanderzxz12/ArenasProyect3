
namespace ArenasProyect3.Visualizadores
{
    partial class VisualizarOrdenTrabajo
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
            this.CrvVisualizarOrdenProduccion = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CrvVisualizarOrdenProduccion
            // 
            this.CrvVisualizarOrdenProduccion.ActiveViewIndex = -1;
            this.CrvVisualizarOrdenProduccion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrvVisualizarOrdenProduccion.Cursor = System.Windows.Forms.Cursors.Default;
            this.CrvVisualizarOrdenProduccion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrvVisualizarOrdenProduccion.Location = new System.Drawing.Point(0, 0);
            this.CrvVisualizarOrdenProduccion.Name = "CrvVisualizarOrdenProduccion";
            this.CrvVisualizarOrdenProduccion.Size = new System.Drawing.Size(990, 526);
            this.CrvVisualizarOrdenProduccion.TabIndex = 6;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(788, 9);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(39, 13);
            this.lblCodigo.TabIndex = 8;
            this.lblCodigo.Text = "codigo";
            // 
            // VisualizarOrdenTrabajo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 526);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.CrvVisualizarOrdenProduccion);
            this.Name = "VisualizarOrdenTrabajo";
            this.Text = "VisualizarOrdenTrabajo";
            this.Load += new System.EventHandler(this.VisualizarOrdenTrabajo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrvVisualizarOrdenProduccion;
        public System.Windows.Forms.Label lblCodigo;
    }
}