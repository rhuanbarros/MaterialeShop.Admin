using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MaterialeShop.Admin.Src.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.ListasFolder.ListaItensFolder;

public partial class ListaItens
{
    [Parameter]
    public int ListaId { get; set; }

    [Inject] 
    protected ListasViewService ListasViewService {get; set;}

    [Inject] 
    protected ListaItensService ListaItensService {get; set;}

    private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Home", href: "#", icon: Icons.Material.Filled.Home),
        new BreadcrumbItem("Lista", href: Rotas.listas, icon: Icons.Material.Filled.List),
        new BreadcrumbItem("Produtos", href: null, icon: Icons.Material.Filled.List),
    };


    protected override async Task OnParametersSetAsync()
    {
        await GetListaView();
        await GetTable();
    }

    // ---------------- SELECT TABLE
    protected override async Task GetTable()
    {
        _tableList = await ListaItensService.SelectAllByListaId(ListaId);
        _tableListFiltered = _tableList;
        await InvokeAsync(StateHasChanged);
    }


    // ---------------- GET ListaView
    private ListasView _ListaView {get; set;}
    private string NomeCliente = "Carregando";
    private string Endereco = "Carregando";
    protected async Task GetListaView()
    {
        _ListaView = await ListasViewService.SelectAllByListaId(ListaId);
        NomeCliente = _ListaView?.NomeCompleto;
        Endereco = _ListaView?.Endereco;
    }

    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        Func<ListaItem, bool> predicate = row =>
        {
            if (
                !string.IsNullOrEmpty(row.Descricao) && row.Descricao.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Quantidade) && row.Quantidade.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.UnidadeMedida) && row.UnidadeMedida.ToLower().Contains(text.ToLower())
            )
                return true;
            else
                return false;
        };
        _tableListFiltered = _tableList?.Where(predicate).ToList();
    }

    // ---------------- CREATE NEW
    protected override ListaItem SetModelReferenceId(ListaItem item)
    {
        item.ListaId = ListaId;
        return item;
    }

}