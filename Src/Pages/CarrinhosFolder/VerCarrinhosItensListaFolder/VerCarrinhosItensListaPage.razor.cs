using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MaterialeShop.Admin.Src.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.CarrinhosFolder.VerCarrinhosItensListaFolder;

public partial class VerCarrinhosItensListaPage
{
    [Parameter]
    public int ListaId { get; set; }
    
    [Inject] 
    protected CarrinhoItemViewService CarrinhoItemViewService {get; set;}

    // ---------------- SELECT TABLE
    protected override async Task GetTable()
    {
        _tableList = await CarrinhoItemViewService.SelectAll();
        _tableListFiltered = _tableList;
        await InvokeAsync(StateHasChanged);
    }
    

    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        Func<CarrinhoItemView, bool> predicate = row =>
        {
            if (
                !string.IsNullOrEmpty(row.Descricao) && row.Descricao.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.UnidadeMedida) && row.UnidadeMedida.ToLower().Contains(text.ToLower())
            )
                return true;
            else
                return false;
        };
        _tableListFiltered = _tableList?.Where(predicate).ToList();
    }

    // ---------------- CLICK NA LINHA DA TABELA
    private void RowClickEvent(TableRowClickEventArgs<CarrinhoItemView> e)
    {
        // NavigationManager.NavigateTo(Rotas.Orcamentos_lista(e.Item.ListaId));
    }
    
    
}