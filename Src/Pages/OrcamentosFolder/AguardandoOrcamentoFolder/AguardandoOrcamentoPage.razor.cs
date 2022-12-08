using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.AguardandoOrcamentoFolder;

public partial class AguardandoOrcamentoPage
{
    [Inject] 
    protected ListasViewService ListasViewService {get; set;}

    protected override async Task OnParametersSetAsync()
    {
        await GetTable();
        await GetTableUsuarioPerfil();
    }

    // ---------------- SELECT TABLE
    protected override async Task GetTable()
    {
        _tableList = await ListasViewService.SelectAllByStatus("Aguardando orçamentos");
        _tableListFiltered = _tableList;
        await InvokeAsync(StateHasChanged);
    }
    

    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        Func<ListasView, bool> predicate = row =>
        {
            if (
                !string.IsNullOrEmpty(row.NomeCompleto) && row.NomeCompleto.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Endereco) && row.Endereco.ToLower().Contains(text.ToLower())
                || !string.IsNullOrEmpty(row.Status.ToString()) && row.Status.ToString().ToLower().Contains(text.ToLower())
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

    protected override Lista model {get;set;} = new()
    {
        Status = "Em criação"
    };

    // ---------------- CREATE NEW
    protected override Lista CreateNewModel()
    {
        model = new()
        {
            Status = "Em criação"
        };
        return model;
    }

    
    protected override async Task OnClickCancel()
    {
        //precisei sobrescrever o OnClickCancel pq essa classe nao tava chamando o CreateNewModel dessa classe e sim das classes base
        form?.Reset();
        model = CreateNewModel();
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
            PerfilId = item.PerfilId,
            Endereco = item.Endereco,
            Status = item.Status
        };
    }

    // -------------------START------------------- CAMPO UsuarioPerfil no MODEL  ----------------------------------------

    // ---------------- SELECT TABLE UsuarioPerfil
    protected IReadOnlyList<Perfil>? _UsuarioPerfilList { get; set; }
    protected async Task GetTableUsuarioPerfil()
    {
        _UsuarioPerfilList = await CrudService.SelectAllFrom<Perfil>();
        await InvokeAsync(StateHasChanged);
    }

    private Func<Perfil, string> convertFuncPapel = ci => ci?.NomeCompleto;
    // -----------------END--------------------- CAMPO UsuarioPerfil no MODEL  ----------------------------------------

}