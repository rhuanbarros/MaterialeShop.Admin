using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Providers;

namespace MaterialeShop.Admin.Src.Services;

public class CarrinhoItemViewService
{
    private readonly DatabaseProvider databaseProvider;
    private readonly AuthService authService;
    private readonly ILogger<DatabaseProvider> logger;
    private readonly Supabase.Client client;

    public CarrinhoItemViewService(
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

    public async Task<IReadOnlyList<CarrinhoItemView>> SelectAll()
    {
        logger.LogInformation("------------------- CarrinhoItemViewService SelectAll -------------------");

        Postgrest.Responses.ModeledResponse<CarrinhoItemView> modeledResponse = await client
            .From<CarrinhoItemView>()
            .Get();
        return modeledResponse.Models;
    }
    
}