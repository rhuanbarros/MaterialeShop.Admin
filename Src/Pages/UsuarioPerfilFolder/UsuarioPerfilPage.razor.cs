using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.UsuarioPerfilFolder;

public partial class UsuarioPerfilPage
{
    //TODO: usar OnParametersSetAsync ?    
    protected override async Task OnInitializedAsync()
    {
        await GetTable();
    }

    // ---------------- SELECT TABLE
    private IReadOnlyList<UsuarioPerfil>? _tableList { get; set; }
    private IReadOnlyList<UsuarioPerfil>? _tableListFiltered { get; set; }
    private MudTable<UsuarioPerfil>? table;
    protected async Task GetTable()
    {
        _tableList = await UsuarioPerfilService.From();
        _tableListFiltered = _tableList;
        await InvokeAsync(StateHasChanged);
    }

}