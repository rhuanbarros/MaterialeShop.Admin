using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Postgrest.Models;

namespace MaterialeShop.Admin.Src.Services;

public class CrudService
{
    private readonly Supabase.Client client;
    private readonly AuthenticationStateProvider customAuthStateProvider;
    private readonly ILocalStorageService localStorage;
    private readonly ILogger<CrudService> logger;

    public CrudService(
        Supabase.Client client,
        AuthenticationStateProvider CustomAuthStateProvider,
        ILocalStorageService localStorage,
        ILogger<CrudService> logger
    ) : base()
    {
        logger.LogInformation("------------------- CONSTRUCTOR -------------------");

        this.client = client;
        this.customAuthStateProvider = CustomAuthStateProvider;
        this.localStorage = localStorage;
        this.logger = logger;
    }

    public async Task<IReadOnlyList<TModel>> SelectFrom<TModel>() where TModel : BaseModel, new()
    {
        Postgrest.Responses.ModeledResponse<TModel> modeledResponse = await client.From<TModel>().Get();
        return modeledResponse.Models;
    }

    public async Task<List<TModel>> Delete<TModel>(TModel item) where TModel : BaseModel, new()
    {
        Postgrest.Responses.ModeledResponse<TModel> modeledResponse = await client.From<TModel>().Delete(item);
        return modeledResponse.Models;
    }

    public async Task<List<TModel>> Insert<TModel>(TModel item) where TModel : BaseModel, new()
    {
        Postgrest.Responses.ModeledResponse<TModel> modeledResponse = await client.From<TModel>().Insert(item);
        return modeledResponse.Models;
    }
    
    public async Task<List<TModel>> Edit<TModel>(TModel item) where TModel : BaseModel, new()
    {
        Postgrest.Responses.ModeledResponse<TModel> modeledResponse = await client.From<TModel>().Update(item);
        return modeledResponse.Models;
    }

}
