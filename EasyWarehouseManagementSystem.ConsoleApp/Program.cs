using System.Text;
using EasyWarehouseManagementSystem.Core.Interfaces;
using EasyWarehouseManagementSystem.Core.Models;
using EasyWarehouseManagementSystem.Core.Repositories;

namespace EasyWarehouseManagementSystem.ConsoleApp;

class Program
{
    private static IGenericRepo<Product> productRepo = new JsonRepo<Product>("products.json");
    private static IGenericRepo<Stock> stockRepo = new JsonRepo<Stock>("stocks.json");
    private static IGenericRepo<Supplier> supplierRepo = new JsonRepo<Supplier>("suppliers.json");
    private static IGenericRepo<DraftOrder> draftOrderRepo = new JsonRepo<DraftOrder>("draftOrders.json");
    public static Menu MainMenu = new MainMenu(CheckNotifications());
    // Needs new methods
    public static Menu ProductMenu = new MainMenu(CheckNotifications());
    public static Menu StockMenu = new MainMenu(CheckNotifications());
    public static Menu SupplierMenu = new MainMenu(CheckNotifications());
    public static Menu DraftOrderMenu = new MainMenu(CheckNotifications());
    
    static void Main(string[] args)
    {
        // To show ÆØÅ in the console
        Console.OutputEncoding = Encoding.UTF8;
        
        while(true)
        {
            MainMenu.ShowMenu();
        }
    }

    public static int CheckNotifications()
    {
        int num = 0;
        foreach (var stock in stockRepo.GetAll())
        {
            if (stock.IsBelowMinStock())
            {
                num++;
            }
        }
        return num;
    }
}