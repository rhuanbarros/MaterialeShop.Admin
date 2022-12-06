using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.LojasFolder;

public partial class LojasPage
{    
    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        _tableListFiltered = _tableList?.Where(row => row.Nome.Contains(text)).ToList();
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