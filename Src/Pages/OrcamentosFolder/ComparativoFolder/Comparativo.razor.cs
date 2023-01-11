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
    private IEnumerator<OrcamentoView> _OrcamentoViewListEnumerator;
    private IEnumerator<List<OrcamentoItem>> _OrcamentoItemListEnumerator;

    List<List<string>> linhas = new();

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

        _ListaItemListEnumerator = _ListaItemList.GetEnumerator();
        _OrcamentoViewListEnumerator = _OrcamentoViewList.GetEnumerator();
        _OrcamentoItemListEnumerator = _OrcamentoItemList.GetEnumerator();

        
        bool temMais;
        string conteudo;
        for (int i = 0; i < quantidade_linhas; i++)
        {
            //COLUNA 1 - NUMERO DA LINHA DA TABELA COMPARATIVA DE ORÇAMENTOS
            List<string> linha = new();
            linha.Add((i+1).ToString());

            //ITEM SOLICITADO PELO CLIENTE

                //COLUNA 2 - CAMPO QUANTIDADE
                temMais = _ListaItemListEnumerator.MoveNext();
                conteudo = temMais ? ( _ListaItemListEnumerator.Current.Quantidade != null ? _ListaItemListEnumerator.Current.Quantidade.ToString() : "" ) : "";
                linha.Add(conteudo);
                
                //COLUNA 3 - CAMPO DESCRICAO
                conteudo = temMais ? ( _ListaItemListEnumerator.Current.Descricao != null ? _ListaItemListEnumerator.Current.Descricao.ToString() : "" ) : "";
                linha.Add(conteudo);
            
            //COLUNA 4 - COLUNA EM BRANCO SEPARADORA
            linha.Add("");

            //COLUNAS DE ITENS DO ORÇAMENTOS DAS LOJAS
                //para cada loja, insere as colunas correspondentes
            temMais = _OrcamentoViewListEnumerator.MoveNext();


            linha.Add("x");
            linha.Add("x");
            linha.Add("x");
            linha.Add("x");
            linha.Add("x");
            linha.Add("x");
            linha.Add("x");

            linhas.Add(linha);
            // linhas.Add(new List<string> { (i+1).ToString(), "Quantidade solicitada", "Produto", "", "Quantidade ofertada", "Valor unitário", "Valor total", "", "Quantidade ofertada", "Valor unitário", "Valor total" });
        }

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
        _OrcamentoItemList = new();
        foreach (var item in OrcamentoViewList)
        {
            List<OrcamentoItem>? aux = (List<OrcamentoItem>?) await OrcamentoItemService.SelectByOrcamentoId(item.OrcamentoId);
            _OrcamentoItemList.Add(aux);
        }
    }
    
}