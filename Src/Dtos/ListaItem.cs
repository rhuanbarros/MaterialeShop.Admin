using System;
using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("ListaItem")]
public class ListaItem : BaseModel
{
    [PrimaryKey("Id", false)] // Key is Autogenerated
    public int Id { get; set; }
    
    [Column("CreatedAt")]
    public string? CreatedAt { get; set; }

    [Required]
    [Column("Descricao")]
    public string Descricao { get; set; }

    [Column("Quantidade")]
    public string? Quantidade { get; set; }

    [Column("UnidadeMedida")]
    public string? UnidadeMedida { get; set; }

    [Column("ListaId")]
    public int ListaId { get; set; }
}
