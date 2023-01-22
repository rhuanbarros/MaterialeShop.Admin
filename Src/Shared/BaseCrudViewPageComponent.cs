using MaterialeShop.Admin.Src.Dtos;
using Postgrest.Models;

namespace MaterialeShop.Admin.Src.Shared;

public class BaseCrudViewPageComponent<TCrudModel, TViewModel> : BaseCrudPageComponent<TViewModel> where TCrudModel : BaseModelApp, new() where TViewModel : BaseModelApp, new()
{
    // ---------------- CREATE NEW
    protected new virtual TCrudModel model {get;set;} = new();

    protected virtual TCrudModel CreateNewModel()
    {
        model = new()
        {
            CreatedAt = DateTime.Now
        };
        // Console.WriteLine("CreateNewModel");
        // Console.WriteLine(model.CreatedAt);
        return model;
    }

    // usar isso para setar o id da chave estrangeira
    protected virtual TCrudModel SetModelReferenceId(TCrudModel item)
    {
        return item;
    }
    protected override async Task OnClickSave()
    {
        form?.Validate();

        if (form.IsValid)
        {
            _processingNewItem = true;
            if (ModoEdicao == false)
            {
                model = SetModelReferenceId(model);
                await CrudService.Insert<TCrudModel>(model);
            }
            else
            {
                //TODO: qdo vir a nova versao da API, arrumar para ele atualizar apenas os campos que foram alterados
                
                await CrudService.Edit<TCrudModel>(model);
                ModoEdicao = false;
            }

            model = CreateNewModel();
            await GetTable();
            success = false;
            _processingNewItem = false;
            
            Snackbar.Add("Registro salvo com sucesso.");
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
            //TODO: qdo vir a nova versao da API, arrumar para ele atualizar apenas os campos de SoftDelete

            TCrudModel newItem = SetModelIdToDelete(item);
            
            await CrudService.SoftDelete<TCrudModel>(newItem);
            
            Snackbar.Add("Registro apagado com sucesso.");
        }
        await GetTable();

        form?.Reset();
        model = CreateNewModel();
        
        await InvokeAsync(StateHasChanged);
    }

    // ---------------- EDIT MODEL
    protected bool ModoEdicao = false;
    protected virtual TCrudModel SetModelIdToEdit(TViewModel item)
    {
        throw new NotImplementedException();
    }
    protected virtual async Task OnClickEdit(TViewModel item)
    {
        // TODO: verificar se eu arrumei em todos os codigos.
        //essa linha gera um bug q ele edita a instancia e ja aperece na tabela na tela.
        //isso acontece por causa da passagem por referencia.
        // teria q criar um novo
        model = SetModelIdToEdit(item);
        ModoEdicao = true;
    }

}