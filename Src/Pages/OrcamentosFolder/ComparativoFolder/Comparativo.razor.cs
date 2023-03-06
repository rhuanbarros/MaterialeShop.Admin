using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MaterialeShop.Admin.Src.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.ComparativoFolder;

public partial class Comparativo
{
    [Parameter]
    public int ListaId { get; set; }

    [Inject]
    protected ListasViewService ListasViewService { get; set; }

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
        await GetOrcamentoItemViewList(_OrcamentoViewList);

        tabelaComparativa = CriarTabelaComparativa(_ListaItemList, _OrcamentoItemViewListList);
        // ImprimirResultado(tabelaComparativa);

        maisBarato = getMaisBarato(_OrcamentoViewList);
        maisQuantidadeItens = getMaisQuantidadeItens(_OrcamentoViewList);

        await InvokeAsync(StateHasChanged);

        // TODO quem sabe de para remover esta chamada ao banco de dados
        await GetLista(ListaId);
        await CalculaCarrinhoMaisEconomico(ListaId);
    }

    // ---------------- GET ListaView
    private ListasView _ListaView { get; set; }
    private string NomeCliente = "Carregando";
    private string Endereco = "Carregando";
    
    protected async Task GetListaView()
    {
        _ListaView = await ListasViewService.SelectAllByListaId(ListaId);
        NomeCliente = _ListaView?.NomeCompleto;
        Endereco = _ListaView?.Endereco;
        // PrecoTotalCarrinhosMaisEconomico = "R$" + String.Format("{0:0.00}", _ListaView?.EconomiaPrecoTotalComEntrega);

        // Console.WriteLine("_ListaView?.EconomiaPrecoTotalComEntrega");
        // Console.WriteLine(_ListaView?.EconomiaPrecoTotalComEntrega);
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
    
    // ---------------- SELECT TABLE OrcamentoItemView
    protected List<List<OrcamentoItemView>>? _OrcamentoItemViewListList { get; set; } = new();
    protected async Task GetOrcamentoItemViewList(List<OrcamentoView>? OrcamentoViewList)
    {
        _OrcamentoItemViewListList = new();
        foreach (var item in OrcamentoViewList)
        {
            List<OrcamentoItemView>? aux = (List<OrcamentoItemView>?)await OrcamentoItemViewService.SelectByOrcamentoId(item.OrcamentoId);
            _OrcamentoItemViewListList.Add(aux);
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
    List<List<CelulaTabelaComparativa>> CriarTabelaComparativa(List<ListaItem> listaItemList, List<List<OrcamentoItemView>> orcamentoItemListList)
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
            
            // se não houver orçamento correspondente, adiciona o item na primeira coluna e colunas vazias para os orçamentos
            if (budgets.Count == 0)
            {
                var itemOrcamento = new List<CelulaTabelaComparativa>();

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

    protected virtual async Task OnClickAdicionarAoCarrinhoTodoOrcamento(OrcamentoView item)
    {
        // TODO verificar se as variveis sao nulas antes de tentar fazer o seguinte.
        // quem sabe usar um try e capturar, em vez de usar 3 ifs

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

    private async Task AddToCarrinhoAsync(OrcamentoItemView item)
    {
        //verificar se carrinho para este orcamento ja existe
        IReadOnlyList<Carrinho> carrinhos = await CarrinhoService.FindCarrinho(ListaId, _Lista.PerfilId, item.OrcamentoId);
        carrinhos?.FirstOrDefault();

    }

    // ---------------- SELECT TABLE ListaItem
    private string PrecoTotalCarrinhosMaisEconomico = "Carregando";
    private string EconomiaEmRelacaoAoOrcamentoMaisCaro = "Carregando";
    private string EconomiaEmRelacaoAoOrcamentoMaisCaroPorcentagem = "Carregando";
    protected List<OrcamentoItemView>? _OrcamentoItemViewList { get; set; } = new();
    List<OrcamentoItemView> itensEconomia;
    decimal? diferencaPrecoOrcamentoMaisCaro = 0;
    protected async Task CalculaCarrinhoMaisEconomico(int ListaId)
    {
        _OrcamentoItemViewList = (List<OrcamentoItemView>?)await OrcamentoItemViewService.SelectByListaId(ListaId);

        List<OrcamentoItemView> ListOrcamentoItemViewMaisBaratoParaCadaListaItem = GetListOrcamentoItemViewMaisBaratoParaCadaListaItem(_OrcamentoItemViewList);

        Console.WriteLine("ListOrcamentoItemViewMaisBaratoParaCadaListaItem");
        PrintList(ListOrcamentoItemViewMaisBaratoParaCadaListaItem);
        Console.WriteLine("");
        Console.WriteLine("");

        itensEconomia = RemoveItensDuplicadosEMantemAPenasUm(ListOrcamentoItemViewMaisBaratoParaCadaListaItem);

        Console.WriteLine("itensEconomia");        
        
        PrintList(itensEconomia);
        Console.WriteLine("");
        Console.WriteLine("");
        
        decimal? economiaTotal = itensEconomia.Sum( x => x.Preco * (x.OrcamentoItem_Quantidade ?? x.ListaItem_Quantidade ?? 1) );
        Console.WriteLine("economiaTotal");
        Console.WriteLine(economiaTotal);

        // ---------
        // soma o total de taxa de entrega se houver itens de mais de uma loja diferente
        List<int> listOrcamentoId = itensEconomia.Select( x => x.OrcamentoId).Distinct().ToList();
        
        decimal totalOrcamentoEconomia = 0;
        foreach (var item in listOrcamentoId)
        {
            OrcamentoView? orcamentoView = _OrcamentoViewList?.Find( x=> x.OrcamentoId == item);
            totalOrcamentoEconomia += orcamentoView?.EntregaPreco ?? 0;
        }        
        Console.WriteLine("totalOrcamentoEconomia");
        Console.WriteLine(totalOrcamentoEconomia);

        decimal? economiaTotalComEntrega = economiaTotal + totalOrcamentoEconomia;

        PrecoTotalCarrinhosMaisEconomico = "R$" + String.Format("{0:0.00}", economiaTotalComEntrega);

        decimal? PrecoOrcamentoMaisCaro = _OrcamentoViewList.Max( x => x.PrecoTotalComEntrega);
        diferencaPrecoOrcamentoMaisCaro = PrecoOrcamentoMaisCaro - economiaTotalComEntrega;
        
        EconomiaEmRelacaoAoOrcamentoMaisCaro = "R$" + String.Format("{0:0.00}", diferencaPrecoOrcamentoMaisCaro);

        decimal? porcentagemDiferencaPrecoOrcamentoMaisCaro;
        if(PrecoOrcamentoMaisCaro == 0)
            porcentagemDiferencaPrecoOrcamentoMaisCaro = 0;
        else
            porcentagemDiferencaPrecoOrcamentoMaisCaro = diferencaPrecoOrcamentoMaisCaro / PrecoOrcamentoMaisCaro * 100;
            
        EconomiaEmRelacaoAoOrcamentoMaisCaroPorcentagem = String.Format("{0:0.00}", porcentagemDiferencaPrecoOrcamentoMaisCaro )+"%";
    }

    public List<OrcamentoItemView> GetListOrcamentoItemViewMaisBaratoParaCadaListaItem(List<OrcamentoItemView> _OrcamentoItemViewList)
    {
        // Agrupa os itens por ListaItemId
        var grouped = _OrcamentoItemViewList.GroupBy(x => x.ListaItemId);

        // Cria uma lista para armazenar os itens selecionados
        List<OrcamentoItemView> result = new List<OrcamentoItemView>();

        // Para cada grupo de itens com o mesmo ListaItemId
        foreach (var group in grouped)
        {
            // Seleciona o item com o menor valor de Preco
            var minPreco = group.Min(x => x.Preco);

            // Adiciona todos os itens com o menor valor de Preco à lista de resultados
            result.AddRange(group.Where(x => x.Preco == minPreco));
        }

        // Retorna a lista de resultados
        return result;
    }

    public List<OrcamentoItemView> RemoveItensDuplicadosEMantemAPenasUm(List<OrcamentoItemView> _OrcamentoItemViewList)
    {
        // Usaremos um dicionário para armazenar as ocorrências de ListaItemId
        Dictionary<int?, int> ocorrencias = new Dictionary<int?, int>();

        // Loop para contar o número de ocorrências de cada ListaItemId
        foreach (var item in _OrcamentoItemViewList)
        {
            if (item.ListaItemId != null)
            {
                if (ocorrencias.ContainsKey(item.ListaItemId))
                {
                    ocorrencias[item.ListaItemId]++;
                }
                else
                {
                    ocorrencias[item.ListaItemId] = 1;
                }
            }
        }

        // Criamos uma lista para armazenar os itens a serem removidos
        List<OrcamentoItemView> itensRemovidos = new List<OrcamentoItemView>();

        // Loop para verificar se há itens com ListaItemId duplicado
        foreach (var item in _OrcamentoItemViewList)
        {
            if (item.ListaItemId != null && ocorrencias[item.ListaItemId] > 1)
            {
                // TODO isso daqui possivelmente vai dar problema no futuro, pq ele pode remover um item que nao era pra remover
                //      fiz um teste diferente e funcionou, vai ver ta certo mesmo.

                // Se houver, contamos quantos itens contém o mesmo OrcamentoId
                int quantidade = _OrcamentoItemViewList.Count(i => i.OrcamentoId == item.OrcamentoId);

                // Se não houver mais de 1 item com o mesmo OrcamentoId, adicionamos o item à lista de itens a serem removidos
                if (quantidade < 2)
                {
                    itensRemovidos.Add(item);
                }
            }
        }

        // Removemos os itens a serem removidos da lista original
        foreach (var item in itensRemovidos)
        {
            _OrcamentoItemViewList.Remove(item);
        }

        // Retornamos a lista atualizada
        return _OrcamentoItemViewList;
    }



    public void PrintList(List<OrcamentoItemView> orcamentoItemViews)
    {
        orcamentoItemViews = orcamentoItemViews.OrderBy(x => x.OrcamentoId).ToList();

        int maxOrcamentoIdLength = "OrcamentoId".Length;
        int maxPrecoLength = "Preco".Length;
        int maxListaIdLength = "ListaId".Length;
        int maxListaItemIdLength = "ListaItemId".Length;
        int maxListaItemDescricaoLength = "ListaItem_Descricao".Length;

        foreach (var item in orcamentoItemViews)
        {
            if (item.OrcamentoId.ToString().Length > maxOrcamentoIdLength)
                maxOrcamentoIdLength = item.OrcamentoId.ToString().Length;

            if (item.Preco.ToString().Length > maxPrecoLength)
                maxPrecoLength = item.Preco.ToString().Length;

            if (item.ListaId.ToString().Length > maxListaIdLength)
                maxListaIdLength = item.ListaId.ToString().Length;

            if (item.ListaItemId.ToString().Length > maxListaItemIdLength)
                maxListaItemIdLength = item.ListaItemId.ToString().Length;

            if (item.ListaItem_Descricao.Length > maxListaItemDescricaoLength)
                maxListaItemDescricaoLength = item.ListaItem_Descricao.Length;
        }

        Console.WriteLine("OrcamentoId".PadRight(maxOrcamentoIdLength) + " " +
                        "Preco".PadRight(maxPrecoLength) + " " +
                        "ListaId".PadRight(maxListaIdLength) + " " +
                        "ListaItemId".PadRight(maxListaItemIdLength) + " " +
                        "ListaItem_Descricao");

        Console.WriteLine(new string('-', maxOrcamentoIdLength) + " " +
                        new string('-', maxPrecoLength) + " " +
                        new string('-', maxListaIdLength) + " " +
                        new string('-', maxListaItemIdLength) + " " +
                        new string('-', maxListaItemDescricaoLength));

        foreach (var item in orcamentoItemViews)
        {
            Console.WriteLine(item.OrcamentoId.ToString().PadRight(maxOrcamentoIdLength) + " " +
                            item.Preco.ToString().PadRight(maxPrecoLength) + " " +
                            item.ListaId.ToString().PadRight(maxListaIdLength) + " " +
                            item.ListaItemId.ToString().PadRight(maxListaItemIdLength) + " " +
                            item.ListaItem_Descricao);
        }
    }


    private async Task OnClickMontarCarrinhoEconomico()
    {
        // limpa todos os carrinhos ja existentes pra esta lista
        await CarrinhoService.SetStatus( Carrinho.StatusConstCarrinho.Cancelado, ListaId );

        List<int> listOrcamentoId = itensEconomia.Select( x => x.OrcamentoId ).Distinct().ToList();

        foreach (var orcamentoId in listOrcamentoId)
        {
            // cria carrinho novo para cada orcamento existente
            Carrinho carrinho = new()
            {
                PerfilId = _Lista.PerfilId,
                ListaId = ListaId,
                OrcamentoId = orcamentoId,
                Status = Carrinho.StatusConstCarrinho.EmCriacao
            };
            List<Carrinho> carrinhos1 = await CarrinhoService.Insert(carrinho);
            carrinho = carrinhos1.First();

            // INSERE TODOS OS ITENS DO ORCAMENTO NO CARRINHO
            // filtra todos os itens de orçamento pelo item.OrcamentoId
            List<OrcamentoItemView>? orcamentoItemViews = itensEconomia?.FindAll(x => x.OrcamentoId == orcamentoId);

            //cria CarrinhoItem para cada item do orcamento
            List<CarrinhoItem>? CarrinhoItemList = orcamentoItemViews?.Select(x => new CarrinhoItem(carrinho.Id, x.Id, (x.OrcamentoItem_Quantidade ?? x.ListaItem_Quantidade ?? 1), null)).ToList();

            //insere todos os itens ao mesmo tempo no banco de dados
            await CarrinhoItemService.Upsert(CarrinhoItemList);            
        }

        NavigationManager.NavigateTo(Rotas.CarrinhosItensLista(ListaId));
    }

    // download orcamento
    private const string BucketName = "orcamentos";

    // public Supabase.Storage.FileObject? fileObject;
    // private async Task<String> GetFileToDownload(OrcamentoView orcamentoView)
    // {
    //     fileObject = await StorageService.GetLastFileFromBucket(BucketName, ListaId.ToString() +"/"+ orcamentoView.LojaId.ToString());
        
    //     System.Console.WriteLine("fileObject.Name");
    //     System.Console.WriteLine(fileObject.Name);
    // }


    private async Task DownloadClick(OrcamentoView orcamentoView)
    {
        byte[] downloadedBytes = await StorageService.DownloadFile(BucketName, ListaId.ToString() +"/"+ orcamentoView.LojaId, orcamentoView?.OrcamentoAnexo);

        await JS.InvokeVoidAsync("downloadFileFromStream", orcamentoView?.OrcamentoAnexo, downloadedBytes);
    }


}