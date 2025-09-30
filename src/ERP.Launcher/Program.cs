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
            // Habilita estilos visuais para os controles do Windows Forms
            Application.EnableVisualStyles();
            // Define o modo de renderização de texto para compatibilidade
            Application.SetCompatibleTextRenderingDefault(false);
            // Inicia o formulário principal da aplicação
            Application.Run(new TelaPrincipal());
        }
    }
}
