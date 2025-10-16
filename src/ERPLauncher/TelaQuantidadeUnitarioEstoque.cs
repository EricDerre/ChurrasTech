using System;
using System.Windows.Forms;

namespace ERPLauncher
{
    public partial class TelaQuantidadeUnitarioEstoque : Form
    {
        public TelaQuantidadeUnitarioEstoque()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TelaQuantidadeUnitarioEstoque
            // 
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Name = "TelaQuantidadeUnitarioEstoque";
            this.Text = "Detalhamento Unitário de Estoque";
            this.ResumeLayout(false);
        }
    }
}
