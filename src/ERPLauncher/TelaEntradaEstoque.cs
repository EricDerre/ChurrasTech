using System;
using System.Windows.Forms;

namespace ERPLauncher
{
    public partial class TelaEntradaEstoque : Form
    {
        public TelaEntradaEstoque()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TelaEntradaEstoque
            // 
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Name = "TelaEntradaEstoque";
            this.Text = "Entrada de Estoque";
            this.ResumeLayout(false);
        }
    }
}
