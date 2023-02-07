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
    protected ListasViewService ListasViewService {get; set;}
    
    [Inject] 
    protected CarrinhoItemViewService CarrinhoItemViewService {get; set;}
    
    [Inject] 
    protected CarrinhoItemService CarrinhoItemService {get; set;}
    
    [Inject] 
    protected CarrinhoViewService CarrinhoViewService {get; set;}

    protected override async Task OnParametersSetAsync()
    {
        await GetCarrinhoViewService();
        await GetTable();
        await GetListaView();
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

    // ---------------- SELECT TABLE
    protected override async Task GetTable()
    {
        _tableList = await CarrinhoItemViewService.SelectAll();
        
    }

    List<CarrinhoView> _carrinhoViews;
    private async Task GetCarrinhoViewService()
    {
        _carrinhoViews = (List<CarrinhoView>) await CarrinhoViewService.SelectAllByListaId(ListaId);
    }

    private async void ValueChangedHandler(int newValue, CarrinhoItemView item)
    {
        await CarrinhoItemService.SetQuantidade(newValue, item);
        
        await GetTable();
        await InvokeAsync(StateHasChanged);
    }

    private List<CarrinhoItemView> getCarrinhoItemViewFromCarrinho(int id)
    {
        return ((List<CarrinhoItemView>?)_tableList)?.FindAll( x => x.CarrinhoId == id);
    }
    
}