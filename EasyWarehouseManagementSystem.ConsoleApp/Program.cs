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
    public static Menu MainMenu = new MainMenu(CheckDraftOrderNotifications());
    // Needs new methods
    public static Menu ProductMenu = new ProductMenu(productRepo, stockRepo, new CategoryRepo());
    public static Menu StockMenu = new StockMenu(CheckLowStockNotifications(), productRepo, stockRepo, new CategoryRepo());
    public static Menu SupplierMenu = new SupplierMenu(supplierRepo);
    public static Menu DraftOrderMenu = new MainMenu(CheckDraftOrderNotifications());
    
    static void Main(string[] args)
    {
        // To show ÆØÅ in the console, and to store ÆØÅ inputs.
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        while (true)
        {
            MainMenu.ShowMenu();
        }
    }

    public static int CheckDraftOrderNotifications()
    {
        int num = 0;
        foreach (var draftOrder in draftOrderRepo.GetAll())
        {
            if (draftOrder.IsOpen())
            {
                num++;
            }
        }
        return num;
    }

    public static int CheckLowStockNotifications()
    {
        int num = 0;
        IEnumerable<DraftOrder> draftOrders = draftOrderRepo.GetAll();
        draftOrders.Reverse();
        foreach (var stock in stockRepo.GetAll())
        {
            if (stock.IsBelowMinStock())
            {
                num++;

                foreach (var supplier in supplierRepo.GetAll())
                {
                    foreach (var brand in supplier.GetBrands())
                    {
                        if (stock.Product.Name.ToLower().Contains(brand.ToLower()))
                        {
                            if (draftOrders.Any(f => f.IsOpen()) &&
                                draftOrders.Any(f => f.Supplier == supplier))
                            {
                                DraftOrder draft =
                                    draftOrders.FirstOrDefault(f => f.Supplier == supplier && f.IsOpen());
                                draft.OrderLines.Add(new OrderLine(stock));
                                draftOrderRepo.Update(draft);
                            }
                            else
                            {
                                List<OrderLine> orderLines = new List<OrderLine>() { new OrderLine(stock) };
                                DraftOrder draft = new DraftOrder(0, OrderStatus.Open, orderLines, supplier, DateTime.Now);
                                draftOrderRepo.Add(draft);
                            }
                        }
                    }
                }
                
                // For hver supplier, get brands
                // For hvert brand, hvis brand contained in product name
                // Hvis draftorder med supplier ikke ekisisterer
                // Add DraftOrder with supplier
                // Ellers tilføj product til draftorder via ordrelinje
            }
        }
        return num;
    }
}