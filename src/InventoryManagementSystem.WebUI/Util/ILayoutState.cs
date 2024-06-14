namespace InventoryManagementSystem.WebUI.Util;

public interface ILayoutState
{
    public event Action OnChange;
    public bool IsSidebarOpen { get; set; }
    public bool IsDarkMode { get; set; }
    public void ToggleSidebar();
    public void CloseSidebar();
    public void OpenSidebar();
    public void ToggleDarkMode(bool? isDarkMode = null);
}