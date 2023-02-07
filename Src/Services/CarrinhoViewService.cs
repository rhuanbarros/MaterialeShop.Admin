using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Providers;

namespace MaterialeShop.Admin.Src.Services;

public class CarrinhoViewService
{
    private readonly DatabaseProvider databaseProvider;
    private readonly AuthService authService;
    private readonly ILogger<DatabaseProvider> logger;
    private readonly Supabase.Client client;

    public CarrinhoViewService(
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

    public async Task<IReadOnlyList<CarrinhoView>> SelectAll()
    {
        logger.LogInformation("------------------- CarrinhoViewService SelectAll -------------------");

        Postgrest.Responses.ModeledResponse<CarrinhoView> modeledResponse = await client
            .From<CarrinhoView>()
            .Where(x => x.SoftDeleted == false)
            .Get();
        return modeledResponse.Models;
    }
    
    public async Task<List<CarrinhoView>> Delete(CarrinhoView item)
    {
        logger.LogInformation("------------------- CarrinhoViewService Delete -------------------");

        Postgrest.Responses.ModeledResponse<CarrinhoView> modeledResponse = await client
            .From<CarrinhoView>()
            .Delete(item);
        return modeledResponse.Models;
    }

    public async Task<IReadOnlyList<CarrinhoView>> FindCarrinhoView(int ListaId, int PerfilId, int OrcamentoId)
    {
        logger.LogInformation("------------------- CarrinhoViewService FindCarrinhoView -------------------");

        Postgrest.Responses.ModeledResponse<CarrinhoView> modeledResponse = await client
            .From<CarrinhoView>()
            // .Filter(nameof(CarrinhoView.PerfilId), Postgrest.Constants.Operator.Equals, PerfilId)
            // .Filter(nameof(CarrinhoView.OrcamentoId), Postgrest.Constants.Operator.Equals, OrcamentoId)
            // .Filter(nameof(CarrinhoView.ListaId), Postgrest.Constants.Operator.Equals, ListaId)
            // .Filter(x => x.PerfilId == PerfilId)
            // .Filter(x => x.OrcamentoId == OrcamentoId)
            // .Filter(x => x.ListaId == ListaId)
            .Where(x => x.PerfilId == PerfilId)
            .Where(x => x.OrcamentoId == OrcamentoId)
            .Where(x => x.ListaId == ListaId)
            .Get();
        return modeledResponse.Models;
    }

    public async Task<IReadOnlyList<CarrinhoView>> SelectAllByListaId(int id, string status)
    {
        logger.LogInformation("------------------- CarrinhoViewService SelectAllByListaId -------------------");

        Postgrest.Responses.ModeledResponse<CarrinhoView> modeledResponse = await client
            .From<CarrinhoView>()
            // .Filter(nameof(CarrinhoView.ListaId), Postgrest.Constants.Operator.Equals, id)
            .Where(x => x.ListaId == id)
            .Where(x => x.SoftDeleted == false)
            .Where(x => x.Status == status)
            .Get();
        return modeledResponse.Models;
    }

    public async Task<IReadOnlyList<CarrinhoView>> SelectAllByStatus(string status)
    {
        logger.LogInformation("------------------- CarrinhoViewService SelectAllByStatus -------------------");

        Postgrest.Responses.ModeledResponse<CarrinhoView> modeledResponse = await client
            .From<CarrinhoView>()
            // .Filter(nameof(ListasView.Status), Postgrest.Constants.Operator.Equals, status)
            .Where(x => x.Status == status)
            .Where(x => x.SoftDeleted == false)
            .Get();
        return modeledResponse.Models;
    }

}