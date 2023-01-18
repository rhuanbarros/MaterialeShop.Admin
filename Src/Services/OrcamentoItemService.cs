using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Providers;

namespace MaterialeShop.Admin.Src.Services;

public class OrcamentoItemService
{
    private readonly DatabaseProvider databaseProvider;
    private readonly AuthService authService;
    private readonly ILogger<DatabaseProvider> logger;
    private readonly Supabase.Client client;

    public OrcamentoItemService(
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

    public async Task<IReadOnlyList<OrcamentoItem>> SelectAll()
    {
        logger.LogInformation("------------------- OrcamentoItemService SelectAll -------------------");
        Postgrest.Responses.ModeledResponse<OrcamentoItem> modeledResponse = await client
            .From<OrcamentoItem>()
            .Where(x => x.SoftDeleted == false)
            .Get();
        return modeledResponse.Models;
    }
    
    public async Task<IReadOnlyList<OrcamentoItem>> SelectByOrcamentoId(int id)
    {
        logger.LogInformation("------------------- OrcamentoItemService SelectByOrcamentoId -------------------");
        Postgrest.Responses.ModeledResponse<OrcamentoItem> modeledResponse =  await client
            .From<OrcamentoItem>()
            // .Filter(nameof(OrcamentoItem.OrcamentoId), Postgrest.Constants.Operator.Equals, id)
            .Where(x => x.OrcamentoId == id)
            .Where(x => x.SoftDeleted == false)
            .Get();
        return modeledResponse.Models;
    }

}