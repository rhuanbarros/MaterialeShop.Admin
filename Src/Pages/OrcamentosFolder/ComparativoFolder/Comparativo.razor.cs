using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.ComparativoFolder;

public partial class Comparativo
{
    [Parameter]
    public int ListaId { get; set; }

    [Inject]
    ListaItensService ListaItensService { get; set; }

    [Inject]
    OrcamentoViewService OrcamentoViewService { get; set; }

    [Inject]
    OrcamentoItemService OrcamentoItemService { get; set; }

    private int quantidade_linhas {get; set;}
    private IEnumerator<ListaItem> _ListaItemListEnumerator;

    protected override async Task OnParametersSetAsync()
    {
        await GetListaItem(ListaId);
        await GetOrcamentoView(ListaId);
        await GetOrcamentoItemList(_OrcamentoViewList);

        // retorna o maior valor entre as variaveis. 
        // no caso, retona a quantidade maior de linhas que serão necessárias inserir na tabela
        quantidade_linhas = new [] { _ListaItemList.Count, _OrcamentoViewList.Count, _OrcamentoItemList.Count}.Max();

        Console.WriteLine("================================================================");
        Console.WriteLine("Quantidade de item em cada lista");
        Console.WriteLine("_ListaItemList.Count");
        Console.WriteLine(_ListaItemList.Count);
        Console.WriteLine("_OrcamentoViewList.Count");
        Console.WriteLine(_OrcamentoViewList.Count);
        Console.WriteLine("_OrcamentoItemList.Count");
        Console.WriteLine(_OrcamentoItemList.Count);
        Console.WriteLine("================================================================");

        _ListaItemListEnumerator = _ListaItemListIterator().GetEnumerator();

        // ListaItem item = new();
        // bool temMais = _ListaItemListEnumerator.MoveNext();
        // if(temMais)
        // {
        //     item = _ListaItemListEnumerator.Current;
        // }
        // Console.WriteLine("item");
        // Console.WriteLine(item.Quantidade);
        // Console.WriteLine(item.Descricao);
        
        // temMais = _ListaItemListEnumerator.MoveNext();
        // if(temMais)
        // {
        //     item = _ListaItemListEnumerator.Current;
        // }
        // Console.WriteLine("item");
        // Console.WriteLine(item.Quantidade);
        // Console.WriteLine(item.Descricao);

        await InvokeAsync(StateHasChanged);
    }
    
    // ---------------- SELECT TABLE ListaItem
    protected List<ListaItem>? _ListaItemList { get; set; } = new();
    protected async Task GetListaItem(int ListaId)
    {
        _ListaItemList = (List<ListaItem>?) await ListaItensService.SelectAllByListaId(ListaId);
        // await InvokeAsync(StateHasChanged);
    }


    // ---------------- SELECT TABLE OrcamentoView
    protected List<OrcamentoView>? _OrcamentoViewList { get; set; } = new();
    protected async Task GetOrcamentoView(int ListaId)
    {
        _OrcamentoViewList = (List<OrcamentoView>?) await OrcamentoViewService.SelectAllByListaId(ListaId);
        // await InvokeAsync(StateHasChanged);
    }

    // ---------------- SELECT TABLE OrcamentoItem
    protected List<List<OrcamentoItem>>? _OrcamentoItemList { get; set; } = new();
    protected async Task GetOrcamentoItemList(List<OrcamentoView>? OrcamentoViewList)
    {
        _OrcamentoItemList.Clear();
        foreach (var item in OrcamentoViewList)
        {
            List<OrcamentoItem>? aux = (List<OrcamentoItem>?) await OrcamentoItemService.SelectByOrcamentoId(item.OrcamentoId);
            _OrcamentoItemList.Add(aux);
        }
    }

    public IEnumerable<ListaItem> _ListaItemListIterator()
    {
        foreach (var item in _ListaItemList)
        {
            yield return item;
        }
    }
    
}