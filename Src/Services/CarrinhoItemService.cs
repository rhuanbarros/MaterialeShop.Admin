using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Providers;

namespace MaterialeShop.Admin.Src.Services;

public class CarrinhoItemService
{
    private readonly DatabaseProvider databaseProvider;
    private readonly AuthService authService;
    private readonly ILogger<DatabaseProvider> logger;
    private readonly Supabase.Client client;

    public CarrinhoItemService(
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

    public async Task<IReadOnlyList<CarrinhoItem>> SelectAll()
    {
        logger.LogInformation("------------------- CarrinhoItemService SelectAll -------------------");

        Postgrest.Responses.ModeledResponse<CarrinhoItem> modeledResponse = await client
            .From<CarrinhoItem>()
            .Where(x => x.SoftDeleted == false)
            .Get();
        return modeledResponse.Models;
    }
    
    public async Task<List<CarrinhoItem>> Delete(CarrinhoItem item)
    {
        logger.LogInformation("------------------- CarrinhoItemService Delete -------------------");

        Postgrest.Responses.ModeledResponse<CarrinhoItem> modeledResponse = await client
            .From<CarrinhoItem>()
            .Delete(item);
        return modeledResponse.Models;
    }
    
    public async Task<List<CarrinhoItem>> Insert(CarrinhoItem item)
    {
        logger.LogInformation("------------------- CarrinhoItemService Insert -------------------");

        Postgrest.Responses.ModeledResponse<CarrinhoItem> modeledResponse = await client
            .From<CarrinhoItem>()
            .Insert(item);
        return modeledResponse.Models;
    }

    public async Task<List<CarrinhoItem>> Upsert(CarrinhoItem item)
    {
        logger.LogInformation("------------------- CarrinhoItemService Upsert -------------------");

        Postgrest.Responses.ModeledResponse<CarrinhoItem> modeledResponse = await client
            .From<CarrinhoItem>()
            .OnConflict(f => f.CarrinhoId)
            .OnConflict(f => f.OrcamentoItemId)
            .Insert(item);
        return modeledResponse.Models;
    }
    
    public async Task<List<CarrinhoItem>> Upsert(List<CarrinhoItem> item)
    {
        logger.LogInformation("------------------- CarrinhoItemService Upsert -------------------");

        Postgrest.Responses.ModeledResponse<CarrinhoItem> modeledResponse = await client
            .From<CarrinhoItem>()
            .OnConflict(f => f.CarrinhoId)
            .OnConflict(f => f.OrcamentoItemId)
            .Insert(item);
        return modeledResponse.Models;
    }

}