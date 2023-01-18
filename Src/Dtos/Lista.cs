using System;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("Lista")]
public class Lista : BaseModelApp
{
    [Column("PerfilId")]
    public int? PerfilId { get; set; }

    [Column("Endereco")]
    public string? Endereco { get; set; }

    [Column("Status")]
    public string? Status { get; set; }

    public Lista(Lista other)
    {
        Id = other.Id;
        CreatedAt = other.CreatedAt;
        PerfilId = other.PerfilId;
        Endereco = other.Endereco;
        Status = other.Status;
    }

    public Lista()
    {
    }

    public Lista GetCopy()
    {
        return new Lista(this);
    }

}
