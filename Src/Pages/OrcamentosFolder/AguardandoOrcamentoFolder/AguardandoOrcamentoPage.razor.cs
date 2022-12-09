using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.AguardandoOrcamentoFolder;

public partial class AguardandoOrcamentoPage
{
    [Inject] 
    protected ListasViewService ListasViewService {get; set;}

    // ---------------- SELECT TABLE
    protected override async Task GetTable()
    {
        _tableList = await ListasViewService.SelectAllByStatus(ListasView.StatusConst.AguardandoOrcamentos);
        _tableListFiltered = _tableList;
        await InvokeAsync(StateHasChanged);
    }
    

    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        Func<ListasView, bool> predicate = row =>
        {
            if (
                !string.IsNullOrEmpty(row.NomeCompleto) && row.NomeCompleto.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Endereco) && row.Endereco.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Status.ToString()) && row.Status.ToString().ToLower().Contains(text.ToLower())
            )
                return true;
            else
                return false;
        };
        _tableListFiltered = _tableList?.Where(predicate).ToList();
    }


}