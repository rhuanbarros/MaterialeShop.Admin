using System;
using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("Loja")]
public class Loja : BaseModelApp
{
    [Required]
    [Column("Nome")]
    public string Nome { get; set; }

    [Column("Cnpj")]
    public string? Cnpj { get; set; }

    [Column("Email")]
    public string? Email { get; set; }

    [Column("Telefone")]
    public string? Telefone { get; set; }

    [Column("Endereco")]
    public string? Endereco { get; set; }

    [Column("Cidade")]
    public string? Cidade { get; set; }

    [Column("Estado")]
    public string? Estado { get; set; }

    public Loja(Loja other)
    {
        Id = other.Id;
        CreatedAt = other.CreatedAt;
        SoftDeleted = other.SoftDeleted;
        SoftDeletedAt = other.SoftDeletedAt;

        Nome = other.Nome;
        Cnpj = other.Cnpj;
        Email = other.Email;
        Telefone = other.Telefone;
        Endereco = other.Endereco;
        Cidade = other.Cidade;
        Estado = other.Estado;
    }

    public Loja()
    {}

    public Loja GetCopy()
    {
        return new Loja(this);
    }

}
