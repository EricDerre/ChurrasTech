using ERP.Core.Services;
using ERP.Core.Models;

namespace ERP.MainApp.UI;

public class ProdutoMenu
{
    private readonly IProdutoService _produtoService;

    public ProdutoMenu(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Gestão de Produtos ===");
            Console.WriteLine();
            Console.WriteLine("1. Listar Produtos");
            Console.WriteLine("2. Buscar Produto por Código");
            Console.WriteLine("3. Cadastrar Produto");
            Console.WriteLine("4. Editar Produto");
            Console.WriteLine("5. Desativar Produto");
            Console.WriteLine("6. Produtos com Estoque Baixo");
            Console.WriteLine("0. Voltar ao Menu Principal");
            Console.WriteLine();
            Console.Write("Escolha uma opção: ");

            var input = Console.ReadLine();

            try
            {
                switch (input)
                {
                    case "1":
                        await ListarProdutos();
                        break;
                    case "2":
                        await BuscarProdutoPorCodigo();
                        break;
                    case "3":
                        await CadastrarProduto();
                        break;
                    case "4":
                        await EditarProduto();
                        break;
                    case "5":
                        await DesativarProduto();
                        break;
                    case "6":
                        await ListarEstoqueBaixo();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida! Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    private async Task ListarProdutos()
    {
        Console.Clear();
        Console.WriteLine("=== Lista de Produtos ===");
        Console.WriteLine();

        var produtos = await _produtoService.GetAllAsync();

        if (!produtos.Any())
        {
            Console.WriteLine("Nenhum produto cadastrado.");
        }
        else
        {
            Console.WriteLine($"{"Código",-10} {"Nome",-30} {"Tipo",-20} {"Preço Venda",-12} {"Estoque",-10}");
            Console.WriteLine(new string('-', 82));

            foreach (var produto in produtos)
            {
                Console.WriteLine($"{produto.Codigo,-10} {produto.Nome,-30} {produto.TipoProduto?.Nome,-20} {produto.PrecoVenda:C2,-12} {produto.QuantidadeAtual:N2,-10}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    private async Task BuscarProdutoPorCodigo()
    {
        Console.Clear();
        Console.WriteLine("=== Buscar Produto por Código ===");
        Console.WriteLine();
        Console.Write("Digite o código do produto: ");
        var codigo = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(codigo))
        {
            Console.WriteLine("Código não pode estar vazio!");
            Console.ReadKey();
            return;
        }

        var produto = await _produtoService.GetByCodigoAsync(codigo);

        if (produto == null)
        {
            Console.WriteLine($"Produto com código '{codigo}' não encontrado.");
        }
        else
        {
            ExibirDetalhesProduto(produto);
        }

        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    private async Task CadastrarProduto()
    {
        Console.Clear();
        Console.WriteLine("=== Cadastrar Novo Produto ===");
        Console.WriteLine();

        var produto = new Produto
        {
            TipoProdutoId = 1, // Default - seria obtido de uma seleção
            UnidadeMedidaId = 1 // Default - seria obtido de uma seleção
        };

        Console.Write("Código: ");
        produto.Codigo = Console.ReadLine() ?? "";

        Console.Write("Nome: ");
        produto.Nome = Console.ReadLine() ?? "";

        Console.Write("Descrição: ");
        produto.Descricao = Console.ReadLine();

        Console.Write("Preço de Compra: ");
        if (decimal.TryParse(Console.ReadLine(), out var precoCompra))
            produto.PrecoCompra = precoCompra;

        Console.Write("Preço de Venda: ");
        if (decimal.TryParse(Console.ReadLine(), out var precoVenda))
            produto.PrecoVenda = precoVenda;

        Console.Write("Quantidade Mínima: ");
        if (decimal.TryParse(Console.ReadLine(), out var qtdMinima))
            produto.QuantidadeMinima = qtdMinima;

        Console.Write("Quantidade Atual: ");
        if (decimal.TryParse(Console.ReadLine(), out var qtdAtual))
            produto.QuantidadeAtual = qtdAtual;

        if (string.IsNullOrWhiteSpace(produto.Codigo) || string.IsNullOrWhiteSpace(produto.Nome))
        {
            Console.WriteLine("Código e Nome são obrigatórios!");
            Console.ReadKey();
            return;
        }

        var novoProduto = await _produtoService.CreateAsync(produto);
        Console.WriteLine($"Produto '{novoProduto.Nome}' cadastrado com sucesso! ID: {novoProduto.Id}");
        Console.ReadKey();
    }

    private async Task EditarProduto()
    {
        await Task.Run(() =>
        {
            Console.Clear();
            Console.WriteLine("=== Editar Produto ===");
            Console.WriteLine();
            Console.WriteLine("Funcionalidade não implementada ainda.");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        });
    }

    private async Task DesativarProduto()
    {
        Console.Clear();
        Console.WriteLine("=== Desativar Produto ===");
        Console.WriteLine();
        Console.Write("Digite o ID do produto: ");
        
        if (int.TryParse(Console.ReadLine(), out var id))
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null)
            {
                Console.WriteLine($"Produto com ID {id} não encontrado.");
            }
            else
            {
                Console.WriteLine($"Desativando produto: {produto.Nome}");
                await _produtoService.DeleteAsync(id);
                Console.WriteLine("Produto desativado com sucesso!");
            }
        }
        else
        {
            Console.WriteLine("ID inválido!");
        }

        Console.ReadKey();
    }

    private async Task ListarEstoqueBaixo()
    {
        Console.Clear();
        Console.WriteLine("=== Produtos com Estoque Baixo ===");
        Console.WriteLine();

        var produtos = await _produtoService.GetEstoqueBaixoAsync();

        if (!produtos.Any())
        {
            Console.WriteLine("Nenhum produto com estoque baixo.");
        }
        else
        {
            Console.WriteLine($"{"Código",-10} {"Nome",-30} {"Atual",-8} {"Mínimo",-8} {"Status",-15}");
            Console.WriteLine(new string('-', 71));

            foreach (var produto in produtos)
            {
                var status = produto.QuantidadeAtual == 0 ? "SEM ESTOQUE" : "ESTOQUE BAIXO";
                Console.WriteLine($"{produto.Codigo,-10} {produto.Nome,-30} {produto.QuantidadeAtual:N2,-8} {produto.QuantidadeMinima:N2,-8} {status,-15}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    private void ExibirDetalhesProduto(Produto produto)
    {
        Console.WriteLine();
        Console.WriteLine($"ID: {produto.Id}");
        Console.WriteLine($"Código: {produto.Codigo}");
        Console.WriteLine($"Nome: {produto.Nome}");
        Console.WriteLine($"Descrição: {produto.Descricao ?? "N/A"}");
        Console.WriteLine($"Tipo: {produto.TipoProduto?.Nome ?? "N/A"}");
        Console.WriteLine($"Unidade: {produto.UnidadeMedida?.Nome ?? "N/A"}");
        Console.WriteLine($"Preço de Compra: {produto.PrecoCompra:C2}");
        Console.WriteLine($"Preço de Venda: {produto.PrecoVenda:C2}");
        Console.WriteLine($"Quantidade Atual: {produto.QuantidadeAtual:N2}");
        Console.WriteLine($"Quantidade Mínima: {produto.QuantidadeMinima:N2}");
        Console.WriteLine($"Status: {(produto.Ativo ? "Ativo" : "Inativo")}");
        Console.WriteLine($"Data de Criação: {produto.DataCriacao:dd/MM/yyyy HH:mm}");
        Console.WriteLine();
    }
}