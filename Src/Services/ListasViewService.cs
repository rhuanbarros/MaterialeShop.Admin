using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Providers;
using Postgrest.Models;
using Supabase.Gotrue;
using Supabase.Interfaces;
using Supabase.Realtime;
using Supabase.Storage;

namespace MaterialeShop.Admin.Src.Services;

public class ListasViewService
{
    private readonly DatabaseProvider databaseProvider;
    private readonly AuthService authService;
    private readonly ILogger<DatabaseProvider> logger;
    private readonly Supabase.Client client;

    public ListasViewService(
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

    public async Task<IReadOnlyList<ListasView>> SelectAll()
    {
        logger.LogInformation("------------------- ListasViewService SelectAll -------------------");
        Postgrest.Responses.ModeledResponse<ListasView> modeledResponse = await client
            .From<ListasView>()
            .Where(x => x.SoftDeleted == false)
            .Get();
        return modeledResponse.Models;
    }
    
    public async Task<ListasView?> SelectAllByListaId(int id)
    {
        logger.LogInformation("------------------- ListasViewService SelectAllByListaId -------------------");
        return await client
            .From<ListasView>()
            // .Filter(nameof(ListasView.ListaId), Postgrest.Constants.Operator.Equals, id)
            .Where(x => x.ListaId == id)
            .Where(x => x.SoftDeleted == false)
            .Single();
    }
    
    public async Task<IReadOnlyList<ListasView>> SelectAllByStatus(string status)
    {
        logger.LogInformation("------------------- ListasViewService SelectAllByStatus -------------------");
        Postgrest.Responses.ModeledResponse<ListasView> modeledResponse = await client
            .From<ListasView>()
            // .Filter(nameof(ListasView.Status), Postgrest.Constants.Operator.Equals, status)
            .Where(x => x.Status == status)
            .Where(x => x.SoftDeleted == false)
            .Get();
        return modeledResponse.Models;
    }
    
    

}