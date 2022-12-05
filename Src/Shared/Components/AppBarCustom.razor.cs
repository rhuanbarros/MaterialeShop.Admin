namespace MaterialeShop.Admin.Src.Shared.Components;

public partial class AppBarCustom
{
    async Task DrawerToggleAsync()
    {
        AppGlobals.DrawerOpen = !AppGlobals.DrawerOpen;
        await InvokeAsync(StateHasChanged);
    }

    private async Task OnClickLogout()
    {
        await AuthService.Logout();
        Snackbar.Add("Logout com sucesso.");
        NavigationManager.NavigateTo($"/");
    }
}

