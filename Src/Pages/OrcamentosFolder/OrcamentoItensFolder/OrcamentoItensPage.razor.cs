using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.OrcamentoItensFolder;

public partial class OrcamentoItensPage
{
    //TODO: na lista de itens do orçamento, fazer aparecer a descrição do nome do item solicitado pelo cliente. acho q tem q criar uma view

    [Parameter]
    public int ListaId { get; set; }
    
    [Parameter]
    public int OrcamentoId { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await GetTable();
        await GetListaItensByListaId();
        await GetOrcamentoView();
    }

    // ---------------- SELECT TABLE
    [Inject] 
    protected OrcamentoItemService OrcamentoItemService {get; set;}

    protected IReadOnlyList<OrcamentoItem>? _tableList { get; set; }
    protected IReadOnlyList<OrcamentoItem>? _tableListFiltered { get; set; }
    protected MudTable<OrcamentoItem>? table;
    
    protected override async Task GetTable()
    {
        _tableList = await OrcamentoItemService.SelectByOrcamentoId(OrcamentoId);
        _tableListFiltered = _tableList;
        await InvokeAsync(StateHasChanged);
    }

    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        Func<OrcamentoItem, bool> predicate = row =>
        {
            if (
                !string.IsNullOrEmpty(row.Descricao) && row.Descricao.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Observacao) && row.Observacao.ToLower().Contains(text.ToLower())
            )
                return true;
            else
                return false;
        };
        _tableListFiltered = _tableList?.Where(predicate).ToList();
    }

    // ---------------- CREATE NEW
    protected override OrcamentoItem SetModelReferenceId(OrcamentoItem item)
    {
        Console.WriteLine("----------------------------------------- SetModelReferenceId");

        item.OrcamentoId = OrcamentoId;
        return item;
    }

    // protected override OrcamentoItem CreateNewModel()
    // {
    //     model = new()
    //     {
    //         ListaItemId = null,
    //         CreatedAt = DateTime.Now
    //     };
    //     return model;
    // }

    // -------------------START------------------- CAMPO ListaItemId no MODEL  ----------------------------------------

    [Inject] 
    protected ListaItensService ListaItensService {get; set;}

    // ---------------- SELECT TABLE Loja
    protected IReadOnlyList<ListaItem>? _ListaItensList { get; set; }
    protected async Task GetListaItensByListaId()
    {
        _ListaItensList = await ListaItensService.SelectAllByListaId(ListaId);
        await InvokeAsync(StateHasChanged);
    }

    private Func<ListaItem, string> convertFuncPapel = ci => ci?.Descricao;

    // -----------------END--------------------- CAMPO ListaItemId no MODEL  ----------------------------------------

    // ---------------- GET OrcamentoView
    [Inject] 
    protected OrcamentoViewService OrcamentoViewService {get; set;}

    private OrcamentoView _OrcamentoView {get; set;}
    private string NomeLoja = "Carregando";
    protected async Task GetOrcamentoView()
    {
        _OrcamentoView = await OrcamentoViewService.SelectByOrcamentoId(OrcamentoId);
        NomeLoja = _OrcamentoView?.LojaNome;
    }
    
}