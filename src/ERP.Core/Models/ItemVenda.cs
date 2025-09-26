using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Core.Models;

public class ItemVenda
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "decimal(10,3)")]
    public decimal Quantidade { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoUnitario { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Desconto { get; set; } = 0;

    [Column(TypeName = "decimal(10,2)")]
    public decimal SubTotal { get; set; }

    [StringLength(200)]
    public string? Observacoes { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.Now;

    // Foreign Keys
    public int VendaId { get; set; }
    public int ProdutoId { get; set; }

    // Navigation properties
    public virtual Venda Venda { get; set; } = null!;
    public virtual Produto Produto { get; set; } = null!;
}