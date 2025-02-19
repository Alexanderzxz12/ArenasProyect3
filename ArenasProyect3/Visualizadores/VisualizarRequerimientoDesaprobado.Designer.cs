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
            this.CrvVisualizarRequerimientoDesaprobado.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CrvVisualizarRequerimientoDesaprobado.Name = "CrvVisualizarRequerimientoDesaprobado";
            this.CrvVisualizarRequerimientoDesaprobado.Size = new System.Drawing.Size(1320, 647);
            this.CrvVisualizarRequerimientoDesaprobado.TabIndex = 2;
            this.CrvVisualizarRequerimientoDesaprobado.ToolPanelWidth = 267;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(1048, 11);
            this.lblCodigo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(50, 17);
            this.lblCodigo.TabIndex = 4;
            this.lblCodigo.Text = "codigo";
            // 
            // VisualizarRequerimientoDesaprobado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 647);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.CrvVisualizarRequerimientoDesaprobado);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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