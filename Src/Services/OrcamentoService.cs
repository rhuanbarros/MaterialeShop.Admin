using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Providers;
using Postgrest.Models;
using Supabase.Gotrue;
using Supabase.Interfaces;
using Supabase.Realtime;
using Supabase.Storage;

namespace MaterialeShop.Admin.Src.Services;

public class OrcamentoService
{
    private readonly DatabaseProvider databaseProvider;
    private readonly AuthService authService;
    private readonly ILogger<DatabaseProvider> logger;
    private readonly Supabase.Client client;

    public OrcamentoService(
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

    public async Task<IReadOnlyList<Orcamento>> SelectAll()
    {
        logger.LogInformation("------------------- OrcamentoService SelectAll -------------------");

        Postgrest.Responses.ModeledResponse<Orcamento> modeledResponse = await client.From<Orcamento>().Filter("SoftDelete", Postgrest.Constants.Operator.Equals, "false").Get();
        return modeledResponse.Models;
    }
    
    public async Task<List<Orcamento>> Delete(Orcamento item)
    {
        logger.LogInformation("------------------- OrcamentoService Delete -------------------");

        Postgrest.Responses.ModeledResponse<Orcamento> modeledResponse = await client.From<Orcamento>().Delete(item);
        return modeledResponse.Models;
    }
    
    public async Task<List<Orcamento>> Insert(Orcamento item)
    {
        logger.LogInformation("------------------- OrcamentoService Insert -------------------");

        Postgrest.Responses.ModeledResponse<Orcamento> modeledResponse = await client.From<Orcamento>().Insert(item);
        return modeledResponse.Models;
    }

    public async Task<IReadOnlyList<Orcamento>> SelectAllByListaId(int id)
    {
        logger.LogInformation("------------------- OrcamentoService SelectAllByListaId -------------------");

        Postgrest.Responses.ModeledResponse<Orcamento> modeledResponse = await client.From<Orcamento>().Filter(nameof(Orcamento.ListaId), Postgrest.Constants.Operator.Equals, id).Filter("SoftDelete", Postgrest.Constants.Operator.Equals, "false").Get();
        return modeledResponse.Models;
    }

}