using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MaterialeShop.Admin.Src.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.OrcamentosListaFolder;

public partial class OrcamentosComponent
{
    //TODO: fazer aparecer apenas as lojas nao apagadas

    [Parameter]
    public int ListaId { get; set; }

    [Inject]
    protected OrcamentoViewService OrcamentoViewService { get; set; }
    
    [Inject]
    protected OrcamentoService OrcamentoService { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await GetTable();
        await GetTableLoja();
    }

    protected override async Task GetTable()
    {
        _tableList = await OrcamentoViewService.SelectAllByListaId(ListaId);
        _tableListFiltered = _tableList;
        await InvokeAsync(StateHasChanged);
    }

    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        Func<OrcamentoView, bool> predicate = row =>
        {
            if (
                !string.IsNullOrEmpty(row.LojaNome) && row.LojaNome.ToLower().Contains(text.ToLower())
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
            CreatedAt = item.CreatedAt,
            LojaId = item.LojaId,
            ListaId = ListaId,
            SolicitacaoData = item.SolicitacaoData,
            SolicitacaoHora = item.SolicitacaoHora,
            Recebido = item.Recebido,
            RecebidoData = item.RecebidoData,
            EntregaPreco = item.EntregaPreco,
            EntregaPrazo = item.EntregaPrazo,
            DescontoNoTotal = item.DescontoNoTotal,
            CodigoLoja = item.CodigoLoja
        };
    }

    // -------------------START------------------- CAMPO LojaId no MODEL  ----------------------------------------

    // ---------------- SELECT TABLE Loja
    protected List<Loja>? _LojaList { get; set; }
    protected async Task GetTableLoja()
    {
        _LojaList = (List<Loja>?)await CrudService.SelectAllFrom<Loja>();
        await InvokeAsync(StateHasChanged);
    }

    private Func<Loja, string> convertFuncPapel = ci => ci?.Nome;
    // -----------------END--------------------- CAMPO LojaId no MODEL  ----------------------------------------

    // ---------------- CLICK NA LINHA DA TABELA
    private void RowClickEvent(TableRowClickEventArgs<OrcamentoView> e)
    {
        NavigationManager.NavigateTo(Rotas.Orcamentos_itens(ListaId, e.Item.OrcamentoId));
    }

    private bool mudSelectValidation(int value)
    {
        // Console.WriteLine("value");
        // Console.WriteLine(value);

        // Console.WriteLine("LISTA");
        // foreach (var item in _UsuarioPerfilList)
        // {
        //     Console.WriteLine("item.Id");
        //     Console.WriteLine(item.Id);
        // }

        int quantidade = _LojaList!.FindAll(x => x.Id == value).Count();
        if (quantidade == 0)
            return false;
        else
            return true;
    }

    protected override async Task OnClickEdit(OrcamentoView item)
    {
        await base.OnClickEdit(item);
        await GetFilesFromBucket();
    }

    // --------------------------- FILE UPLOAD

    private const string BucketName = "orcamentos";

    public List<Supabase.Storage.FileObject>? fileObjects;
    private async Task GetFilesFromBucket()
    {
        fileObjects = await StorageService.GetFilesFromBucket(BucketName, ListaId.ToString() +"/"+ model.LojaId);
    }

    static long maxFileSizeInMB = 15;
    private async Task UploadFilesAsync(IBrowserFile file)
    {
        try
        {
            Loja? loja = _LojaList.Find( f => f.Id == model.LojaId);
            String nomeLoja = loja?.Nome;

            string filename = await StorageService.UploadFile( file, BucketName, ListaId.ToString() +"/"+ model.LojaId, nomeLoja );

            Snackbar.Add("File uploaded: " + filename.Split("/").Last());

            await GetFilesFromBucket();
            await InvokeAsync(StateHasChanged);

            await OrcamentoService.SetLastUploadedFileName(model.Id, filename.Split("/").Last() );

        } catch ( NullReferenceException  ex)
        {
            Snackbar.Add("Não é possível fazer upload porque não foi possível carregar os nomes das lojas para colocar no nome do arquivo.");
            return;
        } catch (System.IO.IOException ex)
        {
            Snackbar.Add("Error: Max file size exceeded.");
        }
    }

    private async Task DownloadClick(Supabase.Storage.FileObject row)
    {
        byte[] downloadedBytes = await StorageService.DownloadFile(BucketName, ListaId.ToString() +"/"+ model.LojaId, row.Name);

        await JS.InvokeVoidAsync("downloadFileFromStream", row.Name, downloadedBytes);
    }

}