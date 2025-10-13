using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace ERPLauncher
{
    public partial class TelaHub : Form
    {
        // Lista para rastrear telas abertas
        private List<Form> telasAbertas = new List<Form>();

        public TelaHub()
        {
            InitializeComponent();
        }

        // Exemplo de método para abrir uma tela
        private void AbrirTela(Form tela)
        {
            tela.FormClosed += (s, e) => telasAbertas.Remove(tela);
            telasAbertas.Add(tela);
            tela.Show();
        }

        // Evento de fechamento do Hub
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (telasAbertas.Count > 0)
            {
                MessageBox.Show("Feche todas as telas abertas antes de fechar o Hub.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }

        private void TelaHub_Load(object sender, EventArgs e)
        {

        }

        private void labelChurras_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btEstoque_Click(object sender, EventArgs e)
        {

        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {
            // Evento de Paint vazio para evitar erro de compilação
        }
    }
}
