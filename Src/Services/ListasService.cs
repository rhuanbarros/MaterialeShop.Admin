using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Providers;
using Postgrest.Models;
using Supabase.Gotrue;
using Supabase.Interfaces;
using Supabase.Realtime;
using Supabase.Storage;

namespace MaterialeShop.Admin.Src.Services;

public class ListasService
{
    private readonly DatabaseProvider databaseProvider;
    private readonly AuthService authService;
    private readonly ILogger<DatabaseProvider> logger;
    private readonly Supabase.Client client;

    public ListasService(
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

    public async Task<IReadOnlyList<Lista>> From()
    {
        logger.LogInformation("------------------- ListasService From -------------------");
        Postgrest.Responses.ModeledResponse<Lista> modeledResponse = await client.From<Lista>().Filter("SoftDelete", Postgrest.Constants.Operator.Equals, "false").Get();
        return modeledResponse.Models;
    }
    
    public async Task<List<Lista>> Delete(Lista item)
    {
        Postgrest.Responses.ModeledResponse<Lista> modeledResponse = await client.From<Lista>().Delete(item);
        return modeledResponse.Models;
    }
    
    public async Task<List<Lista>> Insert(Lista item)
    {
        // string userId = authService?.CurrentUser?.Id;
        // Console.WriteLine("---------------userId");
        // Console.WriteLine(userId);
        // Console.WriteLine("---------------userId");
        // item.User_id = userId;
        Postgrest.Responses.ModeledResponse<Lista> modeledResponse = await client.From<Lista>().Insert(item);
        return modeledResponse.Models;
    }

}