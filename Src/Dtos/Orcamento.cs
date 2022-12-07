using System;
using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("Orcamento")]
public class Orcamento : BaseModel
{
    [PrimaryKey("id", false)] // Key is Autogenerated
    public int Id { get; set; }
    
    [Column("created_at")]
    public string? CreatedAt { get; set; }

    [Required]
    [Column("ListaId")]
    public int ListaId { get; set; }
    
    [Required]
    [Column("LojaId")]
    public int LojaId { get; set; }
    
    [Column("SolicitacaoData")]
    public DateTime? SolicitacaoData { get; set; }
    
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

}
