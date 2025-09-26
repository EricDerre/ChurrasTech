using Microsoft.EntityFrameworkCore;
using ERP.Core.Data;
using ERP.Core.Models;

namespace ERP.Core.Services;

public class ProdutoService : IProdutoService
{
    private readonly ChurrasDbContext _context;

    public ProdutoService(ChurrasDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> GetAllAsync()
    {
        return await _context.Produtos
            .Include(p => p.TipoProduto)
            .Include(p => p.UnidadeMedida)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task<Produto?> GetByIdAsync(int id)
    {
        return await _context.Produtos
            .Include(p => p.TipoProduto)
            .Include(p => p.UnidadeMedida)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Produto?> GetByCodigoAsync(string codigo)
    {
        return await _context.Produtos
            .Include(p => p.TipoProduto)
            .Include(p => p.UnidadeMedida)
            .FirstOrDefaultAsync(p => p.Codigo == codigo);
    }

    public async Task<Produto> CreateAsync(Produto produto)
    {
        if (await CodigoExistsAsync(produto.Codigo))
        {
            throw new ArgumentException($"Código '{produto.Codigo}' já existe");
        }

        produto.DataCriacao = DateTime.Now;
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task<Produto> UpdateAsync(Produto produto)
    {
        if (await CodigoExistsAsync(produto.Codigo, produto.Id))
        {
            throw new ArgumentException($"Código '{produto.Codigo}' já existe");
        }

        produto.DataAtualizacao = DateTime.Now;
        _context.Entry(produto).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task DeleteAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto != null)
        {
            produto.Ativo = false;
            produto.DataAtualizacao = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Produtos.AnyAsync(p => p.Id == id);
    }

    public async Task<bool> CodigoExistsAsync(string codigo, int? excludeId = null)
    {
        var query = _context.Produtos.Where(p => p.Codigo == codigo);
        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }
        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Produto>> GetByTipoAsync(int tipoProdutoId)
    {
        return await _context.Produtos
            .Include(p => p.TipoProduto)
            .Include(p => p.UnidadeMedida)
            .Where(p => p.TipoProdutoId == tipoProdutoId && p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Produto>> GetEstoqueBaixoAsync()
    {
        return await _context.Produtos
            .Include(p => p.TipoProduto)
            .Include(p => p.UnidadeMedida)
            .Where(p => p.Ativo && p.QuantidadeAtual <= p.QuantidadeMinima)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }
}