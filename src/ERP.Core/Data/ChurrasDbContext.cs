using Microsoft.EntityFrameworkCore;
using ERP.Core.Models;

namespace ERP.Core.Data;

// Contexto principal do banco de dados do sistema ChurrasTech ERP
public class ChurrasDbContext : DbContext
{
    // Construtor que recebe as opções de configuração do contexto
    public ChurrasDbContext(DbContextOptions<ChurrasDbContext> options) : base(options)
    {
    }

    // Tabelas do banco de dados
    public DbSet<TipoProduto> TiposProduto { get; set; }
    public DbSet<UnidadeMedida> UnidadesMedida { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<FormaPagamento> FormasPagamento { get; set; }
    public DbSet<Venda> Vendas { get; set; }
    public DbSet<ItemVenda> ItensVenda { get; set; }
    public DbSet<Compra> Compras { get; set; }
    public DbSet<ItemCompra> ItensCompra { get; set; }
    public DbSet<Estoque> Estoques { get; set; }
    public DbSet<ItensUnitarioNoEstoque> ItensUnitarioNoEstoque { get; set; }

    // Configuração dos relacionamentos e restrições do modelo
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configura os relacionamentos entre entidades
        modelBuilder.Entity<Produto>()
            .HasOne(p => p.TipoProduto)
            .WithMany(tp => tp.Produtos)
            .HasForeignKey(p => p.TipoProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Produto>()
            .HasOne(p => p.UnidadeMedida)
            .WithMany(um => um.Produtos)
            .HasForeignKey(p => p.UnidadeMedidaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Venda>()
            .HasOne(v => v.FormaPagamento)
            .WithMany(fp => fp.Vendas)
            .HasForeignKey(v => v.FormaPagamentoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ItemVenda>()
            .HasOne(iv => iv.Venda)
            .WithMany(v => v.Itens)
            .HasForeignKey(iv => iv.VendaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ItemVenda>()
            .HasOne(iv => iv.Produto)
            .WithMany(p => p.ItensVenda)
            .HasForeignKey(iv => iv.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Compra>()
            .HasOne(c => c.FormaPagamento)
            .WithMany(fp => fp.Compras)
            .HasForeignKey(c => c.FormaPagamentoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ItemCompra>()
            .HasOne(ic => ic.Compra)
            .WithMany(c => c.Itens)
            .HasForeignKey(ic => ic.CompraId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ItemCompra>()
            .HasOne(ic => ic.Produto)
            .WithMany(p => p.ItensCompra)
            .HasForeignKey(ic => ic.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ItensUnitarioNoEstoque>()
            .HasOne(iue => iue.Estoque)
            .WithMany(e => e.ItensUnitarios)
            .HasForeignKey(iue => iue.EstoqueId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ItensUnitarioNoEstoque>()
            .HasOne(iue => iue.Produto)
            .WithMany(p => p.ItensEstoque)
            .HasForeignKey(iue => iue.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configura restrições de unicidade
        modelBuilder.Entity<Produto>()
            .HasIndex(p => p.Codigo)
            .IsUnique();

        modelBuilder.Entity<TipoProduto>()
            .HasIndex(tp => tp.Nome)
            .IsUnique();

        modelBuilder.Entity<UnidadeMedida>()
            .HasIndex(um => um.Sigla)
            .IsUnique();

        modelBuilder.Entity<FormaPagamento>()
            .HasIndex(fp => fp.Nome)
            .IsUnique();

        modelBuilder.Entity<Venda>()
            .HasIndex(v => v.NumeroVenda)
            .IsUnique();

        modelBuilder.Entity<Compra>()
            .HasIndex(c => c.NumeroCompra)
            .IsUnique();

        // Adiciona dados iniciais (seed)
        SeedData(modelBuilder);
    }

    // Método para popular o banco com dados iniciais
    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Tipos de produto
        modelBuilder.Entity<TipoProduto>().HasData(
            new TipoProduto { Id = 1, Nome = "Carnes", Descricao = "Carnes para churrasco" },
            new TipoProduto { Id = 2, Nome = "Acompanhamentos", Descricao = "Acompanhamentos e guarnições" },
            new TipoProduto { Id = 3, Nome = "Bebidas", Descricao = "Bebidas em geral" },
            new TipoProduto { Id = 4, Nome = "Temperos", Descricao = "Temperos e condimentos" }
        );

        // Unidades de medida
        modelBuilder.Entity<UnidadeMedida>().HasData(
            new UnidadeMedida { Id = 1, Sigla = "KG", Nome = "Quilograma" },
            new UnidadeMedida { Id = 2, Sigla = "UN", Nome = "Unidade" },
            new UnidadeMedida { Id = 3, Sigla = "L", Nome = "Litro" },
            new UnidadeMedida { Id = 4, Sigla = "G", Nome = "Grama" }
        );

        // Formas de pagamento
        modelBuilder.Entity<FormaPagamento>().HasData(
            new FormaPagamento { Id = 1, Nome = "Dinheiro", AceitaTroco = true },
            new FormaPagamento { Id = 2, Nome = "Cartão de Débito", AceitaTroco = false },
            new FormaPagamento { Id = 3, Nome = "Cartão de Crédito", AceitaTroco = false },
            new FormaPagamento { Id = 4, Nome = "PIX", AceitaTroco = false }
        );

        // Estoque principal
        modelBuilder.Entity<Estoque>().HasData(
            new Estoque { Id = 1, Nome = "Estoque Principal", Descricao = "Estoque principal da churrascaria", Localizacao = "Depósito 1" }
        );
    }
}