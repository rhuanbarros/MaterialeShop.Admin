using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.SelecionarLojaFolder;

public partial class SelecionarLojaPage
{
    [Parameter]
    public int ListaId { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await GetTable();
        await GetTableLoja();
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

    // -------------------START------------------- CAMPO LojaId no MODEL  ----------------------------------------

    // ---------------- SELECT TABLE Loja
    protected IReadOnlyList<Loja>? _LojaList { get; set; }
    protected async Task GetTableLoja()
    {
        _LojaList = await CrudService.SelectAllFrom<Loja>();
        await InvokeAsync(StateHasChanged);
    }

    private Func<Loja, string> convertFuncPapel = ci => ci?.Nome;
    // -----------------END--------------------- CAMPO UsuarioPerfil no MODEL  ----------------------------------------

}