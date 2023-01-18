using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Providers;
using Postgrest.Models;
using Supabase.Gotrue;
using Supabase.Interfaces;
using Supabase.Realtime;
using Supabase.Storage;

namespace MaterialeShop.Admin.Src.Services;

public class ListaItensService
{
    private readonly DatabaseProvider databaseProvider;
    private readonly AuthService authService;
    private readonly ILogger<DatabaseProvider> logger;
    private readonly Supabase.Client client;

    public ListaItensService(
        DatabaseProvider DatabaseProvider,
        AuthService AuthService,
        ILogger<DatabaseProvider> logger
        )
    {
        databaseProvider = DatabaseProvider;
        authService = AuthService;
        this.logger = logger;

        this.client = databaseProvider.client;
    }

    public async Task<IReadOnlyList<ListaItem>> SelectAll()
    {
        logger.LogInformation("------------------- ListaItensService SelectAll -------------------");

        Postgrest.Responses.ModeledResponse<ListaItem> modeledResponse = await client
            .From<ListaItem>()
            .Where(x => x.SoftDeleted == false)
            .Get();
        return modeledResponse.Models;
    }
    
    public async Task<List<ListaItem>> Delete(ListaItem item)
    {
        logger.LogInformation("------------------- ListaItensService Delete -------------------");

        Postgrest.Responses.ModeledResponse<ListaItem> modeledResponse = await client
            .From<ListaItem>()
            .Delete(item);
        return modeledResponse.Models;
    }
    
    public async Task<List<ListaItem>> Insert(ListaItem item)
    {
        logger.LogInformation("------------------- ListaItensService Insert -------------------");

        Postgrest.Responses.ModeledResponse<ListaItem> modeledResponse = await client
            .From<ListaItem>()
            .Insert(item);
        return modeledResponse.Models;
    }

    public async Task<IReadOnlyList<ListaItem>> SelectAllByListaId(int id)
    {
        logger.LogInformation("------------------- ListaItensService SelectAllByListaId -------------------");

        Postgrest.Responses.ModeledResponse<ListaItem> modeledResponse = await client
            .From<ListaItem>()
            // .Filter(nameof(ListaItem.ListaId), Postgrest.Constants.Operator.Equals, id)
            .Where(x => x.ListaId == id)
            // .Order(nameof(ListaItem.CreatedAt), Postgrest.Constants.Ordering.Ascending)
            .Where(x => x.SoftDeleted == false)
            .Order(x => x.CreatedAt, Postgrest.Constants.Ordering.Ascending)
            .Get();
        return modeledResponse.Models;
    }

}