using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MaterialeShop.Admin.Src.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.OrcamentosListaFolder;

public partial class OrcamentosComponent
{
    [Parameter]
    public int ListaId { get; set; }

    [Inject] 
    protected OrcamentoViewService OrcamentoViewService {get; set;}

    protected override async Task OnParametersSetAsync()
    {
        await GetTable();
        await GetTableLoja();
    }

    protected override async Task GetTable()
    {
        _tableList = await OrcamentoViewService.SelectAllByListaId(ListaId);
        _tableListFiltered = _tableList;
        await InvokeAsync(StateHasChanged);
    }

    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        Func<OrcamentoView, bool> predicate = row =>
        {
            if (
                !string.IsNullOrEmpty(row.LojaNome) && row.LojaNome.ToLower().Contains(text.ToLower())
            )
                return true;
            else
                return false;
        };
        _tableListFiltered = _tableList?.Where(predicate).ToList();
    }

    // ---------------- DELETE
    protected override Orcamento SetModelIdToDelete(OrcamentoView item)
    {
        return new Orcamento()
            {
                Id = item.OrcamentoId
            };
    }

    protected override Orcamento SetModelReferenceId(Orcamento item)
    {
        item.ListaId = ListaId;
        return item;
    }

    // ---------------- EDIT MODEL
    protected override Orcamento SetModelIdToEdit(OrcamentoView item)
    {
        return new Orcamento()
        {
            Id = item.OrcamentoId,
            LojaId = item.LojaId,
            ListaId = ListaId,
            SolicitacaoData = item.SolicitacaoData,
            SolicitacaoHora = item.SolicitacaoHora
        };
    }

    // -------------------START------------------- CAMPO LojaId no MODEL  ----------------------------------------

    // ---------------- SELECT TABLE Loja
    protected IReadOnlyList<Loja>? _LojaList { get; set; }
    protected async Task GetTableLoja()
    {
        _LojaList = await CrudService.SelectAllFrom<Loja>();
        await InvokeAsync(StateHasChanged);
    }

    private Func<Loja, string> convertFuncPapel = ci => ci?.Nome;
    // -----------------END--------------------- CAMPO LojaId no MODEL  ----------------------------------------

    // ---------------- CLICK NA LINHA DA TABELA
    private void RowClickEvent(TableRowClickEventArgs<OrcamentoView> e)
    {
        NavigationManager.NavigateTo(Rotas.Orcamentos_itens(ListaId, e.Item.OrcamentoId));
    }

}