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

    public List<List<CelulaTabelaComparativa>> tabelaComparativa;
    public int? maisBarato;
    public int? maisQuantidadeItens;

    protected override async Task OnParametersSetAsync()
    {
        await GetListaItem(ListaId);
        await GetOrcamentoView(ListaId);
        await GetOrcamentoItemList(_OrcamentoViewList);

        tabelaComparativa = CriarTabelaComparativa(_ListaItemList, _OrcamentoItemList);
        // ImprimirResultado(tabelaComparativa);

        maisBarato = getMaisBarato(_OrcamentoViewList);
        maisQuantidadeItens = getMaisQuantidadeItens(_OrcamentoViewList);

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
    protected List<List<OrcamentoItem>>? _OrcamentoItemList { get; set; } = new();
    protected async Task GetOrcamentoItemList(List<OrcamentoView>? OrcamentoViewList)
    {
        _OrcamentoItemList = new();
        foreach (var item in OrcamentoViewList)
        {
            List<OrcamentoItem>? aux = (List<OrcamentoItem>?)await OrcamentoItemService.SelectByOrcamentoId(item.OrcamentoId);
            _OrcamentoItemList.Add(aux);
        }
    }

    public enum TipoColuna
    {
        ListaItem,
        ListaItemVazio,
        OrcamentoItem,
        OrcamentoItemVazio
    }

    public class CelulaTabelaComparativa
    {
        public CelulaTabelaComparativa(TipoColuna tipoColuna, BaseModelApp conteudo)
        {
            TipoColuna = tipoColuna;
            Conteudo = conteudo;
        }

        public TipoColuna TipoColuna { get; set; }
        public BaseModelApp Conteudo { get; set; }
    }

    List<List<CelulaTabelaComparativa>> CriarTabelaComparativa(List<ListaItem> listaItemList, List<List<OrcamentoItem>> orcamentoItemListList)
    {
        //TODO: verificar se, on caso de uma loja oferecer 2 itens em relação a 1 item solicitado, ele repete o item solicitado na coluna ListaItem

        var result = new List<List<CelulaTabelaComparativa>>();
        var distinctBudgetIds = orcamentoItemListList.SelectMany(x => x.Select(y => y.OrcamentoId)).Distinct();

        //iterating over each item of the list
        foreach (var item in listaItemList)
        {
            var itemOrcamento = new List<CelulaTabelaComparativa> {
                new CelulaTabelaComparativa(TipoColuna.ListaItem, item)
            };
            foreach (var budgetId in distinctBudgetIds)
            {
                var budget = orcamentoItemListList.SelectMany(x => x.Where(y => y.ListaItemId == item.Id && y.OrcamentoId == budgetId)).FirstOrDefault();
                if(budget != null)
                    itemOrcamento.Add( new CelulaTabelaComparativa(TipoColuna.OrcamentoItem, budget) );
                else
                    itemOrcamento.Add( new CelulaTabelaComparativa(TipoColuna.OrcamentoItemVazio, null) );
            }
            result.Add(itemOrcamento);
        }

        //iterating over each budget
        foreach (var budgetId in distinctBudgetIds)
        {
            var budgetItens = orcamentoItemListList.SelectMany(x => x.Where(y => !listaItemList.Any(z => z.Id == y.ListaItemId) && y.OrcamentoId == budgetId)).ToList();
            foreach (var budget in budgetItens)
            {
                var itemOrcamento = new List<CelulaTabelaComparativa> {
                    new CelulaTabelaComparativa(TipoColuna.ListaItemVazio, null)
                };
                foreach (var id in distinctBudgetIds)
                {
                    if (id == budget.OrcamentoId)
                    {
                        itemOrcamento.Add( new CelulaTabelaComparativa(TipoColuna.OrcamentoItem, budget) );
                    }
                    else
                    {
                        itemOrcamento.Add( new CelulaTabelaComparativa(TipoColuna.OrcamentoItemVazio, null) );
                    }
                }
                result.Add(itemOrcamento);
            }
        }
        return result;
    }

    static void ImprimirResultado(List<List<string>> resultado)
    {
        // Find the max lenght of string in each column
        var columnWidths = Enumerable.Range(0, resultado.First().Count)
            .Select(i => resultado.Max(e => e[i].Length))
            .ToArray();
        // printing the result
        foreach (var row in resultado)
        {
            for (int i = 0; i < row.Count; i++)
            {
                Console.Write(" | ");
                Console.Write(row[i].PadRight(columnWidths[i] + 1));
            }
            Console.WriteLine();
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


}