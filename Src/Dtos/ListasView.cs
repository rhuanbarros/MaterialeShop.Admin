using System;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("listasview")]
public class ListasView : BaseModel
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("created_at")]
    public string? CreatedAt { get; set; }

    [Column("nomecompleto")]
    public string? NomeCompleto { get; set; }

    [Column("endereco")]
    public string? Endereco { get; set; }

    [Column("status")]
    public string? Status { get; set; }

}
