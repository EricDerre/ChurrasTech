using ERP.Core.Models;

namespace ERP.Core.Services;

public interface IProdutoService
{
    Task<IEnumerable<Produto>> GetAllAsync();
    Task<Produto?> GetByIdAsync(int id);
    Task<Produto?> GetByCodigoAsync(string codigo);
    Task<Produto> CreateAsync(Produto produto);
    Task<Produto> UpdateAsync(Produto produto);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> CodigoExistsAsync(string codigo, int? excludeId = null);
    Task<IEnumerable<Produto>> GetByTipoAsync(int tipoProdutoId);
    Task<IEnumerable<Produto>> GetEstoqueBaixoAsync();
}