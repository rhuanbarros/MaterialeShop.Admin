using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.UsuarioPerfilFolder;

public partial class UsuarioPerfilPage
{
    private string Titulo = "Minhas listas de compras";

    //TODO: usar OnParametersSetAsync ?    
    protected override async Task OnInitializedAsync()
    {
        await GetTable();

        IReadOnlyList<UsuarioPerfil> perfil = await UsuarioPerfilService.GetByUserId("a7251a08-9573-4192-8d14-d4de1ab0ee6e");
        
        Console.WriteLine("perfil.Single()");
        Console.WriteLine(perfil.Single().Email);
        

        // foreach (var item in perfil)
        // {
        //     Console.WriteLine("item");
        //     Console.WriteLine(item.Email);
        //     Console.WriteLine(item.NomeCompleto);
        // }
    }

    // ---------------- SELECT TABLE
    private IReadOnlyList<UsuarioPerfil>? _listaList { get; set; }
    private IReadOnlyList<UsuarioPerfil>? _listaListFiltered { get; set; }
    private MudTable<UsuarioPerfil>? table;
    protected async Task GetTable()
    {
        // await Task.Delay(10000);
        IReadOnlyList<UsuarioPerfil> listas = await UsuarioPerfilService.From();
        _listaList = listas;
        _listaListFiltered = listas;
        await InvokeAsync(StateHasChanged);
    }

}