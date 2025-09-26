using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Core.Models;

public class ItensUnitarioNoEstoque
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "decimal(10,3)")]
    public decimal Quantidade { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoMedio { get; set; }

    public DateTime DataValidade { get; set; }

    [StringLength(50)]
    public string? Lote { get; set; }

    [StringLength(200)]
    public string? Observacoes { get; set; }

    public DateTime DataEntrada { get; set; } = DateTime.Now;

    public DateTime? DataSaida { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public DateTime? DataAtualizacao { get; set; }

    // Foreign Keys
    public int EstoqueId { get; set; }
    public int ProdutoId { get; set; }

    // Navigation properties
    public virtual Estoque Estoque { get; set; } = null!;
    public virtual Produto Produto { get; set; } = null!;
}