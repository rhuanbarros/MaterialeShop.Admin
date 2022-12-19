using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.OrcamentoItensFolder;

public partial class OrcamentoItensPage
{    
    [Parameter]
    public int ListaId { get; set; }
    
    [Parameter]
    public int OrcamentoId { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await GetTable();
        await GetListaItensByListaId();
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
    
}