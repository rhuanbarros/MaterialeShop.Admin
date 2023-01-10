using System;
using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("OrcamentoItem")]
public class OrcamentoItem : BaseModelApp
{
    [PrimaryKey("Id", false)] // Key is Autogenerated
    public int Id { get; set; }
    
    [Column("CreatedAt")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    [Required]
    [Column("OrcamentoId")]
    public int OrcamentoId { get; set; }

    [Required]
    [Column("ListaItemId")]
    public int ListaItemId { get; set; }
    
    [Column("Descricao")]
    public string? Descricao { get; set; }
    
    [Column("Quantidade")]
    public int? Quantidade { get; set; }
    
    [Column("UnidadeMedida")]
    public string? UnidadeMedida { get; set; }
    
    [Column("Observacao")]
    public string? Observacao { get; set; }
    
    [Column("Preco")]
    public decimal? Preco { get; set; }
    
    [Column("Desconto")]
    public string? Desconto { get; set; }

}