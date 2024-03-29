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
using MaterialeShop.Admin.Src.Services.StorageServiceFolder;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

// ---------- SUPABASE
var url = "";
var key = "";

builder.Services.AddScoped<Supabase.Client>(
    provider => new Supabase.Client(
        url,
        key,
        new Supabase.SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = false,
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
builder.Services.AddScoped<ListasViewService>();
builder.Services.AddScoped<ListaItensService>();
builder.Services.AddScoped<OrcamentoService>();
builder.Services.AddScoped<OrcamentoViewService>();
builder.Services.AddScoped<OrcamentoItemService>();
builder.Services.AddScoped<CarrinhoService>();
builder.Services.AddScoped<CarrinhoItemService>();
builder.Services.AddScoped<CarrinhoViewService>();
builder.Services.AddScoped<CarrinhoItemViewService>();
builder.Services.AddScoped<CarrinhoGroupByListaViewService>();
builder.Services.AddScoped<OrcamentoItemViewService>();
builder.Services.AddScoped<StorageService>();



await builder.Build().RunAsync();
