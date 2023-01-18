using System;
using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;

namespace MaterialeShop.Admin.Src.Dtos;

[Table("Perfil")]
public class Perfil : BaseModelApp
{
    [Column("Uuid")]
    public string? Uuid { get; set; }

    [Column("Email")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [Column("NomeCompleto")]
    public string NomeCompleto { get; set; }

    [Column("Cpf")]
    public string? Cpf { get; set; }

    [Column("Telefone")]
    public string? Telefone { get; set; }

    // Note: this is important so the MudSelect can compare 
    public override bool Equals(object o)
    {
        var other = o as Perfil;
        return other?.Id == Id;
    }

    // Note: this is important too!
    public override int GetHashCode() => NomeCompleto?.GetHashCode() ?? 0;

    // Implement this to display correctly in MudSelect
    public override string ToString() => NomeCompleto;

    public Perfil GetCopy()
    {
        return new Perfil(this);
    }

    public Perfil(Perfil other)
    {
        Id = other.Id;
        Uuid = other.Uuid;
        CreatedAt = other.CreatedAt;
        Email = other.Email;
        NomeCompleto = other.NomeCompleto;
        Cpf = other.Cpf;
        Telefone = other.Telefone;
    }

    public Perfil()
    {
    }

}
