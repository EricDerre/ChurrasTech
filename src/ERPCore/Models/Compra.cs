using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Core.Models;

public class Compra
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string NumeroCompra { get; set; } = string.Empty;

    public DateTime DataCompra { get; set; } = DateTime.Now;

    [StringLength(200)]
    public string? Fornecedor { get; set; }

    [StringLength(50)]
    public string? NotaFiscal { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Desconto { get; set; } = 0;

    [Column(TypeName = "decimal(10,2)")]
    public decimal ValorTotal { get; set; }

    [StringLength(500)]
    public string? Observacoes { get; set; }

    public bool Cancelada { get; set; } = false;

    public DateTime? DataCancelamento { get; set; }

    [StringLength(500)]
    public string? MotivoCancelamento { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public DateTime? DataAtualizacao { get; set; }

    // Foreign Key
    public int FormaPagamentoId { get; set; }

    // Navigation properties
    public virtual FormaPagamento FormaPagamento { get; set; } = null!;
    public virtual ICollection<ItemCompra> Itens { get; set; } = new List<ItemCompra>();
}