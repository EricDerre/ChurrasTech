using System;
using System.Windows.Forms;

namespace ERP.Launcher
{
    // Classe principal do programa
    static class Program
    {
        // M�todo de entrada da aplica��o
        [STAThread]
        static void Main()
        {
            // Habilita estilos visuais para os controles do Windows Forms
            Application.EnableVisualStyles();
            // Define o modo de renderiza��o de texto para compatibilidade
            Application.SetCompatibleTextRenderingDefault(false);
            // Inicia o formul�rio principal da aplica��o
            Application.Run(new TelaPrincipal());
        }
    }
}
