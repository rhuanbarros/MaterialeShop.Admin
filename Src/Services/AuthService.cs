using Blazored.LocalStorage;
using MaterialeShop.Admin.Src.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
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
    private readonly IDialogService dialogService;

    // guarda o model UsuarioPerfil do usuario logado 
    public Perfil UsuarioPerfil {get;set;}

    public Perfil? UsuarioLogado { get; private set; }

    public AuthService(
        Supabase.Client client,
        AuthenticationStateProvider CustomAuthStateProvider,
        UsuarioPerfilService UsuarioPerfilService,
        ILocalStorageService localStorage,
        ILogger<AuthService> logger,
        IDialogService DialogService
    ) : base()
    {
        logger.LogInformation("------------------- CONSTRUCTOR -------------------");

        this.client = client;
        this.customAuthStateProvider = CustomAuthStateProvider;
        this.usuarioPerfilService = UsuarioPerfilService;
        this.localStorage = localStorage;
        this.logger = logger;
        dialogService = DialogService;
        client.InitializeAsync();
        client.Auth.RetrieveSessionAsync();
    }

    public async Task SignupByEmail(string Email, string Password, string NomeCompleto)
    {
        Session? session = await client.Auth.SignUp(Email, Password);
        
        //TODO fazer validação se o uuid veio correto

        Perfil perfil = new()
        {
            Email = Email,
            NomeCompleto = NomeCompleto,
            UserUuid = session.User.Id
        };

        await usuarioPerfilService.Insert(perfil);

        await customAuthStateProvider.GetAuthenticationStateAsync();
    }
    public async Task Login(string Email, string Password)
    {
        logger.LogInformation("METHOD: Login");
        
        try
        {
            Session? session = await client.Auth.SignIn(Email, Password);            
        }
        catch (System.Net.Http.HttpRequestException ex)
        {
            await dialogService.ShowMessageBox(
                    "Atenção", 
                    "Houve um erro de conexão de rede. Tente novamente ou contate o suporte."
                );            
        }

        logger.LogInformation("------------------- User logged in -------------------");
        logger.LogInformation($"instance.Auth.CurrentUser.Id {client?.Auth?.CurrentUser?.Id}");
        logger.LogInformation($"client.Auth.CurrentUser.Email {client?.Auth?.CurrentUser?.Email}");

        await customAuthStateProvider.GetAuthenticationStateAsync();

        //guarda o perfi do usuario
        IReadOnlyList<Perfil> perfil = await usuarioPerfilService.GetByUserUuid( client?.Auth?.CurrentUser?.Id );
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
