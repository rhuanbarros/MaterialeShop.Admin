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

    public async Task<IReadOnlyList<UsuarioPerfil>> From()
    {
        Postgrest.Responses.ModeledResponse<UsuarioPerfil> modeledResponse = await client.From<UsuarioPerfil>().Get();
        return modeledResponse.Models;
    }
    
    public async Task<IReadOnlyList<UsuarioPerfil>> GetByUserId(string userId)
    {
        Postgrest.Responses.ModeledResponse<UsuarioPerfil> modeledResponse = await client.From<UsuarioPerfil>().Filter("id", Postgrest.Constants.Operator.Equals, userId).Get();
        return modeledResponse.Models;
    }
    

}