using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Providers;

namespace MaterialeShop.Admin.Src.Services;

public class CarrinhoService
{
    private readonly DatabaseProvider databaseProvider;
    private readonly AuthService authService;
    private readonly ILogger<DatabaseProvider> logger;
    private readonly Supabase.Client client;

    public CarrinhoService(
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

    public async Task<IReadOnlyList<Carrinho>> SelectAll()
    {
        logger.LogInformation("------------------- CarrinhoService SelectAll -------------------");

        Postgrest.Responses.ModeledResponse<Carrinho> modeledResponse = await client
            .From<Carrinho>()
            .Where(x => x.SoftDeleted == false)
            .Get();
        return modeledResponse.Models;
    }
    
    public async Task<List<Carrinho>> Delete(Carrinho item)
    {
        logger.LogInformation("------------------- CarrinhoService Delete -------------------");

        Postgrest.Responses.ModeledResponse<Carrinho> modeledResponse = await client
            .From<Carrinho>()
            .Delete(item);
        return modeledResponse.Models;
    }
    
    public async Task<List<Carrinho>> Insert(Carrinho item)
    {
        logger.LogInformation("------------------- CarrinhoService Insert -------------------");

        Postgrest.Responses.ModeledResponse<Carrinho> modeledResponse = await client
            .From<Carrinho>()
            .Insert(item);
        return modeledResponse.Models;
    }

    public async Task<List<Carrinho>> Upsert(Carrinho item)
    {
        logger.LogInformation("------------------- CarrinhoService Upsert -------------------");

        Postgrest.Responses.ModeledResponse<Carrinho> modeledResponse = await client
            .From<Carrinho>()
            .OnConflict(f => f.ListaId)
            .OnConflict(f => f.PerfilId)
            .OnConflict(f => f.OrcamentoId)
            .Insert(item);
        return modeledResponse.Models;
    }

    public async Task<IReadOnlyList<Carrinho>> FindCarrinho(int ListaId, int PerfilId, int OrcamentoId)
    {
        logger.LogInformation("------------------- CarrinhoService FindCarrinho -------------------");

        Postgrest.Responses.ModeledResponse<Carrinho> modeledResponse = await client
            .From<Carrinho>()
            // .Filter(nameof(Carrinho.PerfilId), Postgrest.Constants.Operator.Equals, PerfilId)
            // .Filter(nameof(Carrinho.OrcamentoId), Postgrest.Constants.Operator.Equals, OrcamentoId)
            // .Filter(nameof(Carrinho.ListaId), Postgrest.Constants.Operator.Equals, ListaId)
            // .Filter(x => x.PerfilId == PerfilId)
            // .Filter(x => x.OrcamentoId == OrcamentoId)
            // .Filter(x => x.ListaId == ListaId)
            .Where(x => x.PerfilId == PerfilId)
            .Where(x => x.OrcamentoId == OrcamentoId)
            .Where(x => x.ListaId == ListaId)
            .Get();
        return modeledResponse.Models;
    }

    public async Task<IReadOnlyList<Carrinho>> SelectAllByListaId(int id)
    {
        logger.LogInformation("------------------- CarrinhoService SelectAllByListaId -------------------");

        Postgrest.Responses.ModeledResponse<Carrinho> modeledResponse = await client
            .From<Carrinho>()
            // .Filter(nameof(Carrinho.ListaId), Postgrest.Constants.Operator.Equals, id)
            .Where(x => x.ListaId == id)
            .Where(x => x.SoftDeleted == false)
            .Get();
        return modeledResponse.Models;
    }

}