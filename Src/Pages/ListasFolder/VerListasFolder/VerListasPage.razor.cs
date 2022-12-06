using MaterialeShop.Admin.Src.Dtos;
using MaterialeShop.Admin.Src.Services;
using MudBlazor;

namespace MaterialeShop.Admin.Src.Pages.ListasFolder.VerListasFolder;

public partial class VerListasPage
{    
    // ---------------- SEARCH
    private void OnValueChangedSearch(string text)
    {
        _tableListFiltered = _tableList?.Where(row => row.Endereco.Contains(text)).ToList();
    }

    // ---------------- CREATE NEW
    // protected async Task OnClickSave()
    // {
    //     _processingNewItem = true;
    //     if(ModoEdicao == false)
    //     {
    //         await CrudService.Insert<Lista>(model);
    //     } else 
    //     {
    //         await CrudService.Edit<Lista>(model);            
    //         ModoEdicao = false;
    //     }

    //     model = new();
    //     await GetTable();
    //     success = false;
    //     _processingNewItem = false;
    // }

    // ---------------- DELETE
    protected new async Task OnClickDelete(ListasView item)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Atenção",
            "Você deseja apagar este item?", 
            yesText:"Sim", cancelText:"Não");
        
        if(result == true)
        {
            Lista newItem = new()
            {
                Id = item.Id
            };
            await CrudService.Delete<Lista>(newItem);
        }
        await GetTable();
    }
    
}