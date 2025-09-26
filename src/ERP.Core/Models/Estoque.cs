using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Core.Models;

public class Estoque
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descricao { get; set; }

    [StringLength(200)]
    public string? Localizacao { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public DateTime? DataAtualizacao { get; set; }

    // Navigation property
    public virtual ICollection<ItensUnitarioNoEstoque> ItensUnitarios { get; set; } = new List<ItensUnitarioNoEstoque>();
}