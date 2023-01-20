namespace MaterialeShop.Admin.Src.Pages.Auth.LoginFolder;

public partial class Login
{
    protected string? email {get; set;} = "rhuanbarros@gmail.com";
    protected string? password {get; set;} = "123456789";

    public async Task OnClickLogin()
    {
        await AuthService.Login(email, password);
        Snackbar.Add("Login successfull");
        NavigationManager.NavigateTo($"/");
    }
}

