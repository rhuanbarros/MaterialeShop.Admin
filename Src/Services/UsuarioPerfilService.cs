using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Providers;
using Postgrest.Models;
using Supabase.Gotrue;
using Supabase.Interfaces;
using Supabase.Realtime;
using Supabase.Storage;

namespace MaterialeShop.Admin.Src.Services;

public class UsuarioPerfilService
{
    private readonly DatabaseProvider databaseProvider;
    private readonly ILogger<DatabaseProvider> logger;
    private readonly Supabase.Client client;

    public UsuarioPerfilService(
        DatabaseProvider DatabaseProvider,
        ILogger<DatabaseProvider> logger
        )
    {
        databaseProvider = DatabaseProvider;
        this.logger = logger;
        this.client = databaseProvider.client;
    }

    public async Task<IReadOnlyList<Perfil>> From()
    {
        Postgrest.Responses.ModeledResponse<Perfil> modeledResponse = await client.From<Perfil>().Get();
        return modeledResponse.Models;
    }
    
    public async Task<IReadOnlyList<Perfil>> GetByUserId(string userId)
    {
        Postgrest.Responses.ModeledResponse<Perfil> modeledResponse = await client.From<Perfil>().Filter(nameof(Perfil.Id), Postgrest.Constants.Operator.Equals, userId).Filter("SoftDelete", Postgrest.Constants.Operator.Equals, "false").Get();
        return modeledResponse.Models;
    }
    

}