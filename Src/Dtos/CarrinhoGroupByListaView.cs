using Postgrest.Attributes;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("CarrinhoGroupByListaView")]
public class CarrinhoGroupByListaView : BaseModelApp
{
    [Column("ListaId")]
    public int ListaId { get; set; }
    
    [Column("PerfilId")]
    public int PerfilId { get; set; }
    
    [Column("NomeCompleto")]
    public string NomeCompleto { get; set; }

    [Column("Endereco")]
    public string? Endereco { get; set; }

    [Column("ListaCreatedAt")]
    public DateTime ListaCreatedAt { get; set; }

    [Column("Lojas")]
    public string Lojas { get; set; }

    [Column("EntregaPrecoMinimo")]
    public decimal? EntregaPrecoTotal { get; set; }
    
    [Column("EntregaPrazoMinimo")]
    public string? EntregaPrazoMinimo { get; set; }

    [Column("PrecoTotal")]
    public decimal? PrecoTotal { get; set; }

    [Column("QuantidadeItens")]
    public int? QuantidadeItens { get; set; }
}