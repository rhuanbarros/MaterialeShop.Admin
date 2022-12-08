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
    
    [Column("created_at")]
    public string? CreatedAt { get; set; }

    [Column("UsuarioPerfilId")]
    public int UsuarioPerfilId { get; set; }

    [Column("nomecompleto")]
    public string? NomeCompleto { get; set; }

    [Column("endereco")]
    public string? Endereco { get; set; }

    [Column("status")]
    public string? Status { get; set; }

}
