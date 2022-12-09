using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.NovoOrcamentoFolder;

public partial class NovoOrcamentoPage
{    
    [Parameter]
    public int ListaId { get; set; }
    
    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        Func<Loja, bool> predicate = row => {
            if(
                !string.IsNullOrEmpty(row.Nome) && row.Nome.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Cnpj) && row.Cnpj.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Email) && row.Email.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Telefone) && row.Telefone.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Endereco) && row.Endereco.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Cidade) && row.Cidade.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Estado) && row.Estado.ToLower().Contains(text.ToLower())
            )
                return true;
            else
                return false;                
        };        
        _tableListFiltered = _tableList?.Where(predicate).ToList();
    }

    // private void OnValueChangedSearch(string text)
    // {
    //     if(text == "")
    //     {
    //         _tableListFiltered = _tableList;
    //     } else
    //     {
    //         var colunas = typeof(UsuarioPerfil).GetProperties();
    //         _tableListFiltered = new List<UsuarioPerfil>();

    //         // _tableListFiltered = _tableList?.Where(row => row..Contains(text)).ToList();
    //         foreach (var linha in _tableList)
    //         {
    //             foreach (var coluna in colunas)
    //             {
    //                 string value = typeof(UsuarioPerfil)?.GetProperty(coluna.ToString())?.GetValue(linha)?.ToString();
    //                 if(value?.Contains(text) == true)
    //                 {
    //                     _tableListFiltered = (IReadOnlyList<UsuarioPerfil>)_tableListFiltered.Append(linha);
    //                 }
    //             }
                
    //         }
    //     }
    // }

    
}