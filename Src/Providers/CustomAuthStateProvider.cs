using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Supabase;

// Credits https://github.com/patrickgod/BlazorAuthenticationTutorial

namespace MaterialeShop.Admin.Src.Providers;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService localStorage;
    private readonly Client client;
    private readonly ILogger<CustomAuthStateProvider> logger;

    public CustomAuthStateProvider(
        ILocalStorageService localStorage,
        Supabase.Client client,
        ILogger<CustomAuthStateProvider> logger
    )
    {
        logger.LogInformation("------------------- CONSTRUCTOR -------------------");

        this.localStorage = localStorage;
        this.client = client;
        this.logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        logger.LogInformation("------------------- GetAuthenticationStateAsync() -------------------");

        try {
            // Sets client auth and connects to realtime (if enabled)
            await client.InitializeAsync();
        } catch {
            logger.LogInformation("Houve uma falha na conexão a um dos serviços do Supabase. Verifique a sua conexão de rede. Também é possível que o servidor esteja fora do ar.");
        }
        
        var identity = new ClaimsIdentity();

        if (!string.IsNullOrEmpty(client.Auth.CurrentSession?.AccessToken))
        {
            identity = new ClaimsIdentity(ParseClaimsFromJwt(client.Auth.CurrentSession.AccessToken), "jwt");
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
