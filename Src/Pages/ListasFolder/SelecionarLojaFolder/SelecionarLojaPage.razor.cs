using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MaterialeShop.Admin.Src.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.ListasFolder.SelecionarLojaFolder;

public partial class SelecionarLojaPage
{
    [Parameter]
    public int ListaId { get; set; }

    private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Home", href: "#", icon: Icons.Material.Filled.Home),
        //TODO: fazer essa lista ser populada na inicialização do componente
        // new BreadcrumbItem("Lista", href: Rotas.Lista(ListaId), icon: Icons.Material.Filled.List),
        new BreadcrumbItem("Lista", href: null, icon: Icons.Material.Filled.List),
        new BreadcrumbItem("Selecionar loja", href: null, icon: Icons.Material.Filled.Shop),
    };

}