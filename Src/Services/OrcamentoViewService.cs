using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Providers;

namespace MaterialeShop.Admin.Src.Services;

public class OrcamentoViewService
{
    private readonly DatabaseProvider databaseProvider;
    private readonly AuthService authService;
    private readonly ILogger<DatabaseProvider> logger;
    private readonly Supabase.Client client;

    public OrcamentoViewService(
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

    public async Task<IReadOnlyList<OrcamentoView>> SelectAll()
    {
        logger.LogInformation("------------------- OrcamentoViewService SelectAll -------------------");
        Postgrest.Responses.ModeledResponse<OrcamentoView> modeledResponse = await client.From<OrcamentoView>().Get();
        return modeledResponse.Models;
    }
    
    public async Task<IReadOnlyList<OrcamentoView>> SelectAllByListaId(int id)
    {
        logger.LogInformation("------------------- OrcamentoViewService SelectAllByListaId -------------------");
        Postgrest.Responses.ModeledResponse<OrcamentoView> modeledResponse = await client.From<OrcamentoView>().Filter(nameof(OrcamentoView.ListaId), Postgrest.Constants.Operator.Equals, id).Get();
        return modeledResponse.Models;
    }
    
    

}