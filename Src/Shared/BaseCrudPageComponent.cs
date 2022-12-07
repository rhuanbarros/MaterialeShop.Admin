using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Postgrest.Models;

namespace MaterialeShop.Admin.Src.Shared;

public class BaseCrudPageComponent<TModel> : BasePageComponent where TModel : BaseModel, new()
{
    [Inject] 
    protected CrudService CrudService {get; set;}
    [Inject]
    protected IDialogService DialogService { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await GetTable();
    }

    // ---------------- SELECT TABLE
    protected IReadOnlyList<TModel>? _tableList { get; set; }
    protected IReadOnlyList<TModel>? _tableListFiltered { get; set; }
    protected MudTable<TModel>? table;
    
    protected virtual async Task GetTable()
    {
        _tableList = await CrudService.SelectAllFrom<TModel>();
        _tableListFiltered = _tableList;
        await InvokeAsync(StateHasChanged);
    }

    // ---------------- CREATE NEW
    protected TModel model = new();
    protected bool success = false;
    protected string[] errors = { };
    protected MudForm? form;
    protected bool _processingNewItem = false;
    protected virtual async Task OnClickSave()
    {
        form?.Validate();
        
        if(form.IsValid)
        {
            _processingNewItem = true;
            if(ModoEdicao == false)
            {
                await CrudService.Insert<TModel>(model);
            } else 
            {
                await CrudService.Edit<TModel>(model);            
                ModoEdicao = false;
            }

            model = new();
            await GetTable();
            success = false;
            _processingNewItem = false;
        }
    }

    protected async Task OnClickCancel()
    {
        form?.Reset();
        model = new();
    }

    // ---------------- DELETE
    protected async Task OnClickDelete(TModel item)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Atenção",
            "Você deseja apagar este item?", 
            yesText:"Sim", cancelText:"Não");
        
        if(result == true)
        {
            await CrudService.Delete<TModel>(item);
        }
        await GetTable();
    }

    // ---------------- EDIT MODEL
    protected bool ModoEdicao = false;
    protected async Task OnClickEdit(TModel item)
    {
        //essa linha gera um bug q ele edita a instancia e ja aperece na tabela na tela.
        //isso acontece por causa da passagem por referencia.
        // teria q criar um novo
        model = item;
        ModoEdicao = true;
    }
    
}