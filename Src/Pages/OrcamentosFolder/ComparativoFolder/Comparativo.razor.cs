using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MaterialeShop.Admin.Src.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.ComparativoFolder;

public partial class Comparativo
{
    [Parameter]
    public int ListaId { get; set; }

    [Inject] 
    protected ListasViewService ListasViewService {get; set;}

    [Inject]
    ListaItensService ListaItensService { get; set; }

    [Inject]
    OrcamentoViewService OrcamentoViewService { get; set; }

    [Inject]
    OrcamentoItemService OrcamentoItemService { get; set; }
    
    [Inject]
    ListasService ListasService { get; set; }

    [Inject]
    CarrinhoService CarrinhoService { get; set; }

    [Inject]
    CarrinhoItemService CarrinhoItemService { get; set; }

    [Inject]
    OrcamentoItemViewService OrcamentoItemViewService { get; set; }

    public List<List<CelulaTabelaComparativa>> tabelaComparativa;
    public int? maisBarato;
    public int? maisQuantidadeItens;

    protected override async Task OnParametersSetAsync()
    {
        await GetListaView();
        await GetListaItem(ListaId);
        await GetOrcamentoView(ListaId);
        await GetOrcamentoItemList(_OrcamentoViewList);

        tabelaComparativa = CriarTabelaComparativa(_ListaItemList, _OrcamentoItemListList);
        // ImprimirResultado(tabelaComparativa);

        maisBarato = getMaisBarato(_OrcamentoViewList);
        maisQuantidadeItens = getMaisQuantidadeItens(_OrcamentoViewList);

        await InvokeAsync(StateHasChanged);
        
        // TODO quem sabe de para remover esta chamada ao banco de dados
        await GetLista(ListaId);
    }

    // ---------------- GET ListaView
    private ListasView _ListaView {get; set;}
    private string NomeCliente = "Carregando";
    private string Endereco = "Carregando";
    private string PrecoTotalCarrinhosMaisEconomico = "Carregando";
    protected async Task GetListaView()
    {
        _ListaView = await ListasViewService.SelectAllByListaId(ListaId);
        NomeCliente = _ListaView?.NomeCompleto;
        Endereco = _ListaView?.Endereco;
        PrecoTotalCarrinhosMaisEconomico = "R$" + String.Format("{0:0.00}", _ListaView?.EconomiaPrecoTotalComEntrega );
        
        Console.WriteLine("_ListaView?.EconomiaPrecoTotalComEntrega");
        Console.WriteLine(_ListaView?.EconomiaPrecoTotalComEntrega);
    }

    // ---------------- SELECT TABLE ListaItem
    protected List<ListaItem>? _ListaItemList { get; set; } = new();
    protected async Task GetListaItem(int ListaId)
    {
        _ListaItemList = (List<ListaItem>?)await ListaItensService.SelectAllByListaId(ListaId);
    }


    // ---------------- SELECT TABLE OrcamentoView
    protected List<OrcamentoView>? _OrcamentoViewList { get; set; } = new();
    protected async Task GetOrcamentoView(int ListaId)
    {
        _OrcamentoViewList = (List<OrcamentoView>?)await OrcamentoViewService.SelectAllByListaId(ListaId);
        // await InvokeAsync(StateHasChanged);
    }

    // ---------------- SELECT TABLE OrcamentoItem
    protected List<List<OrcamentoItem>>? _OrcamentoItemListList { get; set; } = new();
    protected async Task GetOrcamentoItemList(List<OrcamentoView>? OrcamentoViewList)
    {
        _OrcamentoItemListList = new();
        foreach (var item in OrcamentoViewList)
        {
            List<OrcamentoItem>? aux = (List<OrcamentoItem>?)await OrcamentoItemService.SelectByOrcamentoId(item.OrcamentoId);
            _OrcamentoItemListList.Add(aux);
        }
    }

    // ---------------- SELECT TABLE Lista
    protected Lista? _Lista { get; set; } = new();
    protected async Task GetLista(int ListaId)
    {
        _Lista = (Lista?)await ListasService.SelectByListaId(ListaId);
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

    // TODO: qdo o orcamento nao tiver nenhum item, ele nao aparece no comparativo
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

    protected async Task OnClickhandlerTeste()
    {
        Console.WriteLine("OnClickhandlerTeste()");
        await Task.Delay(3000);
        Console.WriteLine("FOI");

    }

    protected bool _processingNewItem = false;

    protected virtual async Task OnClickAdicionarAoCarrinhoTodoOrcamento(OrcamentoView item)
    {
        // TODO verificar se as variveis sao nulas antes de tentar fazer o seguinte.
        // quem sabe usar um try e capturar, em vez de usar 3 ifs

        // TODO arrumar o indicador de loading dos botoes
        _processingNewItem = true;

        Carrinho? carrinho = await CriaCarrinhoSeNaoExistir(item);

        // INSERE TODOS OS ITENS DO ORCAMENTO NO CARRINHO
        //pega todos os itens do orcamento especifico

        //transforma List<List<OrcamentoItem>> em List<OrcamentoItem>
        var orcamentoItemList = _OrcamentoItemListList?.SelectMany(x => x).ToList();
        // filtra todos os itens de orçamento pelo item.OrcamentoId
        List<OrcamentoItem>? orcamentoItemsListOrcamentoId = orcamentoItemList?.FindAll(x => x.OrcamentoId == item.OrcamentoId);

        //cria CarrinhoItem para cada item do orcamento
        List<CarrinhoItem>? CarrinhoItemList = orcamentoItemsListOrcamentoId?.Select(x => new CarrinhoItem(carrinho.Id, x.Id, x.Quantidade, null)).ToList();

        //insere todos os itens ao mesmo tempo no banco de dados
        await CarrinhoItemService.Upsert(CarrinhoItemList);

        NavigationManager.NavigateTo(Rotas.CarrinhosItensLista(ListaId));

        _processingNewItem = false;
    }

    private async Task<Carrinho?> CriaCarrinhoSeNaoExistir(OrcamentoView item)
    {
        //verificar se carrinho para este orcamento ja existe
        IReadOnlyList<Carrinho> carrinhos = await CarrinhoService.FindCarrinho(ListaId, _Lista.PerfilId, item.OrcamentoId);
        Carrinho? carrinho = carrinhos?.FirstOrDefault();

        bool carrinhoNaoExiste = carrinho is null ? true : false;

        if (carrinhoNaoExiste)
        {
            //criar carrinho
            carrinho = new()
            {
                PerfilId = _Lista.PerfilId,
                ListaId = ListaId,
                OrcamentoId = item.OrcamentoId,
                Status = Carrinho.StatusConstCarrinho.EmCriacao
            };
            List<Carrinho> carrinhos1 = await CarrinhoService.Insert(carrinho);
            carrinho = carrinhos1.First();
        }

        return carrinho;
    }

    private async Task AddToCarrinhoAsync(OrcamentoItem item)
    {
        //verificar se carrinho para este orcamento ja existe
        IReadOnlyList<Carrinho> carrinhos = await CarrinhoService.FindCarrinho(ListaId, _Lista.PerfilId, item.OrcamentoId);
        carrinhos?.FirstOrDefault();
        
    }

    // ---------------- SELECT TABLE ListaItem
    protected List<OrcamentoItemView>? _OrcamentoItemViewList { get; set; } = new();
    protected async Task GetOrcamentoItemView(int ListaId)
    {
        _OrcamentoItemViewList = (List<OrcamentoItemView>?)await OrcamentoItemViewService.SelectByListaId(ListaId);

        
    }


}