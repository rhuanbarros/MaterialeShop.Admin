using System;
using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("ListaItem")]
public class ListaItem : BaseModelApp
{
    [Required]
    [Column("Descricao")]
    public string Descricao { get; set; }

    [Column("Quantidade")]
    public int? Quantidade { get; set; }

    [Column("UnidadeMedida")]
    public string? UnidadeMedida { get; set; }

    [Column("ListaId")]
    public int ListaId { get; set; }

    public ListaItem(ListaItem other)
    {
        Id = other.Id;
        CreatedAt = other.CreatedAt;
        Descricao = other.Descricao;
        Quantidade = other.Quantidade;
        UnidadeMedida = other.UnidadeMedida;
        ListaId = other.ListaId;
    }

    public ListaItem()
    {}

    public ListaItem GetCopy()
    {
        return new ListaItem(this);
    }


}
