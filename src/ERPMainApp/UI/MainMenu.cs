using ERP.Core.Services;

namespace ERP.MainApp.UI;

public class MainMenu
{
    private readonly ProdutoMenu _produtoMenu;

    public MainMenu(ProdutoMenu produtoMenu)
    {
        _produtoMenu = produtoMenu;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== ChurrasTech ERP - Menu Principal ===");
            Console.WriteLine();
            Console.WriteLine("1. Produtos");
            Console.WriteLine("2. Compras");
            Console.WriteLine("3. Vendas");
            Console.WriteLine("4. Estoque");
            Console.WriteLine("5. Relatórios");
            Console.WriteLine("6. Formas de Pagamento");
            Console.WriteLine("0. Sair");
            Console.WriteLine();
            Console.Write("Escolha uma opção: ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await _produtoMenu.ShowAsync();
                    break;
                case "2":
                    await ShowNotImplemented("Compras");
                    break;
                case "3":
                    await ShowNotImplemented("Vendas");
                    break;
                case "4":
                    await ShowNotImplemented("Estoque");
                    break;
                case "5":
                    await ShowNotImplemented("Relatórios");
                    break;
                case "6":
                    await ShowNotImplemented("Formas de Pagamento");
                    break;
                case "0":
                    Console.WriteLine("Encerrando sistema...");
                    return;
                default:
                    Console.WriteLine("Opção inválida! Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task ShowNotImplemented(string moduleName)
    {
        Console.Clear();
        Console.WriteLine($"=== {moduleName} ===");
        Console.WriteLine();
        Console.WriteLine("Este módulo ainda não foi implementado.");
        Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal...");
        Console.ReadKey();
        await Task.CompletedTask;
    }
}