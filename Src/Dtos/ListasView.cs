using System;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("ListasView")]
public class ListasView : BaseModelApp
{
    [Column("ListaId")]
    public int ListaId { get; set; }

    [Column("PerfilId")]
    public int PerfilId { get; set; }

    [Column("NomeCompleto")]
    public string? NomeCompleto { get; set; }

    [Column("Endereco")]
    public string? Endereco { get; set; }

    [Column("Status")]
    public string? Status { get; set; }

    [Column("EconomiaPrecoTotalSemEntrega")]
    public decimal? EconomiaPrecoTotalSemEntrega { get; set; }

    [Column("EntregaPrecoTotal")]
    public decimal? EntregaPrecoTotal { get; set; }
    
    [Column("EconomiaPrecoTotalComEntrega")]
    public decimal? EconomiaPrecoTotalComEntrega { get; set; }

    public static class StatusConstLista
    {
        public static string EmCriacao = "Em criação";
        public static string AguardandoOrcamentos = "Aguardando orçamentos";
    }

}
