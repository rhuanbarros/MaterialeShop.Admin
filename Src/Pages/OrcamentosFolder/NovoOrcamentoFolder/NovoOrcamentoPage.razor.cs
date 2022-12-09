using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.NovoOrcamentoFolder;

public partial class NovoOrcamentoPage
{    
    [Parameter]
    public int ListaId { get; set; }

    [Inject] 
    protected OrcamentoService OrcamentoService {get; set;}

    [Inject] 
    protected OrcamentoViewService OrcamentoViewService {get; set;}

    protected override async Task OnParametersSetAsync()
    {
        await GetTable();
        await GetOrcamentoViewByListaId();
    }

    // ---------------- SELECT TABLE ORCAMENTO
    protected override async Task GetTable()
    {
        _tableList = await OrcamentoService.SelectAllByListaId(ListaId);
        await InvokeAsync(StateHasChanged);
    }                         

    // -------------------START------------------- CAMPO Loja no MODEL  ----------------------------------------

    // ---------------- SELECT TABLE ORCAMENTOVIEW
    protected IReadOnlyList<OrcamentoView>? _OrcamentoViewList { get; set; }
    protected async Task GetOrcamentoViewByListaId()
    {
        _OrcamentoViewList = await OrcamentoViewService.SelectAllByListaId(ListaId);
        await InvokeAsync(StateHasChanged);
    }

    private OrcamentoView selectedLoja {get;set;}

    private Func<OrcamentoView, string> convertFuncPapel = ci => ci?.Nome;

    private async Task selectedValuesChangedLoja(IEnumerable<OrcamentoView> values)
    {
        OrcamentoView orcamentoSelected = values.First();

        model = _tableList.Where(f => f.Id == orcamentoSelected.OrcamentoId).First();
    }

    // -----------------END--------------------- CAMPO Loja no MODEL  ----------------------------------------

    protected override async Task OnClickSave()
    {
        form?.Validate();
        
        if(form.IsValid)
        {
            _processingNewItem = true;
            await CrudService.Edit<Orcamento>(model);            

            await GetTable();
            success = false;
            _processingNewItem = false;
        }
    }
    
}