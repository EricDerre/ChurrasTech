using System;
using System.Windows.Forms;

namespace ERPLauncher
{
    public partial class TelaQuantidadeEstoque : Form
    {
        public TelaQuantidadeEstoque()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TelaQuantidadeEstoque
            // 
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Name = "TelaQuantidadeEstoque";
            this.Text = "Quantidade por Produto";
            this.ResumeLayout(false);
        }
    }
}
