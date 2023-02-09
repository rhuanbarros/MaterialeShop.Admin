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
    protected async Task GetCarrinhoItemView(List<int> carrinhoIdList)
    {
        Console.WriteLine("GetCarrinhoItemView()");

        _tableList = await CarrinhoItemViewService.SelectAllByCarrinhoId(carrinhoIdList);
        
    }

    List<CarrinhoView> _carrinhoViews;
    private async Task GetCarrinhoViewService()
    {
        Console.WriteLine("GetCarrinhoViewService()");
        _carrinhoViews = (List<CarrinhoView>) await CarrinhoViewService.SelectAllByListaId(ListaId, Carrinho.StatusConstCarrinho.EmCriacao);

        //pega os ids dos carrinhos para poder buscar os itens apenas dos carrinhos que vao aparecer na tela
        List<int> carrinhoIdList = _carrinhoViews.Select(x => x.CarrinhoId).ToList();

        await GetCarrinhoItemView(carrinhoIdList);
    }
    
    
    CarrinhoGroupByListaView? _carrinhoGroupByListaView;
    private async Task GetCarrinhoGroupByListaView()
    {
        List<CarrinhoGroupByListaView> carrinhoGroupByListaViews = (List<CarrinhoGroupByListaView>) await CarrinhoGroupByListaViewService.SelectAllByListaId(ListaId);
        _carrinhoGroupByListaView = carrinhoGroupByListaViews?.FirstOrDefault();
    }

    private async void ValueChangedHandler(int newValue, CarrinhoItemView item)
    {
        if(newValue == 0)
        {
            await CarrinhoItemService.SetSoftDeleted(item.CarrinhoItemId);
            Snackbar.Add("Item removido do carrinho com sucesso.");
        } else
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