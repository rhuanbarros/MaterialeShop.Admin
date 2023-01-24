using System;
using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("OrcamentoItem")]
public class OrcamentoItem : BaseModelApp
{
    [Required]
    [Column("OrcamentoId")]
    public int OrcamentoId { get; set; }

    [Column("ListaItemId")]
    public int? ListaItemId { get; set; }

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

    public OrcamentoItem(OrcamentoItem other)
    {
        Id = other.Id;
        CreatedAt = other.CreatedAt;
        SoftDeleted = other.SoftDeleted;
        SoftDeletedAt = other.SoftDeletedAt;

        OrcamentoId = other.OrcamentoId;
        ListaItemId = other.ListaItemId;
        Descricao = other.Descricao;
        Quantidade = other.Quantidade;
        UnidadeMedida = other.UnidadeMedida;
        Observacao = other.Observacao;
        Preco = other.Preco;
        Desconto = other.Desconto;
    }

    public OrcamentoItem()
    { }

    public OrcamentoItem GetCopy()
    {
        return new OrcamentoItem(this);
    }

}