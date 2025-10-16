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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Abrir TelaHub por padr�o (qualificado para evitar colis�o com recurso gerado)
            Application.Run(new ERPLauncher.TelaHub());
        }
    }
}
