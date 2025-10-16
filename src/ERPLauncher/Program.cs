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
            // Abrir TelaHub por padrão (qualificado para evitar colisão com recurso gerado)
            Application.Run(new ERPLauncher.TelaHub());
        }
    }
}
