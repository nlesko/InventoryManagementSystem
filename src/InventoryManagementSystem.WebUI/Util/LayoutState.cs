namespace InventoryManagementSystem.WebUI.Util;

public class LayoutState : ILayoutState
{
    public event Action OnChange;
    public bool IsSidebarOpen { get; set; }
    public bool IsDarkMode { get; set; }

    public void ToggleSidebar()
    {
        IsSidebarOpen = !IsSidebarOpen;
        NotifyStateChanged();
    }

    public void CloseSidebar()
    {
        IsSidebarOpen = false;
        NotifyStateChanged();
    }

    public void ToggleDarkMode(bool? isDarkMode = null)
    {
        if (isDarkMode.HasValue)
        {
            IsDarkMode = isDarkMode.Value;
        }
        else
        {
            IsDarkMode = !IsDarkMode;
        }
        NotifyStateChanged();
    }

    public void OpenSidebar()
    {
        IsSidebarOpen = true;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}