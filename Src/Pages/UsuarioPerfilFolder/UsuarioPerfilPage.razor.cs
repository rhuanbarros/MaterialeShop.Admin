using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.UsuarioPerfilFolder;

public partial class UsuarioPerfilPage
{    
    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        Func<UsuarioPerfil, bool> predicate = row => {
            if(
                !string.IsNullOrEmpty(row.Email) && row.Email.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.NomeCompleto) && row.NomeCompleto.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Cpf) && row.Cpf.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Telefone) && row.Telefone.ToLower().Contains(text.ToLower())
            )
                return true;
            else
                return false;                
        };        
        _tableListFiltered = _tableList?.Where(predicate).ToList();
    }
    
}