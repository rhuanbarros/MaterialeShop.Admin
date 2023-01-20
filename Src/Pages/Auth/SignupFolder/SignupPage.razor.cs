using MaterialeShop.Admin.Src.Shared;

namespace MaterialeShop.Admin.Src.Pages.Auth.SignupFolder;

public partial class SignupPage //: BasePageComponent
{
    protected string? email {get; set;} = "rhuanbarros@gmail.com";
    protected string? password {get; set;} = "123456789";
    public string? NomeCompleto { get; set; } = "Rhuan Paulo  Lopes Barros";
    
    public async Task OnClickSave()
    {
        await AuthService.SignupByEmail(email, password, NomeCompleto);

        Snackbar.Add("Cadastro realizado com sucesso. Faça login para entrar no sistema.");
        NavigationManager.NavigateTo($"/login");
    }
}

