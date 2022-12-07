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
        Func<ListasView, bool> predicate = row => {
            if(
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
        NavigationManager.NavigateTo($"/lista/{e.Item.Id}");
    }

    // ---------------- CREATE NEW
    protected new Lista model = new();
    protected new async Task OnClickSave()
    {
        _processingNewItem = true;
        if(ModoEdicao == false)
        {
            await CrudService.Insert<Lista>(model);
        } else 
        {
            await CrudService.Edit<Lista>(model);            
            ModoEdicao = false;
        }

        model = new();
        await GetTable();
        success = false;
        _processingNewItem = false;
    }

    // ---------------- DELETE
    protected new async Task OnClickDelete(ListasView item)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Atenção",
            "Você deseja apagar este item?", 
            yesText:"Sim", cancelText:"Não");
        
        if(result == true)
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