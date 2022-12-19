using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.OrcamentosFolder.OrcamentoItensFolder;

public partial class OrcamentoItensPage
{    
    [Parameter]
    public int ListaId { get; set; }
    
    [Parameter]
    public int OrcamentoId { get; set; }
    
}