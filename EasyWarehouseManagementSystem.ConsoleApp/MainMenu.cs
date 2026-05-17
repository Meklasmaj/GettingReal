namespace EasyWarehouseManagementSystem.ConsoleApp;

public class MainMenu : Menu
{
    public override void ShowMenu()
    {
        ShowHeader("Hovedmenu");
        // The menu text
        string[] Options = [$"Kladdeordrer", "Lager", "Produkter", "Leverandører"];
        int choice = ShowInteractiveMenu(Options);

        switch (choice)
        {
            case -1:
                Environment.Exit(0);
                break;
            case 1:
                Program.DraftOrderMenu.ShowMenu();
                break;
            case 2:
                Program.StockMenu.ShowMenu();
                break;
            case 3:
                Program.ProductMenu.ShowMenu();
                break;
            case 4:
                Program.SupplierMenu.ShowMenu();
                break;
        }
    }
}
