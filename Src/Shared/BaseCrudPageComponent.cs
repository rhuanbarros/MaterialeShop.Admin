using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Postgrest.Models;

namespace MaterialeShop.Admin.Src.Shared;

public class BaseCrudPageComponent<TModel> : BasePageComponent where TModel : BaseModelApp, new()
{
    [Inject] 
    protected CrudService CrudService {get; set;}
    
    [Inject]
    protected IDialogService DialogService { get; set; }
    
    [Inject]
    protected AppGlobals AppGlobals { get; set; }

    [Inject]
    protected ISnackbar Snackbar {get;set;}

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
    protected virtual TModel model {get;set;} = new();
    protected bool success = false;
    protected string[] errors = { };
    protected MudForm? form;
    protected bool _processingNewItem = false;

    protected virtual TModel CreateNewModel()
    {
        model = new()
        {
            CreatedAt = DateTime.Now
        };
        return model;
    }
    
    // usar isso para setar o id da chave estrangeira
    protected virtual TModel SetModelReferenceId(TModel item)
    {
        return item;
    }
    protected virtual async Task OnClickSave()
    {
        form?.Validate();
        
        if(form.IsValid)
        {
            _processingNewItem = true;
            if(ModoEdicao == false)
            {
                model = SetModelReferenceId(model);
                await CrudService.Insert<TModel>(model);
            } else 
            {
                //TODO: no futuro, quem sabe fazer ele atualizar apenas os campos que foram alterados
                await CrudService.Edit<TModel>(model);            
                ModoEdicao = false;
            }

            model = CreateNewModel();
            await GetTable();
            success = false;
            _processingNewItem = false;

            Snackbar.Add("Registro salvo com sucesso.");
        }
    }

    protected virtual async Task OnClickCancel()
    {
        form?.Reset();
        model = CreateNewModel();
        ModoEdicao = false;
        success = false;
    }

    // ---------------- DELETE
    protected virtual async Task OnClickDelete(TModel item)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Atenção",
            "Você deseja apagar este item?", 
            yesText:"Sim", cancelText:"Não");
        
        if(result == true)
        {
            await CrudService.SoftDeleted<TModel>(item);
            Snackbar.Add("Registro apagado com sucesso.");
        }
        await GetTable();
        
        form?.Reset();
        model = CreateNewModel();
        
        await InvokeAsync(StateHasChanged);
    }

    // ---------------- EDIT MODEL
    protected bool ModoEdicao = false;
    protected virtual async Task OnClickEdit(TModel item)
    {
        model = item;
        ModoEdicao = true;
    }
    
}