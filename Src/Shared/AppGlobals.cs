
using System.Runtime.Serialization;
namespace MaterialeShop.Admin.Src.Shared;

// https://stackoverflow.com/questions/71713761/how-can-i-declare-a-global-variables-model-in-blazor
public class AppGlobals
{
    public static string AppName = "MaterialeShop";
    protected bool drawerOpen = true;
    public bool DrawerOpen
    {
        get
        {
            return drawerOpen;
        }
        set
        {
            OnDrawerOpened(EventArgs.Empty, value);
        }
    }

    public event EventHandler? drawerOpenChangedEvent;
    protected virtual void OnDrawerOpened(EventArgs e, bool open)
    {
        drawerOpen = open;
        drawerOpenChangedEvent?.Invoke(this, e);
    }

}