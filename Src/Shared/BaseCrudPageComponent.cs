using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Postgrest.Models;

namespace MaterialeShop.Admin.Src.Shared;

public class BaseCrudPageComponent<TModel> : BasePageComponent where TModel : BaseModel, new()
{
    [Inject] 
    protected CrudService CrudService {get; set;}

    protected override async Task OnParametersSetAsync()
    {
        await GetTable();
    }

    // ---------------- SELECT TABLE
    protected IReadOnlyList<TModel>? _tableList { get; set; }
    protected IReadOnlyList<TModel>? _tableListFiltered { get; set; }
    protected MudTable<TModel>? table;
    
    protected async Task GetTable()
    {
        _tableList = await CrudService.From<TModel>();
        _tableListFiltered = _tableList;
        await InvokeAsync(StateHasChanged);
    }

    // ---------------- CREATE NEW

    protected TModel model = new();
    protected bool success = false;
    protected string[] errors = { };
    protected MudForm? form;
    protected bool _processingNewItem = false;
    protected async Task OnClickSave()
    {
        _processingNewItem = true;
        await CrudService.Insert<TModel>(model);
        model = new();
        await GetTable();
        success = false;
        _processingNewItem = false;
    }

}