
namespace ArenasProyect3.Visualizadores
{
    partial class VisualizarLiquidacionesVenta
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
            this.CrvVisualizarLiquidacionVenta = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CrvVisualizarLiquidacionVenta
            // 
            this.CrvVisualizarLiquidacionVenta.ActiveViewIndex = -1;
            this.CrvVisualizarLiquidacionVenta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrvVisualizarLiquidacionVenta.Cursor = System.Windows.Forms.Cursors.Default;
            this.CrvVisualizarLiquidacionVenta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrvVisualizarLiquidacionVenta.Location = new System.Drawing.Point(0, 0);
            this.CrvVisualizarLiquidacionVenta.Name = "CrvVisualizarLiquidacionVenta";
            this.CrvVisualizarLiquidacionVenta.Size = new System.Drawing.Size(990, 526);
            this.CrvVisualizarLiquidacionVenta.TabIndex = 2;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(816, 7);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(39, 13);
            this.lblCodigo.TabIndex = 4;
            this.lblCodigo.Text = "codigo";
            // 
            // VisualizarLiquidacionesVenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 526);
            this.Controls.Add(this.CrvVisualizarLiquidacionVenta);
            this.Controls.Add(this.lblCodigo);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "VisualizarLiquidacionesVenta";
            this.Text = "Visualizar Liquidaciones de venta";
            this.Load += new System.EventHandler(this.VisualizarLiquidacionesVenta_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrvVisualizarLiquidacionVenta;
        public System.Windows.Forms.Label lblCodigo;
    }
}