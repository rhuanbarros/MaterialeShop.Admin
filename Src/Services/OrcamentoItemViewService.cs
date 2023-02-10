using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Providers;

namespace MaterialeShop.Admin.Src.Services;

public class OrcamentoItemViewService
{
    private readonly DatabaseProvider databaseProvider;
    private readonly AuthService authService;
    private readonly ILogger<DatabaseProvider> logger;
    private readonly Supabase.Client client;

    public OrcamentoItemViewService(
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

    public async Task<IReadOnlyList<OrcamentoItemView>> SelectAll()
    {
        logger.LogInformation("------------------- OrcamentoItemViewService SelectAll -------------------");
        Postgrest.Responses.ModeledResponse<OrcamentoItemView> modeledResponse = await client
            .From<OrcamentoItemView>()
            .Where(x => x.SoftDeleted == false)
            .Get();
        return modeledResponse.Models;
    }
    
    public async Task<IReadOnlyList<OrcamentoItemView>> SelectByOrcamentoId(int OrcamentoId)
    {
        logger.LogInformation("------------------- OrcamentoItemViewService SelectByOrcamentoId -------------------");
        Postgrest.Responses.ModeledResponse<OrcamentoItemView> modeledResponse =  await client
            .From<OrcamentoItemView>()
            // .Filter(nameof(OrcamentoItemView.OrcamentoId), Postgrest.Constants.Operator.Equals, id)
            .Where(x => x.OrcamentoId == OrcamentoId)
            .Where(x => x.SoftDeleted == false)
            .Get();
        return modeledResponse.Models;
    }
    
    public async Task<IReadOnlyList<OrcamentoItemView>> SelectByListaId(int ListaId)
    {
        logger.LogInformation("------------------- OrcamentoItemViewService SelectByOrcamentoId -------------------");
        Postgrest.Responses.ModeledResponse<OrcamentoItemView> modeledResponse =  await client
            .From<OrcamentoItemView>()
            // .Filter(nameof(OrcamentoItemView.OrcamentoId), Postgrest.Constants.Operator.Equals, id)
            .Where(x => x.ListaId == ListaId)
            .Where(x => x.SoftDeleted == false)
            .Get();
        return modeledResponse.Models;
    }

}