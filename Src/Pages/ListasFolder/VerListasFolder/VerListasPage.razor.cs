using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.ListasFolder.VerListasFolder;

public partial class VerListasPage
{    
    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        _tableListFiltered = _tableList?.Where(row => row.Endereco.Contains(text)).ToList();
    }
    
}