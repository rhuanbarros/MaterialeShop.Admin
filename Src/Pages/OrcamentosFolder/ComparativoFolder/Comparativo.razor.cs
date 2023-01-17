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
        //cria uma lista vazia para armazenar os resultados
        var result = new List<List<CelulaTabelaComparativa>>();
        //obtém os ids de orçamentos distintos presentes na lista de orçamentos
        var distinctBudgetIds = orcamentoItemListList.SelectMany(x => x.Select(y => y.OrcamentoId)).Distinct();
        // obtém todos os itens de orçamento
        var budgetItems = orcamentoItemListList.SelectMany(x => x).ToList();

        //itera sobre cada item da lista
        foreach (var item in listaItemList)
        {
            // obtém os orçamentos correspondentes para o item atual
            var budgets = budgetItems.Where(x => x.ListaItemId == item.Id).ToList();
            if (budgets.Count == 0)
            {
                var itemOrcamento = new List<CelulaTabelaComparativa>();

                // se não houver orçamento correspondente, adiciona o item na primeira coluna e colunas vazias para os orçamentos
                itemOrcamento.Add(new CelulaTabelaComparativa(TipoColuna.ListaItem, item));
                foreach (var budgetId in distinctBudgetIds)
                {
                    itemOrcamento.Add(new CelulaTabelaComparativa(TipoColuna.OrcamentoItemVazio, null));
                }
                result.Add(itemOrcamento);
            }
            else
            {
                // obtém a quantidade máxima de itens de orçamento para um item de orçamento
                // var maxBudgetCount = budgets.GroupBy(x => x.ListaItemId).Select(x => x.Count()).Max();

                //quantidade máxima de itens de orçamento em relação a todos os orçamentos em relação ao ListaItemId = item.id
                var maxBudgetCount = budgets.Where(x => x.ListaItemId == item.Id).GroupBy(x => x.OrcamentoId).Select(x => x.Count()).Max();

                //itera pela quantidade máxima de itens de orçamento
                for (int i = 0; i < maxBudgetCount; i++)
                {
                    var itemOrcamento = new List<CelulaTabelaComparativa>();
                    
                    // adiciona o item na primeira coluna
                    itemOrcamento.Add(new CelulaTabelaComparativa(TipoColuna.ListaItem, item));

                    //itera sobre cada id de orçamento distinto
                    foreach (var budgetId in distinctBudgetIds)
                    {
                        //Obtém o orçamento correspondente para o id de orçamento atual.
                        var budget = budgets.FirstOrDefault(x => x.OrcamentoId == budgetId);
                        
                        //  Verifica se existe e se ainda há itens de orçamento para serem adicionados.
                        // Se existir orçamento correspondente e ainda há itens de orçamento para serem adicionados, adiciona o orçamento na coluna correspondente e remove-o da lista de orçamentos
                        // if (budget != null && i < budgets.Count(x => x.OrcamentoId == budgetId))
                        if (budget != null)
                        {
                            itemOrcamento.Add(new CelulaTabelaComparativa(TipoColuna.OrcamentoItem, budget));
                            budgets.Remove(budget);
                        }
                        else
                        {
                            // Se não houver orçamento correspondente ou já não há mais itens de orçamento para serem adicionados, adiciona uma célula vazia na coluna correspondente
                            itemOrcamento.Add(new CelulaTabelaComparativa(TipoColuna.OrcamentoItemVazio, null));
                        }
                    }
                    result.Add(itemOrcamento);
                }
            }
            
        }


        // é iterado sobre cada ID de orçamento presente no conjunto de IDs distintos novamente, adicionando os itens de orçamento que não estão presentes na lista de itens.
        foreach (var budgetId in distinctBudgetIds)
        {
            var budgetItens = orcamentoItemListList.SelectMany(x => x.Where(y => !listaItemList.Any(z => z.Id == y.ListaItemId) && y.OrcamentoId == budgetId)).ToList();
            foreach (var budget in budgetItens)
            {
                // Para cada item de orçamento, é adicionado uma célula vazia na primeira coluna (TipoColuna.ListaItemVazio)
                var itemOrcamento = new List<CelulaTabelaComparativa> {
                    new CelulaTabelaComparativa(TipoColuna.ListaItemVazio, null)
                };
                foreach (var id in distinctBudgetIds)
                {
                    // se há um item de orçamento no orçamento em questão, ele adiciona o item a coluna correspondente
                    if (id == budget.OrcamentoId)
                    {
                        itemOrcamento.Add(new CelulaTabelaComparativa(TipoColuna.OrcamentoItem, budget));
                    }
                    else
                    {
                        // se não houver, ele adiciona uma coluna vazia
                        itemOrcamento.Add(new CelulaTabelaComparativa(TipoColuna.OrcamentoItemVazio, null));
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