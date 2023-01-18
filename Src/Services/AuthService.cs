using Blazored.LocalStorage;
using MaterialeShop.Admin.Src.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using Supabase.Gotrue;
using Supabase.Interfaces;
using Supabase.Realtime;
using Supabase.Storage;

namespace MaterialeShop.Admin.Src.Services;

public class AuthService
{
    private readonly Supabase.Client client;
    private readonly AuthenticationStateProvider customAuthStateProvider;
    private readonly UsuarioPerfilService usuarioPerfilService;
    private readonly ILocalStorageService localStorage;
    private readonly ILogger<AuthService> logger;
    public Perfil UsuarioPerfil {get;set;}

    public Perfil? UsuarioLogado { get; private set; }

    public AuthService(
        Supabase.Client client,
        AuthenticationStateProvider CustomAuthStateProvider,
        UsuarioPerfilService UsuarioPerfilService,
        ILocalStorageService localStorage,
        ILogger<AuthService> logger
    ) : base()
    {
        logger.LogInformation("------------------- CONSTRUCTOR -------------------");

        this.client = client;
        this.customAuthStateProvider = CustomAuthStateProvider;
        this.usuarioPerfilService = UsuarioPerfilService;
        this.localStorage = localStorage;
        this.logger = logger;

        client.InitializeAsync();
        client.Auth.RetrieveSessionAsync();
    }

    public async Task Login(string email, string password)
    {
        logger.LogInformation("METHOD: Login");
        
        Session? session = await client.Auth.SignIn(email, password);

        logger.LogInformation("------------------- User logged in -------------------");
        logger.LogInformation($"instance.Auth.CurrentUser.Id {client?.Auth?.CurrentUser?.Id}");
        logger.LogInformation($"client.Auth.CurrentUser.Email {client?.Auth?.CurrentUser?.Email}");

        await customAuthStateProvider.GetAuthenticationStateAsync();

        //guarda o perfi do usuario
        IReadOnlyList<Perfil> perfil = await usuarioPerfilService.GetByUserId( Int32.Parse( client?.Auth?.CurrentUser?.Id ) );
        UsuarioPerfil = perfil.Single();
    }
    
    public async Task Logout()
    {
        logger.LogInformation("METHOD: Logout");

        await client.Auth.SignOut();
        await localStorage.RemoveItemAsync("token");
        await customAuthStateProvider.GetAuthenticationStateAsync();
    }

}
