using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.NovoOrcamentoFolder;

public partial class NovoOrcamentoPage
{    
    [Parameter]
    public int ListaId { get; set; }
    
    [Parameter]
    public int OrcamentoId { get; set; }
    
}