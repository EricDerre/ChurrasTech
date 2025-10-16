using System.ComponentModel.DataAnnotations;

namespace ERP.Core.Models;

public class UnidadeMedida
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(10)]
    public string Sigla { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Descricao { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public DateTime? DataAtualizacao { get; set; }

    // Navigation property
    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}