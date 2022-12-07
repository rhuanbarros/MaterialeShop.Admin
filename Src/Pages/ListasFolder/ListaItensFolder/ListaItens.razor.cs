using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.ListasFolder.ListaItensFolder;

public partial class ListaItens
{
    [Parameter]
    public int ListaId { get; set; }

    [Inject] 
    protected ListasViewService ListasViewService {get; set;}

    [Inject] 
    protected ListaItensService ListaItensService {get; set;}

    protected override async Task OnParametersSetAsync()
    {
        await GetListaView();
        await GetTable();
    }

    // ---------------- SELECT TABLE
    protected override async Task GetTable()
    {
        _tableList = await ListaItensService.SelectAllByListaId(ListaId);
        _tableListFiltered = _tableList;
        await InvokeAsync(StateHasChanged);
    }


    // ---------------- GET ListaView
    private ListasView _ListaView {get; set;}
    private string NomeCliente = "Carregando";
    protected async Task GetListaView()
    {
        _ListaView = await ListasViewService.SelectAllByListaId(ListaId);
        NomeCliente = _ListaView?.NomeCompleto;
    }

    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        Func<ListaItem, bool> predicate = row =>
        {
            if (
                !string.IsNullOrEmpty(row.Descricao) && row.Descricao.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Quantidade) && row.Quantidade.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.UnidadeMedida) && row.UnidadeMedida.ToLower().Contains(text.ToLower())
            )
                return true;
            else
                return false;
        };
        _tableListFiltered = _tableList?.Where(predicate).ToList();
    }

    // ---------------- CREATE NEW
    protected override async Task OnClickSave()
    {
        form?.Validate();
        
        if(form.IsValid)
        {
            _processingNewItem = true;
            if(ModoEdicao == false)
            {
                model.ListaId = ListaId;
                await CrudService.Insert<ListaItem>(model);
            } else 
            {
                await CrudService.Edit<ListaItem>(model);
                ModoEdicao = false;
            }

            model = new();
            await GetTable();
            success = false;
            _processingNewItem = false;
        }
    }

}