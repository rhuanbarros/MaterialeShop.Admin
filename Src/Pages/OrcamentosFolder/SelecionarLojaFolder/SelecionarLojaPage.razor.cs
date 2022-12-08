using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.SelecionarLojaFolder;

public partial class SelecionarLojaPage
{
    [Parameter]
    public int ListaId { get; set; }

    [Inject]
    protected ListasViewService ListasViewService { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await GetTable();
        await GetListaView();
    }

    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        Func<ListasView, bool> predicate = row =>
        {
            if (
                !string.IsNullOrEmpty(row.NomeCompleto) && row.NomeCompleto.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Endereco) && row.Endereco.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Status) && row.Status.ToLower().Contains(text.ToLower())
            )
                return true;
            else
                return false;
        };
        _tableListFiltered = _tableList?.Where(predicate).ToList();
    }

    // ---------------- GET ListaView
    private ListasView _ListaView { get; set; }
    private string NomeCliente = "Carregando";
    protected async Task GetListaView()
    {
        _ListaView = await ListasViewService.SelectAllByListaId(ListaId);
        NomeCliente = _ListaView?.NomeCompleto;
    }

    // ---------------- CLICK NA LINHA DA TABELA
    private void RowClickEvent(TableRowClickEventArgs<ListasView> e)
    {
        NavigationManager.NavigateTo($"/lista/{e.Item.Id}");
    }

    // ---------------- CREATE NEW
    protected new Lista model = new();
    protected new async Task OnClickSave()
    {
        form?.Validate();

        if (form.IsValid)
        {
            _processingNewItem = true;
            if (ModoEdicao == false)
            {
                await CrudService.Insert<Lista>(model);
            }
            else
            {
                await CrudService.Edit<Lista>(model);
                ModoEdicao = false;
            }

            model = new();
            await GetTable();
            success = false;
            _processingNewItem = false;
        }
    }

    // ---------------- DELETE
    protected new async Task OnClickDelete(ListasView item)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Atenção",
            "Você deseja apagar este item?",
            yesText: "Sim", cancelText: "Não");

        if (result == true)
        {
            Lista newItem = new()
            {
                Id = item.Id
            };
            await CrudService.Delete<Lista>(newItem);
        }
        await GetTable();
    }

    // -------------------START------------------- CAMPO UsuarioPerfil no MODEL  ----------------------------------------

    // ---------------- SELECT TABLE UsuarioPerfil
    protected IReadOnlyList<UsuarioPerfil>? _UsuarioPerfilList { get; set; }
    protected async Task GetTableUsuarioPerfil()
    {
        _UsuarioPerfilList = await CrudService.SelectAllFrom<UsuarioPerfil>();
        await InvokeAsync(StateHasChanged);
    }

    private Func<UsuarioPerfil, string> convertFuncPapel = ci => ci?.NomeCompleto;
    // -----------------END--------------------- CAMPO UsuarioPerfil no MODEL  ----------------------------------------

}