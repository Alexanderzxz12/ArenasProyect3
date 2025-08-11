
namespace ArenasProyect3.Visualizadores
{
    partial class VisualizarLiquidacionDesaprobada
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
            this.CrvVisualizarLiquidacionVentaDesaprobada = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CrvVisualizarLiquidacionVentaDesaprobada
            // 
            this.CrvVisualizarLiquidacionVentaDesaprobada.ActiveViewIndex = -1;
            this.CrvVisualizarLiquidacionVentaDesaprobada.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrvVisualizarLiquidacionVentaDesaprobada.Cursor = System.Windows.Forms.Cursors.Default;
            this.CrvVisualizarLiquidacionVentaDesaprobada.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrvVisualizarLiquidacionVentaDesaprobada.Location = new System.Drawing.Point(0, 0);
            this.CrvVisualizarLiquidacionVentaDesaprobada.Name = "CrvVisualizarLiquidacionVentaDesaprobada";
            this.CrvVisualizarLiquidacionVentaDesaprobada.Size = new System.Drawing.Size(990, 526);
            this.CrvVisualizarLiquidacionVentaDesaprobada.TabIndex = 4;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(819, 7);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(39, 13);
            this.lblCodigo.TabIndex = 5;
            this.lblCodigo.Text = "codigo";
            // 
            // VisualizarLiquidacionDesaprobada
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 526);
            this.Controls.Add(this.CrvVisualizarLiquidacionVentaDesaprobada);
            this.Controls.Add(this.lblCodigo);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "VisualizarLiquidacionDesaprobada";
            this.Text = "Visualizar Liquidacion Desaprobada";
            this.Load += new System.EventHandler(this.VisualizarLiquidacionDesaprobada_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrvVisualizarLiquidacionVentaDesaprobada;
        public System.Windows.Forms.Label lblCodigo;
    }
}