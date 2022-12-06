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

        foreach (var item in _UsuarioPerfilList)
        {
            Console.WriteLine("item");
            Console.WriteLine(item.NomeCompleto);
        }
    }

    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        _tableListFiltered = _tableList?.Where(row => row.Endereco.Contains(text)).ToList();
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

    //////////////////////// ---------------- CAMPO UsuarioPerfil no MODEL
    // ---------------- SELECT TABLE UsuarioPerfil
    protected IReadOnlyList<UsuarioPerfil>? _UsuarioPerfilList { get; set; }
    protected async Task GetTableUsuarioPerfil()
    {
        _UsuarioPerfilList = await CrudService.SelectFrom<UsuarioPerfil>();
        await InvokeAsync(StateHasChanged);
    }

    private Func<UsuarioPerfil, string> convertFuncPapel = ci => ci?.NomeCompleto;
    
}