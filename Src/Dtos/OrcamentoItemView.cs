using System;
using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("OrcamentoItemView")]
public class OrcamentoItemView : BaseModelApp
{
    [Required]
    [Column("OrcamentoId")]
    public int OrcamentoId { get; set; }

    [Column("ListaItemId")]
    public int? ListaItemId { get; set; }

    [Column("OrcamentoItem_Descricao")]
    public string? OrcamentoItem_Descricao { get; set; }

    [Column("OrcamentoItem_Quantidade")]
    public int? OrcamentoItem_Quantidade { get; set; }

    [Column("UnidadeMedida")]
    public string? UnidadeMedida { get; set; }

    [Column("Observacao")]
    public string? Observacao { get; set; }

    [Column("Preco")]
    public decimal? Preco { get; set; }

    [Column("Desconto")]
    public string? Desconto { get; set; }

    [Column("ListaId")]
    public int ListaId { get; set; }

    [Column("ListaItem_Descricao")]
    public string ListaItem_Descricao { get; set; }

    [Column("ListaItem_Quantidade")]
    public int? ListaItem_Quantidade { get; set; }

}