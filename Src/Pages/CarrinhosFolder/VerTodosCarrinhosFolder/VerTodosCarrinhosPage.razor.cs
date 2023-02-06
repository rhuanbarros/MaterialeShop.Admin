using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MaterialeShop.Admin.Src.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.CarrinhosFolder.VerTodosCarrinhosFolder;

public partial class VerTodosCarrinhosPage
{
    [Inject] 
    protected CarrinhoViewService CarrinhoViewService {get; set;}

    private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Home", href: "#", icon: Icons.Material.Filled.Home),
        new BreadcrumbItem("Carrinhos de compras", href: Rotas.carrinhos, icon: Icons.Material.Filled.AddShoppingCart),
    };

    protected override async Task OnParametersSetAsync()
    {
        await GetTable();
    }

    // ---------------- SELECT TABLE
    protected override async Task GetTable()
    {
        _tableList = await CarrinhoViewService.SelectAllByStatus(Carrinho.StatusConstCarrinho.EmCriacao);
        _tableListFiltered = _tableList;
        await InvokeAsync(StateHasChanged);
    }
    

    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        Func<CarrinhoView, bool> predicate = row =>
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

    // ---------------- CLICK NA LINHA DA TABELA
    private void RowClickEvent(TableRowClickEventArgs<CarrinhoView> e)
    {
        // NavigationManager.NavigateTo(Rotas.Lista(e.Item.ListaId));
    }

}