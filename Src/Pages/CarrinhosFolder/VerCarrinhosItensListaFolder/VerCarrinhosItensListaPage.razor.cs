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
    
    [Inject] 
    protected CarrinhoService CarrinhoService {get; set;}
    
    [Inject] 
    protected CarrinhoGroupByListaViewService CarrinhoGroupByListaViewService {get; set;}

    protected override async Task OnParametersSetAsync()
    {
        CarregaDadosAsync();
    }

    private async Task CarregaDadosAsync()
    {
        await GetListaView();
        await GetCarrinhoViewService();
        await GetCarrinhoGroupByListaView();
        await GetTable();
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
        _carrinhoViews = (List<CarrinhoView>) await CarrinhoViewService.SelectAllByListaId(ListaId, Carrinho.StatusConstCarrinho.EmCriacao);
    }
    
    
    CarrinhoGroupByListaView _carrinhoGroupByListaView;
    private async Task GetCarrinhoGroupByListaView()
    {
        List<CarrinhoGroupByListaView> carrinhoGroupByListaViews = (List<CarrinhoGroupByListaView>) await CarrinhoGroupByListaViewService.SelectAllByListaId(ListaId);
        _carrinhoGroupByListaView = carrinhoGroupByListaViews.First();
    }

    private async void ValueChangedHandler(int newValue, CarrinhoItemView item)
    {
        await CarrinhoItemService.SetQuantidade(newValue, item);
        
        CarregaDadosAsync();
    }

    private List<CarrinhoItemView> getCarrinhoItemViewFromCarrinho(int id)
    {
        return ((List<CarrinhoItemView>?)_tableList)?.FindAll( x => x.CarrinhoId == id);
    }
    
    private async Task DeleteCarrinhoClickHandlerAsync(int ListaId)
    {
        await CarrinhoService.SetStatus(Carrinho.StatusConstCarrinho.Cancelado, ListaId);
        Snackbar.Add("Carrinho removido com sucesso.");
        
        CarregaDadosAsync();
    }
}