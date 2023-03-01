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
    protected OrcamentoViewService OrcamentoViewService {get; set;}
    
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
        // await calculaEconomiaTotalAsync();
        await InvokeAsync(StateHasChanged);
    }

    // private decimal economiaTotal;
    // private decimal economiaTotalPercentual;
    // private async Task calculaEconomiaTotalAsync()
    // {
    //     decimal carrinhosTotalTotal = verifyNotNull( _carrinhoGroupByListaView?.PrecoTotal ) + 
    //                                         verifyNotNull(_carrinhoGroupByListaView?.EntregaPrecoTotal);

    //     // decimal? carrinhoMaxTotal = _carrinhoViews.Max(x => x.PrecoTotal + x.EntregaPreco);
    //     OrcamentoView? orcamentoView = await OrcamentoViewService.SelectOrcamentoMaisCaroByListaId(ListaId);
        
    //     Console.WriteLine("orcamentoView.PrecoTotalComEntrega");
    //     Console.WriteLine(orcamentoView?.PrecoTotalComEntrega);

    //     economiaTotal = carrinhosTotalTotal - verifyNotNull(orcamentoView?.PrecoTotalComEntrega);
    //     economiaTotalPercentual = economiaTotal / carrinhosTotalTotal *100 ;
    // }

    private decimal verifyNotNull(decimal? value)
    {
        return (decimal) (value is not null ? value : 0);
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
    
    private string economiaTotal = "Carregando";
    private string economiaTotalPercentual = "Carregando";
    private string EntregaPrecoTotal = "Carregando";
    private string PrecoTotalComEntrega = "Carregando";

    CarrinhoGroupByListaView? _carrinhoGroupByListaView;
    private async Task GetCarrinhoGroupByListaView()
    {
        List<CarrinhoGroupByListaView> carrinhoGroupByListaViews = (List<CarrinhoGroupByListaView>) await CarrinhoGroupByListaViewService.SelectAllByListaId(ListaId);
        _carrinhoGroupByListaView = carrinhoGroupByListaViews?.FirstOrDefault();

        if(_carrinhoGroupByListaView is not null)
        {
            economiaTotal = "R$" + String.Format("{0:0.00}", _carrinhoGroupByListaView.Economia );
            economiaTotalPercentual = String.Format("{0:0.00}", _carrinhoGroupByListaView?.Economia / _carrinhoGroupByListaView?.PrecoTotal )+"%";

            Console.WriteLine("_carrinhoGroupByListaView?.EntregaPrecoTotal");
            Console.WriteLine(_carrinhoGroupByListaView?.EntregaPrecoTotal);

            Console.WriteLine("_carrinhoGroupByListaView?.PrecoTotal");
            Console.WriteLine(_carrinhoGroupByListaView?.PrecoTotal);

            EntregaPrecoTotal = "R$" + String.Format("{0:0.00}", _carrinhoGroupByListaView?.EntregaPrecoTotal is not null ? _carrinhoGroupByListaView?.EntregaPrecoTotal : 0 );
            PrecoTotalComEntrega = "R$" + String.Format("{0:0.00}", _carrinhoGroupByListaView?.PrecoTotal + (_carrinhoGroupByListaView?.EntregaPrecoTotal is not null ? _carrinhoGroupByListaView?.EntregaPrecoTotal : 0 ) );
        } else 
        {
            //caso de não haver nenhum carrinho de compras
            EntregaPrecoTotal = "";
            PrecoTotalComEntrega = "";
        }
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
    
    private async Task DeleteCarrinhoClickHandlerAsync(int CarrinhoId)
    {
        // await CarrinhoService.SetStatus(Carrinho.StatusConstCarrinho.Cancelado, ListaId);
        await CarrinhoService.SetSoftDeleted(CarrinhoId);
        Snackbar.Add("Carrinho removido com sucesso.");
        
        await CarregaDadosAsync();
    }
}