
namespace ArenasProyect3.Visualizadores
{
    partial class VisualizarOrdenProduccion
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
            this.CrvVisualizarOrdenProduccion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CrvVisualizarOrdenProduccion.Name = "CrvVisualizarOrdenProduccion";
            this.CrvVisualizarOrdenProduccion.Size = new System.Drawing.Size(1320, 647);
            this.CrvVisualizarOrdenProduccion.TabIndex = 5;
            this.CrvVisualizarOrdenProduccion.ToolPanelWidth = 267;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(1053, 11);
            this.lblCodigo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(50, 17);
            this.lblCodigo.TabIndex = 7;
            this.lblCodigo.Text = "codigo";
            // 
            // VisualizarOrdenProduccion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 647);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.CrvVisualizarOrdenProduccion);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "VisualizarOrdenProduccion";
            this.Text = "VisualizarOrdenProduccion";
            this.Load += new System.EventHandler(this.VisualizarOrdenProduccion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrvVisualizarOrdenProduccion;
        public System.Windows.Forms.Label lblCodigo;
    }
}