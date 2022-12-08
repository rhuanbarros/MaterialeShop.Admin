using System;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("ListasView")]
public class ListasView : BaseModel
{
    [Column("ListaId")]
    public int ListaId { get; set; }

    [Column("CreatedAt")]
    public string? CreatedAt { get; set; }

    [Column("PerfilId")]
    public int PerfilId { get; set; }

    [Column("NomeCompleto")]
    public string? NomeCompleto { get; set; }

    [Column("Endereco")]
    public string? Endereco { get; set; }

    [Column("Status")]
    public string? Status { get; set; }

    public static class StatusConst
    {
        public static string EmCriacao = "Em criação";
        public static string AguardandoOrcamentos = "Aguardando orçamentos";
    }

}
