using System;
using System.Windows.Forms;

namespace ERPLauncher
{
    public partial class TelaBaixaEstoque : Form
    {
        public TelaBaixaEstoque()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TelaBaixaEstoque
            // 
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Name = "TelaBaixaEstoque";
            this.Text = "Baixa de Estoque";
            this.ResumeLayout(false);
        }
    }
}
