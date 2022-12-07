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

    protected override async Task OnParametersSetAsync()
    {
        await GetListaView();
        // await GetTable();
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
        Func<Loja, bool> predicate = row =>
        {
            if (
                !string.IsNullOrEmpty(row.Nome) && row.Nome.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Cnpj) && row.Cnpj.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Email) && row.Email.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Telefone) && row.Telefone.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Endereco) && row.Endereco.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Cidade) && row.Cidade.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Estado) && row.Estado.ToLower().Contains(text.ToLower())
            )
                return true;
            else
                return false;
        };
        _tableListFiltered = _tableList?.Where(predicate).ToList();
    }

}