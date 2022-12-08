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
        Func<OrcamentoView, bool> predicate = row =>
        {
            if (
                !string.IsNullOrEmpty(row.Nome) && row.Nome.ToLower().Contains(text.ToLower())
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

    // ---------------- DELETE
    protected override Orcamento SetModelIdToDelete(OrcamentoView item)
    {
        return new Orcamento()
            {
                Id = item.OrcamentoId
            };
    }

    // ---------------- CREATE NEW
    protected override Orcamento SetModelReferenceId(Orcamento item)
    {
        item.ListaId = ListaId;
        return item;
    }

    // ---------------- EDIT MODEL
    protected override Orcamento SetModelIdToEdit(OrcamentoView item)
    {
        return new Orcamento()
        {
            Id = item.OrcamentoId,
            LojaId = item.LojaId,
            ListaId = ListaId
        };
    }

    ////////////////////////////////////////////////////////////////

                // Fazer a view retornar a Loja.Id tbm para poder setar aqui         
                // depois tem q fazer o campo de selecionar a loja para criar um novo
                // tbm tem q fazer o campo ser setado corretamente na hora de editar
                // tbm tem q arrumar a outra pagina q ja usa o BaseCrudViewPageComponent

    ////////////////////////////////

}