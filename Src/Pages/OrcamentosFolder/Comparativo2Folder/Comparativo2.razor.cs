using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.Comparativo2Folder;

public partial class Comparativo2
{
    [Parameter]
    public int ListaId { get; set; }

    [Inject]
    ListaItensService ListaItensService { get; set; }

    [Inject]
    OrcamentoViewService OrcamentoViewService { get; set; }

    [Inject]
    OrcamentoItemService OrcamentoItemService { get; set; }

    public IEnumerator<ListaItem>? listaItemEnumerator;

    public List<IEnumerator<OrcamentoItem>>? orcamentoItemEnumeratorList = new();

    public int? maisBaratoOrcamentoId;
    public int? maisQuantidadeItensOrcamentoId;
    public int? maior_quantidade_itens;

    protected override async Task OnParametersSetAsync()
    {
        await GetListaItem(ListaId);
        await GetOrcamentoView(ListaId);
        await GetOrcamentoItemList(_OrcamentoViewList);

        listaItemEnumerator = GetSequentialObjects<ListaItem>(_ListaItemList);
        
        // foreach (var orcamentoItemList in _OrcamentoItemListList)
        // {
        //     var enumerator = GetSequentialObjects<OrcamentoItem>(orcamentoItemList);
        //     orcamentoItemEnumeratorList.Add(enumerator);
        // }

        maisBaratoOrcamentoId = getMaisBarato(_OrcamentoViewList);
        maisQuantidadeItensOrcamentoId = getMaisQuantidadeItens(_OrcamentoViewList);
        maior_quantidade_itens = _OrcamentoViewList.Find( x => x.Id == maisQuantidadeItensOrcamentoId)?.QuantidadeItens;

        await InvokeAsync(StateHasChanged);
    }

    // ---------------- SELECT TABLE ListaItem
    protected List<ListaItem>? _ListaItemList { get; set; } = new();
    protected async Task GetListaItem(int ListaId)
    {
        _ListaItemList = (List<ListaItem>?)await ListaItensService.SelectAllByListaId(ListaId);
        // await InvokeAsync(StateHasChanged);
    }


    // ---------------- SELECT TABLE OrcamentoView
    protected List<OrcamentoView>? _OrcamentoViewList { get; set; } = new();
    protected async Task GetOrcamentoView(int ListaId)
    {
        _OrcamentoViewList = (List<OrcamentoView>?)await OrcamentoViewService.SelectAllByListaId(ListaId);
        // await InvokeAsync(StateHasChanged);
    }

    // ---------------- SELECT TABLE OrcamentoItem
    // protected List<List<OrcamentoItem>>? _OrcamentoItemListList { get; set; } = new();
    protected List<OrcamentoItem>? _OrcamentoItemList { get; set; } = new();
    protected async Task GetOrcamentoItemList(List<OrcamentoView>? OrcamentoViewList)
    {
        _OrcamentoItemList = new();
        foreach (var item in OrcamentoViewList)
        {
            List<OrcamentoItem>? aux = (List<OrcamentoItem>?)await OrcamentoItemService.SelectByOrcamentoId(item.OrcamentoId);
            _OrcamentoItemList.AddRange(aux);
        }
    }

    public int getMaisBarato(List<OrcamentoView> orcamentoItemListList)
    {
        if (orcamentoItemListList == null || orcamentoItemListList.Count == 0)
        {
            return -1;
        }

        return orcamentoItemListList.OrderBy(o => o.PrecoTotalComEntrega).First().OrcamentoId;
    }

    public int getMaisQuantidadeItens(List<OrcamentoView> orcamentoItemListList)
    {
        if (orcamentoItemListList == null || orcamentoItemListList.Count == 0)
        {
            return -1;
        }

        return orcamentoItemListList.OrderBy(o => o.QuantidadeItens).Last().OrcamentoId;
    }

    public IEnumerator<T> GetSequentialObjects<T>(List<T> objects)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            yield return objects[i];
        }
    }



}