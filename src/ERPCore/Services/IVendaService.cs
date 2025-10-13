using ERP.Core.Models;

namespace ERP.Core.Services;

public interface IVendaService
{
    Task<IEnumerable<Venda>> GetAllAsync();
    Task<Venda?> GetByIdAsync(int id);
    Task<Venda?> GetByNumeroAsync(string numeroVenda);
    Task<Venda> CreateAsync(Venda venda);
    Task<Venda> UpdateAsync(Venda venda);
    Task CancelarVendaAsync(int id, string motivo);
    Task<bool> ExistsAsync(int id);
    Task<string> GerarProximoNumeroAsync();
    Task<IEnumerable<Venda>> GetVendasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<decimal> GetTotalVendasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
}