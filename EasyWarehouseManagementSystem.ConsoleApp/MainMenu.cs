namespace EasyWarehouseManagementSystem.ConsoleApp;

public class MainMenu : Menu
{
    private static int _notifications = 0;
    public MainMenu(int notifications)
    {
        _notifications = notifications;
    }
    // The menu text
    private static readonly string[] Options = [$"Kladdeordrer : {_notifications.ToString()}", "Lager", "Produkter", "Leverandører"];

    public override void ShowMenu()
    {
        Console.Clear();
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
