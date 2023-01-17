using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;

namespace MaterialeShop.Admin.Src.Shared.Components;

public partial class ListaViewCard : ComponentBase
{
    [Inject]
    protected ListasViewService ListasViewService { get; set; }

    [Parameter]
    public int ListaId { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await GetListaView();
    }

    // ---------------- GET ListaView
    private ListasView _ListaView { get; set; }
    private string NomeCliente = "Carregando";
    private string Endereco = "Carregando";
    protected async Task GetListaView()
    {
        _ListaView = await ListasViewService.SelectAllByListaId(ListaId);
        NomeCliente = _ListaView?.NomeCompleto;
        Endereco = _ListaView?.Endereco;
    }
}