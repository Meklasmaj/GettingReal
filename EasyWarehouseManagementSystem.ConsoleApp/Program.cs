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
    public static Menu MainMenu = new MainMenu();
    public static Menu ProductMenu = new ProductMenu(productRepo, stockRepo, new CategoryRepo());
    public static Menu StockMenu = new StockMenu(CheckLowStockNotifications(), productRepo, stockRepo, new CategoryRepo());
    public static Menu SupplierMenu = new SupplierMenu(supplierRepo);
    public static Menu DraftOrderMenu = new DraftOrderMenu(draftOrderRepo, stockRepo);
    
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
        List<DraftOrder> draftOrders = draftOrderRepo.GetAll().ToList();
        List<Supplier> suppliers = supplierRepo.GetAll().ToList();
        List<Stock> stocks = stockRepo.GetAll().ToList();
        
        // Make Dictionary from brands and suppliers, Key: Brand, Value: Supplier
        Dictionary<string, Supplier> brandToSupplier = suppliers.SelectMany(s => s.GetBrands()
                .Select(b => (Brand: b.ToLower(), Supplier: s)))
            .ToDictionary(x => x.Brand, x => x.Supplier);
        
        // Make Dictionary from supplier and draftorder, Key: Supplier, Value: DraftOrder
        Dictionary<int, DraftOrder> openDraftsBySupplier = draftOrders.Where(d => d
            .IsOpen()).ToDictionary(d => d.Supplier.Id);
        
        foreach (var stock in stocks.Where(s => s.IsBelowMinStock()))
        {
            num++;

            string productName = stock.Product.Name.ToLower();
            
            // Find matching supplier using the dictionary
            Supplier matchedSupplier = brandToSupplier
                .FirstOrDefault(p => productName.Contains(p.Key)).Value;

            // If non matches, skip
            if (matchedSupplier == null)
            {
                continue;
            }

            // Find matching DraftOrder by Supplier and add new orderline
            if (openDraftsBySupplier.TryGetValue(matchedSupplier.Id, out var existingDraft))
            {
                bool isAlreadyOnDraft = existingDraft.OrderLines.Any(o => o.Stock.Product.Name == stock.Product.Name);
                if (!isAlreadyOnDraft)
                {
                    existingDraft.OrderLines.Add(new OrderLine(stock));
                    draftOrderRepo.Update(existingDraft);
                }
            }
            // Create new DraftOrder and add orderline
            else
            {
                DraftOrder newDraft = new DraftOrder
                (
                    0,
                    OrderStatus.Open,
                    new List<OrderLine>() { new OrderLine(stock) },
                    matchedSupplier,
                    DateTime.Now
                );
                draftOrderRepo.Add(newDraft);
                openDraftsBySupplier[matchedSupplier.Id] = newDraft;
            }
        }
        return num;
    }
}