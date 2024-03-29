using Postgrest.Attributes;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("CarrinhoItem")]
public class CarrinhoItem : BaseModelApp
{
    // TODO acho q eu preciso mudar esses campos para serem nao nulaveis, mas tem q ver isso se noa vai quebrar o codigo.
    [Column("CarrinhoId")]
    public int? CarrinhoId { get; set; }
    
    [Column("OrcamentoItemId")]
    public int? OrcamentoItemId { get; set; }

    [Column("Quantidade")]
    public int Quantidade { get; set; }

    [Column("Observacao")]
    public string? Observacao { get; set; }

    public CarrinhoItem(int? carrinhoId, int? orcamentoItemId, int? quantidade, string? observacao)
    {
        CarrinhoId = carrinhoId;
        OrcamentoItemId = orcamentoItemId;
        Quantidade = quantidade != null ? (int) quantidade : 0;
        Observacao = observacao;
    }

    public CarrinhoItem(CarrinhoItem other)
    {
        Id = other.Id;
        CreatedAt = other.CreatedAt;
        SoftDeleted = other.SoftDeleted;
        SoftDeletedAt = other.SoftDeletedAt;

        CarrinhoId = other.CarrinhoId;
        OrcamentoItemId = other.OrcamentoItemId;
        Quantidade = other.Quantidade;
        Observacao = other.Observacao;
    }

    public CarrinhoItem()
    {
    }

    public CarrinhoItem GetCopy()
    {
        return new CarrinhoItem(this);
    }

}