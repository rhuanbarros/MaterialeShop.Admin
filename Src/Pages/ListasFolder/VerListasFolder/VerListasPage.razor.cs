using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.ListasFolder.VerListasFolder;

public partial class VerListasPage
{

    protected override async Task OnParametersSetAsync()
    {
        await GetTable();
        await GetTableUsuarioPerfil();
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

    // ---------------- CLICK NA LINHA DA TABELA
    private void RowClickEvent(TableRowClickEventArgs<ListasView> e)
    {
        NavigationManager.NavigateTo($"/lista/{e.Item.ListaId}");
    }

    // ---------------- DELETE
    protected override Lista SetModelIdToDelete(ListasView item)
    {
        return new Lista()
            {
                Id = item.ListaId
            };
    }

    // ---------------- EDIT MODEL
    protected override Lista SetModelIdToEdit(ListasView item)
    {
        return new Lista()
        {
            Id = item.ListaId,
            UsuarioPerfilId = item.UsuarioPerfilId,
            Endereco = item.Endereco,
            Status = item.Status            
        };
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