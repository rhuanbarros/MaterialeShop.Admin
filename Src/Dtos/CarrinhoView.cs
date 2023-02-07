using Postgrest.Attributes;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("CarrinhoView")]
public class CarrinhoView : BaseModelApp
{
    [Column("PerfilId")]
    public int PerfilId { get; set; }

    [Column("NomeCompleto")]
    public string NomeCompleto { get; set; }
    
    [Column("ListaId")]
    public int ListaId { get; set; }

    [Column("Endereco")]
    public string? Endereco { get; set; }

    [Column("ListaCreatedAt")]
    public DateTime ListaCreatedAt { get; set; }
    
    [Column("OrcamentoId")]
    public int OrcamentoId { get; set; }

    [Column("LojaNome")]
    public string LojaNome { get; set; }

    [Column("EntregaPreco")]
    public decimal? EntregaPreco { get; set; }
    
    [Column("EntregaPrazo")]
    public string? EntregaPrazo { get; set; }

    [Column("Status")]
    public string? Status { get; set; } = Carrinho.StatusConstCarrinho.EmCriacao;

    [Column("CarrinhoId")]
    public int CarrinhoId { get; set; }

    [Column("PrecoTotal")]
    public decimal? PrecoTotal { get; set; }

    [Column("QuantidadeItens")]
    public int? QuantidadeItens { get; set; }
}