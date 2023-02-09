using Microsoft.AspNetCore.Components;

namespace MaterialeShop.Admin.Src.Shared.Components;

public partial class MudButtonWithLoading
{
    [Parameter]
    public Func<Task> onClickEventHandler {get;set;}

    [Parameter]
    public string titulo {get;set;} = "Enviar";

    protected bool _processingNewItem = false;

    protected async Task onClickEventHandlerProcessingAsync()
    {
        _processingNewItem = true;
        await InvokeAsync(StateHasChanged);

        await onClickEventHandler();

        _processingNewItem = false;
        await InvokeAsync(StateHasChanged);
    }
}
