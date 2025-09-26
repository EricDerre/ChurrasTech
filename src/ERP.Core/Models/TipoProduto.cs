using System.ComponentModel.DataAnnotations;

namespace ERP.Core.Models;

public class TipoProduto
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descricao { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public DateTime? DataAtualizacao { get; set; }

    // Navigation property
    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}