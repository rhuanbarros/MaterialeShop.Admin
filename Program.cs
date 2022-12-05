using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MaterialeShop.Admin;
using Microsoft.AspNetCore.Components.Authorization;
using MaterialeShop.Admin.Src.Providers;
using MaterialeShop.Admin.Src.Services;
using Blazored.LocalStorage;
using MudBlazor.Services;
using Supabase.Interfaces;
using Supabase.Gotrue;
using Supabase.Realtime;
using Supabase.Storage;
using MaterialeShop.Admin.Src;
using MaterialeShop.Admin.Src.Shared;
using MaterialeShop.Admin.Src.Dtos;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

// ---------- SUPABASE
var url = "https://ybqilfcwesgbkvxmgxpm.supabase.co";
var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InlicWlsZmN3ZXNnYmt2eG1neHBtIiwicm9sZSI6ImFub24iLCJpYXQiOjE2NjgyOTI4NDcsImV4cCI6MTk4Mzg2ODg0N30.HjO-uxNoe99XnviTPTtZ5kDFKG3z4T5CKuW8xZn-Ra0";

builder.Services.AddScoped<Supabase.Client>(
    provider => new Supabase.Client(
        url,
        key,
        new Supabase.SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true,
            PersistSession = true,
            SessionHandler = new CustomSupabaseSessionHandler(
                provider.GetRequiredService<ILocalStorageService>(),
                provider.GetRequiredService<ILogger<CustomSupabaseSessionHandler>>()
            )
        }
    )
);

// ---------- BLAZOR AUTH
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>(
	provider => new CustomAuthStateProvider(
		provider.GetRequiredService<ILocalStorageService>(),
		provider.GetRequiredService<Supabase.Client>(),
		provider.GetRequiredService<ILogger<CustomAuthStateProvider>>()
	)
);
builder.Services.AddAuthorizationCore();


builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DatabaseProvider>();

// ---------- OTHER SERVICES
builder.Services.AddSingleton<PageHistoryStateService>();

//https://stackoverflow.com/questions/71713761/how-can-i-declare-a-global-variables-model-in-blazor
builder.Services.AddSingleton<AppGlobals>();

// builder.Services.AddScoped<MyCustomTheme>(); //nao funcionou nao sei pq

// ---------- TABLE SERVICES
builder.Services.AddScoped<CrudService>();
builder.Services.AddScoped<UsuarioPerfilService>();
builder.Services.AddScoped<ListasService>();



await builder.Build().RunAsync();