using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Shared;
public class BasePageComponent : ComponentBase
{
    [Inject]
    protected NavigationManager _navManager { get; set; }
    
    [Inject]
    NavigationManager NavigationManager { get; set; }

    [Inject]
    ISnackbar Snackbar { get; set; }

    [Inject]
    protected PageHistoryStateService _pageHistoryStateService { get; set; }

    public BasePageComponent(NavigationManager navManager, PageHistoryStateService pageHistoryStateService)
    {
        _navManager = navManager;
        _pageHistoryStateService = pageHistoryStateService;
    }
    public BasePageComponent()
    {
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Console.WriteLine("_navManager.Uri");
        Console.WriteLine(_navManager.Uri);
        _pageHistoryStateService.AddPageToHistory(_navManager.Uri);
    }

}