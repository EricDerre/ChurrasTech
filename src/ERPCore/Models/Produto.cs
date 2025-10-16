using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Core.Models;

public class Produto
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Codigo { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Descricao { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoCompra { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoVenda { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal QuantidadeMinima { get; set; }

    [Column(TypeName = "decimal(10,3)")]
    public decimal QuantidadeAtual { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public DateTime? DataAtualizacao { get; set; }

    // Foreign Keys
    public int TipoProdutoId { get; set; }
    public int UnidadeMedidaId { get; set; }

    // Navigation properties
    public virtual TipoProduto TipoProduto { get; set; } = null!;
    public virtual UnidadeMedida UnidadeMedida { get; set; } = null!;
    public virtual ICollection<ItemVenda> ItensVenda { get; set; } = new List<ItemVenda>();
    public virtual ICollection<ItemCompra> ItensCompra { get; set; } = new List<ItemCompra>();
    public virtual ICollection<ItensUnitarioNoEstoque> ItensEstoque { get; set; } = new List<ItensUnitarioNoEstoque>();
}