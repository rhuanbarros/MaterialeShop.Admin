using System;
using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("Orcamento")]
public class Orcamento : BaseModelApp
{
    [Required]
    [Column("ListaId")]
    public int ListaId { get; set; }

    [Required]
    [Column("LojaId")]
    public int LojaId { get; set; }

    [Column("SolicitacaoData")]
    public DateTime? SolicitacaoData { get; set; }

    [Column("SolicitacaoHora")]
    public TimeSpan? SolicitacaoHora { get; set; }

    [Column("Recebido")]
    public bool? Recebido { get; set; }

    [Column("RecebidoData")]
    public DateTime? RecebidoData { get; set; }

    [Column("EntregaPreco")]
    public decimal? EntregaPreco { get; set; }

    [Column("EntregaPrazo")]
    public string? EntregaPrazo { get; set; }

    [Column("DescontoNoTotal")]
    public string? DescontoNoTotal { get; set; }

    [Column("OrcamentoAnexo")]
    public string? OrcamentoAnexo { get; set; }

    [Column("CodigoLoja")]
    public string? CodigoLoja { get; set; }

    public Orcamento(Orcamento other)
    {
        Id = other.Id;
        CreatedAt = other.CreatedAt;
        SoftDeleted = other.SoftDeleted;
        SoftDeletedAt = other.SoftDeletedAt;

        ListaId = other.ListaId;
        LojaId = other.LojaId;
        SolicitacaoData = other.SolicitacaoData;
        SolicitacaoHora = other.SolicitacaoHora;
        Recebido = other.Recebido;
        RecebidoData = other.RecebidoData;
        EntregaPreco = other.EntregaPreco;
        EntregaPrazo = other.EntregaPrazo;
        DescontoNoTotal = other.DescontoNoTotal;
        OrcamentoAnexo = other.OrcamentoAnexo;
        CodigoLoja = other.CodigoLoja;
    }

    public Orcamento()
    { }

    public Orcamento GetCopy()
    {
        return new Orcamento(this);
    }

}
