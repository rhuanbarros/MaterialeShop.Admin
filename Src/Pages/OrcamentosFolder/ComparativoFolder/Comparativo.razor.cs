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

    protected override async Task OnParametersSetAsync()
    {
        await GetListaItem(ListaId);
        await GetOrcamentoView(ListaId);
        await GetOrcamentoItemList(_OrcamentoViewList);
        await InvokeAsync(StateHasChanged);
    }
    
    // ---------------- SELECT TABLE ListaItem
    protected IReadOnlyList<ListaItem>? _ListaItemList { get; set; }
    protected async Task GetListaItem(int ListaId)
    {
        _ListaItemList = await ListaItensService.SelectAllByListaId(ListaId);
        // await InvokeAsync(StateHasChanged);
    }


    // ---------------- SELECT TABLE OrcamentoView
    protected List<OrcamentoView>? _OrcamentoViewList { get; set; }
    protected async Task GetOrcamentoView(int ListaId)
    {
        _OrcamentoViewList = (List<OrcamentoView>?) await OrcamentoViewService.SelectAllByListaId(ListaId);
        // await InvokeAsync(StateHasChanged);
    }

    // ---------------- SELECT TABLE OrcamentoItem
    protected List<List<OrcamentoItem>>? _OrcamentoItemList { get; set; } = new();
    protected async Task GetOrcamentoItemList(List<OrcamentoView>? OrcamentoViewList)
    {
        foreach (var item in OrcamentoViewList)
        {
            List<OrcamentoItem>? aux = (List<OrcamentoItem>?) await OrcamentoItemService.SelectByOrcamentoId(item.OrcamentoId);
            _OrcamentoItemList.Add(aux);
        }
    }
    
}