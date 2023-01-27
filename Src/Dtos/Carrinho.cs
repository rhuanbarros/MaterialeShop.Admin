using Postgrest.Attributes;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("Carrinho")]
public class Carrinho : BaseModelApp
{
    [Column("PerfilId")]
    public int PerfilId { get; set; }
    
    [Column("ListaId")]
    public int ListaId { get; set; }
    
    [Column("OrcamentoId")]
    public int OrcamentoId { get; set; }

    [Column("Status")]
    public string? Status { get; set; } = StatusConstCarrinho.EmCriacao;

    public static class StatusConstCarrinho
    {
        public static string EmCriacao = "Em criação";
        public static string Concluido = "Concluído";
        public static string Cancelado = "Cancelado";
    }

    public Carrinho(Carrinho other)
    {
        Id = other.Id;
        CreatedAt = other.CreatedAt;
        SoftDeleted = other.SoftDeleted;
        SoftDeletedAt = other.SoftDeletedAt;

        PerfilId = other.PerfilId;
        ListaId = other.ListaId;
        OrcamentoId = other.OrcamentoId;
        Status = other.Status;
    }

    public Carrinho()
    {
    }

    public Carrinho GetCopy()
    {
        return new Carrinho(this);
    }
    

}