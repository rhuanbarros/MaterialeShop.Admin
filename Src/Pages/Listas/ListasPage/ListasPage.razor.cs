using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.Listas.ListasPage;

public partial class ListasPage
{
    private string Titulo = "Minhas listas de compras";

    //TODO: usar OnParametersSetAsync ?    
    protected override async Task OnInitializedAsync()
    {
        await GetTable();
    }

    // ---------------- SELECT TABLE
    private IReadOnlyList<Lista>? _listaList { get; set; }
    private IReadOnlyList<Lista>? _listaListFiltered { get; set; }
    private MudTable<Lista>? table;
    protected async Task GetTable()
    {
        // await Task.Delay(10000);
        IReadOnlyList<Lista> listas = await ListasService.From();
        _listaList = listas;
        _listaListFiltered = listas;
        await InvokeAsync(StateHasChanged);
    }

    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        _listaListFiltered = _listaList?.Where(row => row.Titulo.Contains(text)).ToList();
    }
    
    // ---------------- DELETE
    private async Task OnClickDelete(Lista item)
    {
        await ListasService.Delete(item);        
        await GetTable();
    }

    // ---------------- CREATE NEW

    protected Lista model = new();
    private bool success = false;
    string[] errors = { };
    MudForm? form;
    private bool _novaListaCarregando = false;
    private async Task OnClickCriarLista()
    {
        _novaListaCarregando = true;
        await InvokeAsync(StateHasChanged);
        
        string nome = "Lista do dia " + String.Format("{0:d/MM/yyyy}", DateTime.Now);
        model = new()
        {
            Titulo = nome,
            User_id = "a7251a08-9573-4192-8d14-d4de1ab0ee6e"
        };

        await ListasService.Insert(model);
        model = new();
        await GetTable();
        success = false;
        _novaListaCarregando = false;
    }
}