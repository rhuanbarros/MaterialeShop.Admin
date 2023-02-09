using Postgrest.Attributes;
using Postgrest.Models;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("CarrinhoItemView")]
public class CarrinhoItemView : BaseModelApp
{    
    [Column("OrcamentoItemId")]
    public int OrcamentoItemId { get; set; }

    [Column("Descricao")]
    public string? Descricao { get; set; }

    [Column("UnidadeMedida")]
    public string? UnidadeMedida { get; set; }

    [Column("OrcamentoItem_Observacao")]
    public string? OrcamentoItem_Observacao { get; set; }

    [Column("Preco")]
    public decimal? Preco { get; set; }

    [Column("Desconto")]
    public string? Desconto { get; set; }
    
    [Column("CarrinhoItemId")]
    public int CarrinhoItemId { get; set; }

    [Column("Quantidade")]
    public int Quantidade { get; set; }

    [Column("CarrinhoItem_Observacao")]
    public string? CarrinhoItem_Observacao { get; set; }

    [Column("CarrinhoId")]
    public int CarrinhoId { get; set; }

}