namespace MaterialeShop.Admin.Src.Services;

//https://stackoverflow.com/questions/62561926/blazor-navigation-manager-go-back/62565035#62565035
public class PageHistoryStateService
{
    private Stack<string> previousPages;

    public PageHistoryStateService()
    {
        previousPages = new Stack<string>();
    }
    public void AddPageToHistory(string pageName)
    {
        Console.WriteLine("AddPageToHistory()");
        
        previousPages.Push(pageName);

        // Console.WriteLine("==========================INICIO=============================");
        // Console.WriteLine("previousPages");
        // foreach (var item in previousPages)
        // {
        //     Console.WriteLine("item -> " + item);
        // }
        // Console.WriteLine("==========================FIM=============================");
    }

    public string GetGoBackPage()
    {
        // Console.WriteLine("GetGoBackPage()");

        if (previousPages.Count > 1)
        {
            previousPages.Pop();
            return previousPages.Pop();
        }
        return "";
    }

    public bool CanGoBack()
    {
        return previousPages.Count > 1;
    }
}