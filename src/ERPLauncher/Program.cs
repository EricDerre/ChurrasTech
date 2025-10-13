using System;
using System.Windows.Forms;

namespace ERP.Launcher
{
    // Classe principal do programa
    static class Program
    {
        // Método de entrada da aplicação
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Nenhuma tela inicial definida, pronto para abrir qualquer tela criada via designer
            // Exemplo: Application.Run(new TelaCadastroProduto());
        }
    }
}
