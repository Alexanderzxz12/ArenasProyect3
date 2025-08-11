
namespace ArenasProyect3.Visualizadores
{
    partial class VisualizarPedidoVenta
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
            this.CrvVisualizarPedido = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CrvVisualizarPedido
            // 
            this.CrvVisualizarPedido.ActiveViewIndex = -1;
            this.CrvVisualizarPedido.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrvVisualizarPedido.Cursor = System.Windows.Forms.Cursors.Default;
            this.CrvVisualizarPedido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrvVisualizarPedido.Location = new System.Drawing.Point(0, 0);
            this.CrvVisualizarPedido.Name = "CrvVisualizarPedido";
            this.CrvVisualizarPedido.Size = new System.Drawing.Size(990, 526);
            this.CrvVisualizarPedido.TabIndex = 3;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(789, 9);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(39, 13);
            this.lblCodigo.TabIndex = 7;
            this.lblCodigo.Text = "codigo";
            // 
            // VisualizarPedidoVenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 526);
            this.Controls.Add(this.CrvVisualizarPedido);
            this.Controls.Add(this.lblCodigo);
            this.Name = "VisualizarPedidoVenta";
            this.Text = "Visualizar Pedido Venta";
            this.Load += new System.EventHandler(this.VisualizarPedidoVenta_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrvVisualizarPedido;
        public System.Windows.Forms.Label lblCodigo;
    }
}