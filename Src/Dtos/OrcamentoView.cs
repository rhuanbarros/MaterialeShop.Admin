using System;
using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("OrcamentoView")]
public class OrcamentoView : BaseModelApp
{
    [Column("OrcamentoId")]
    public int OrcamentoId { get; set; }

    [Column("LojaId")]
    public int LojaId { get; set; }

    [Column("LojaNome")]
    public string LojaNome { get; set; }

    [Column("ListaId")]
    public int ListaId { get; set; }
    
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

    [Column("PrecoTotalSemEntrega")]
    public decimal? PrecoTotalSemEntrega { get; set; }

    [Column("PrecoTotalComEntrega")]
    public decimal? PrecoTotalComEntrega { get; set; }

    [Column("QuantidadeItens")]
    public int? QuantidadeItens { get; set; }
    
}
