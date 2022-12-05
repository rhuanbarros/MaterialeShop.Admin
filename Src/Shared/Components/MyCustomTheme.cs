
using MudBlazor;

namespace MaterialeShop.Admin.Src.Shared;

// esa classe nao funcionou nao sei pq. quem sabe a sobrescrita das propriedades esteja errada.
public partial class MyCustomTheme : MudTheme
{
    public new Palette Palette = new()
    {
        Primary = "#FFBD4A",
        PrimaryContrastText = Colors.Shades.Black,
        Secondary = Colors.Shades.Black,
        SecondaryContrastText = Colors.Grey.Lighten5,
        Tertiary = Colors.Grey.Lighten5,
        TertiaryContrastText = Colors.Shades.Black,
        AppbarBackground = "#FFBD4A"
    };

    public new Palette PaletteDark = new()
    {
        Primary = Colors.Amber.Lighten1
    };

    public LayoutProperties LayoutProperties = new()
    {
        DrawerWidthLeft = "360px",
        DrawerWidthRight = "360px"
    };

    // Typography = new MaterialeShopTypography()
}