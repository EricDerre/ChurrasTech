using Microsoft.EntityFrameworkCore;
using ERP.Core.Data;
using ERP.Core.Models;
using ERP.Core.Services;

namespace ERP.Tests;

public class ProdutoServiceTests : IDisposable
{
    private readonly ChurrasDbContext _context;
    private readonly ProdutoService _produtoService;

    public ProdutoServiceTests()
    {
        var options = new DbContextOptionsBuilder<ChurrasDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ChurrasDbContext(options);
        _produtoService = new ProdutoService(_context);

        // Seed test data
        SeedTestData();
    }

    private void SeedTestData()
    {
        var tipoProduto = new TipoProduto { Id = 1, Nome = "Carnes" };
        var unidadeMedida = new UnidadeMedida { Id = 1, Sigla = "KG", Nome = "Quilograma" };

        _context.TiposProduto.Add(tipoProduto);
        _context.UnidadesMedida.Add(unidadeMedida);
        _context.SaveChanges();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateProduct_WhenDataIsValid()
    {
        // Arrange
        var produto = new Produto
        {
            Codigo = "P001",
            Nome = "Picanha",
            PrecoCompra = 50.00m,
            PrecoVenda = 80.00m,
            QuantidadeMinima = 5,
            QuantidadeAtual = 10,
            TipoProdutoId = 1,
            UnidadeMedidaId = 1
        };

        // Act
        var result = await _produtoService.CreateAsync(produto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("P001", result.Codigo);
        Assert.Equal("Picanha", result.Nome);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenCodigoAlreadyExists()
    {
        // Arrange
        var produto1 = new Produto
        {
            Codigo = "P002",
            Nome = "Alcatra",
            TipoProdutoId = 1,
            UnidadeMedidaId = 1
        };

        var produto2 = new Produto
        {
            Codigo = "P002", // Same code
            Nome = "Maminha",
            TipoProdutoId = 1,
            UnidadeMedidaId = 1
        };

        await _produtoService.CreateAsync(produto1);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _produtoService.CreateAsync(produto2));
    }

    [Fact]
    public async Task GetByCodigoAsync_ShouldReturnProduct_WhenExists()
    {
        // Arrange
        var produto = new Produto
        {
            Codigo = "P003",
            Nome = "Fraldinha",
            TipoProdutoId = 1,
            UnidadeMedidaId = 1
        };

        await _produtoService.CreateAsync(produto);

        // Act
        var result = await _produtoService.GetByCodigoAsync("P003");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("P003", result.Codigo);
        Assert.Equal("Fraldinha", result.Nome);
    }

    [Fact]
    public async Task GetByCodigoAsync_ShouldReturnNull_WhenNotExists()
    {
        // Act
        var result = await _produtoService.GetByCodigoAsync("INEXISTENTE");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetEstoqueBaixoAsync_ShouldReturnProductsWithLowStock()
    {
        // Arrange
        var produtoComEstoqueBom = new Produto
        {
            Codigo = "P004",
            Nome = "Costela",
            QuantidadeAtual = 20,
            QuantidadeMinima = 5,
            TipoProdutoId = 1,
            UnidadeMedidaId = 1
        };

        var produtoComEstoqueBaixo = new Produto
        {
            Codigo = "P005",
            Nome = "Linguiça",
            QuantidadeAtual = 2,
            QuantidadeMinima = 5,
            TipoProdutoId = 1,
            UnidadeMedidaId = 1
        };

        await _produtoService.CreateAsync(produtoComEstoqueBom);
        await _produtoService.CreateAsync(produtoComEstoqueBaixo);

        // Act
        var result = await _produtoService.GetEstoqueBaixoAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("P005", result.First().Codigo);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
