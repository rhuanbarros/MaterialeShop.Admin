using Blazored.LocalStorage;
using MaterialeShop.Admin.Src.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using Postgrest.Models;
using Supabase.Gotrue;
using Supabase.Interfaces;
using Supabase.Realtime;
using Supabase.Storage;

namespace MaterialeShop.Admin.Src.Providers;

public class DatabaseProvider
{
    public Supabase.Client client {get;}
    private readonly AuthenticationStateProvider customAuthStateProvider;
    private readonly ILocalStorageService localStorage;
    private readonly ILogger<DatabaseProvider> logger;

    public DatabaseProvider(
        Supabase.Client client,
        AuthenticationStateProvider CustomAuthStateProvider, 
        ILocalStorageService localStorage,
        ILogger<DatabaseProvider> logger
    ) : base()
    {
        logger.LogInformation("------------------- CONSTRUCTOR -------------------");

        this.client = client;
        this.customAuthStateProvider = CustomAuthStateProvider;
        this.localStorage = localStorage;
        this.logger = logger;

        client.InitializeAsync();
        client.Auth.RetrieveSessionAsync();
    }

}
