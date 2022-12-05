using MudBlazor;

namespace MaterialeShop.Admin.Src.Shared;

public partial class MainLayout
{
    private async Task OnClickLogout()
    {
        await AuthService.Logout();
        Snackbar.Add("Logout successfull");
        NavigationManager.NavigateTo($"/");
    }

    protected override void OnInitialized()
    {
        AppGlobals.drawerOpenChangedEvent += atualizaTela;
        logoGrandeVisible = AppGlobals.DrawerOpen ? "d-block" : "d-none";
        logoPequenoVisible = AppGlobals.DrawerOpen ? "d-none" : "d-block";
    }

    protected override async Task OnParametersSetAsync()
    {
        // await AuthService.LoginWithToken();
    }

    private string logoPequenoVisible;
    private string logoGrandeVisible;
    public void atualizaTela(object sender, EventArgs e)
    {
        logoPequenoVisible = AppGlobals.DrawerOpen ? "d-none" : "d-block";
        logoGrandeVisible = AppGlobals.DrawerOpen ? "d-block" : "d-none";
        StateHasChanged();
    }

    // @(AppGlobals.DrawerOpen ? 'visible':'invisible')
    
    // MyCustomTheme MyCustomTheme = new();

    MudTheme MyCustomTheme = new()
    {
        Palette = new()
        {
            Primary = "#FFBD4A",
            PrimaryContrastText = Colors.Shades.Black,
            Secondary = Colors.Shades.Black,
            SecondaryContrastText = Colors.Grey.Lighten5,
            Tertiary = Colors.Grey.Lighten5,
            TertiaryContrastText = Colors.Shades.Black,
            AppbarBackground = "#FFBD4A"
        },

        PaletteDark = new()
        {
            Primary = Colors.Amber.Lighten1
        },

        LayoutProperties = new()
        {
            DrawerWidthLeft = "360px",
            DrawerWidthRight = "360px"
        }

        // Typography = new MaterialeShopTypography()
    };

    MudBlazor.Color colorBtn = Color.Tertiary;

    // class MaterialeShopTypography : Typography
    //     {
    //     BaseTypography Logo = new H6()
    //     {
    //     FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
    //     FontSize = "1.25rem",
    //     FontWeight = 500,
    //     LineHeight = 1.6,
    //     LetterSpacing = ".0075em"
    //     };
    //     }
}