using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Core.Models;

public class Venda
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string NumeroVenda { get; set; } = string.Empty;

    public DateTime DataVenda { get; set; } = DateTime.Now;

    [StringLength(200)]
    public string? Cliente { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Desconto { get; set; } = 0;

    [Column(TypeName = "decimal(10,2)")]
    public decimal ValorTotal { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal ValorPago { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Troco { get; set; } = 0;

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
    public virtual ICollection<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
}