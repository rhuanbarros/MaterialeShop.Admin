using Postgrest.Models;

namespace MaterialeShop.Admin.Src.Shared;

public class BaseCrudViewPageComponent<TCrudModel, TViewModel> : BaseCrudPageComponent<TViewModel> where TCrudModel : BaseModel, new() where TViewModel : BaseModel, new()
{
    // ---------------- CREATE NEW
    protected new TCrudModel model = new();
    protected override async Task OnClickSave()
    {
        form?.Validate();

        if (form.IsValid)
        {
            _processingNewItem = true;
            if (ModoEdicao == false)
            {
                await CrudService.Insert<TCrudModel>(model);
            }
            else
            {
                await CrudService.Edit<TCrudModel>(model);
                ModoEdicao = false;
            }

            model = new();
            await GetTable();
            success = false;
            _processingNewItem = false;
        }
    }

    // ---------------- DELETE
    protected virtual TCrudModel SetModelIdToDelete(TViewModel item)
    {
        throw new NotImplementedException();
    }

    protected override async Task OnClickDelete(TViewModel item)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Atenção",
            "Você deseja apagar este item?",
            yesText: "Sim", cancelText: "Não");

        if (result == true)
        {
            TCrudModel newItem = SetModelIdToDelete(item);
            await CrudService.Delete<TCrudModel>(newItem);
        }
        await GetTable();
    }


}