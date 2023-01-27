using System;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("Lista")]
public class Lista : BaseModelApp
{
    // TODO depois q limpar todos os registros do banco de dados, trocar esse campo para nao nulavel e tbm no banco de dados.
    [Column("PerfilId")]
    public int PerfilId { get; set; }

    [Column("Endereco")]
    public string? Endereco { get; set; }

    [Column("Status")]
    public string? Status { get; set; }

    public Lista(Lista other)
    {
        Id = other.Id;
        CreatedAt = other.CreatedAt;
        SoftDeleted = other.SoftDeleted;
        SoftDeletedAt = other.SoftDeletedAt;

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
