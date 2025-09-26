using System.ComponentModel.DataAnnotations;

namespace ERP.Core.Models;

public class FormaPagamento
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Descricao { get; set; }

    public bool AceitaTroco { get; set; } = false;

    public bool Ativo { get; set; } = true;

    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public DateTime? DataAtualizacao { get; set; }

    // Navigation properties
    public virtual ICollection<Venda> Vendas { get; set; } = new List<Venda>();
    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}