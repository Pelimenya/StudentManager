using System.Windows.Controls;
using MahApps.Metro.Controls;
using StudentManager.Pages.HomePages;

namespace StudentManager.Pages.AdminPages;

public partial class AdminMenu : Page
{
    public AdminMenu()
    {
        InitializeComponent();
    }

    private void HamburgerMenuControl_OnItemClick(object sender, MahApps.Metro.Controls.ItemClickEventArgs args)
    {
        var menuItem = (HamburgerMenuGlyphItem)args.ClickedItem;
        if (menuItem != null && menuItem.Label == "Выход") 
        {
            NavigationService.Navigate(new LoginPage());
        }
        TableMenu.SetCurrentValue(ContentProperty, args.ClickedItem);
        TableMenu.SetCurrentValue(HamburgerMenu.IsPaneOpenProperty, false);
    }
}