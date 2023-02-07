using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Providers;

namespace MaterialeShop.Admin.Src.Services;

public class CarrinhoGroupByListaViewService
{
    private readonly DatabaseProvider databaseProvider;
    private readonly AuthService authService;
    private readonly ILogger<DatabaseProvider> logger;
    private readonly Supabase.Client client;

    public CarrinhoGroupByListaViewService(
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

    public async Task<IReadOnlyList<CarrinhoGroupByListaView>> SelectAll()
    {
        logger.LogInformation("------------------- CarrinhoGroupByListaViewService SelectAll -------------------");

        Postgrest.Responses.ModeledResponse<CarrinhoGroupByListaView> modeledResponse = await client
            .From<CarrinhoGroupByListaView>()
            .Where(x => x.SoftDeleted == false)
            .Get();
        return modeledResponse.Models;
    }
    
    public async Task<IReadOnlyList<CarrinhoGroupByListaView>> SelectAllByListaId(int listaId)
    {
        logger.LogInformation("------------------- CarrinhoGroupByListaViewService SelectAll -------------------");

        Postgrest.Responses.ModeledResponse<CarrinhoGroupByListaView> modeledResponse = await client
            .From<CarrinhoGroupByListaView>()
            .Where(x => x.ListaId == listaId)
            .Get();
        return modeledResponse.Models;
    }
    
    

}