using EasyWarehouseManagementSystem.Core.Interfaces;
using EasyWarehouseManagementSystem.Core.Models;
using EasyWarehouseManagementSystem.Core.Repositories;

namespace EasyWarehouseManagementSystem.ConsoleApp;

public class StockMenu : Menu
{
    private static int _lowStockNotifications;
    private IGenericRepo<Product> _productRepo;
    private IGenericRepo<Stock> _stockRepo;
    private CategoryRepo _categoryRepo;

    // Opsætter tabel bredder og divider
    private static int colId = 15;
    private static int colName = 28;
    private static int colQty = 9;
    private static int colMin = 18;
    private static int totalWidth = colId + colName + colQty + colMin + 2;
    private static string divider = $"{DimCyan}├{new string('─', colId)}┼{new string('─', colName)}┼{new string('─', colQty)}┼{new string('─', colMin)}┤{Reset}";

    public StockMenu(int lowStock, IGenericRepo<Product> productRepo, IGenericRepo<Stock> stockRepo, CategoryRepo categoryRepo)
    {
        _lowStockNotifications = lowStock;
        _productRepo = productRepo;
        _stockRepo = stockRepo;
        _categoryRepo = categoryRepo;
    }

    public override void ShowMenu()
    {
        ShowHeader("Lagerbeholdning");
        // The menu text
        string[] Options = ["Søg produkt", $"{_lowStockNotifications} produkter under minimumsbeholdning", "Udskriv lagerliste"];
        int choice = ShowInteractiveMenu(Options);

        switch (choice)
        {
            case -1:
                return;
            case 1:
                SearchProduct();
                break;
            case 2:
                ShowLowStock();
                break;
            case 3:
                Program.ProductMenu.ShowMenu();
                break;
            case 4:
                Program.SupplierMenu.ShowMenu();
                break;
        }
    }

    // Header Builder: prints a simple header with the given title
    private void BuildHeader(string title)
    {
        Console.Clear();

        // Udskriver simple header
        string headerLine = $"{Cyan}{title}{Reset}{DimCyan}" + new string('─', totalWidth - $"{title}".Length - 4);
        Console.WriteLine($"{DimCyan}┌──{Reset}{headerLine}┐{Reset}");
        Console.WriteLine($"{DimCyan}└{new string('─', totalWidth - 2)}┘{Reset}");
    }

    // TableBuilder: with dynamic title and column headers
    private void BuildStockTable(string title)
    {
        Console.Clear();

        // Udskriver tabel header
        string headerLine = $"{Cyan}{title}{Reset}{DimCyan}" + new string('─', 75 - $"{title}".Length - 4);
        Console.WriteLine($"{DimCyan}┌──{Reset}{headerLine}┐{Reset}");
        Console.WriteLine($"{DimCyan}│{Reset}{Bold}{Gray} {new string("Varenr.").PadRight(colId - 1)}{DimCyan}│{Reset}{Bold}{Gray} {new string("Varenavn").PadRight(colName - 1)}{DimCyan}│{Reset}{Bold}{Gray} {new string("Antal").PadRight(colQty - 1)}{DimCyan}│{Reset}{Bold}{Gray} {new string("Min. beholdning").PadRight(colMin - 1)}{Reset}{DimCyan}│{Reset}");
        Console.WriteLine(divider);
    }

    // Searches for a product by name or product number and displays its stock information, including current quantity and minimum stock level, with color coding for low stock
    private void SearchProduct()
    {
        Console.Clear();
        BuildHeader("Søg produkt");

        Console.Write("   Indtast søgeord: ");
        string term = Console.ReadLine() ?? "";

        IEnumerable<Product> results = _productRepo.Search(term);
        // var stock = _stockRepo.Get(product.Id);

        BuildStockTable("Søgeresultater");
        if (!results.Any())
        {
            string msg = "Ingen produkter fundet";
            Console.WriteLine($"{DimCyan}│{Reset}{White} {msg.PadRight(totalWidth)}{DimCyan}│{Reset}");
        }
        //else if (stock == null)
        //{
        //    string msg = "Ingen lagerinformation tilgængelig for dette produkt.";
        //    Console.WriteLine($"{DimCyan}│{Reset}{White} {msg.PadRight(totalWidth)}{DimCyan}│{Reset}");
        //}
        //else {
        //Console.WriteLine($"Produkt: {product.Name}");
        //Console.WriteLine($"Varenr.: {product.ProductNumber}");
        //Console.WriteLine($"Antal på lager: {stock.Amount}");
        //Console.WriteLine($"Minimumsbeholdning: {stock.Product.Category.MinStockAmount}");
        //if (stock.IsBelowMinStock())
        //{
        //    Console.WriteLine($"{Red}Advarsel: Produktet er under minimumsbeholdning!{Reset}");
        //}
        //    BuildStockTableFooter();
        //    return;

    }

    // Shows a list of products that are under their category's minimum stock level, with color coding for critical levels and a clear table format
    private void ShowLowStock()
    {
        var lowStockItems = _stockRepo.GetAll().Where(stock => stock.IsBelowMinStock()).ToList(); // Henter alle lagerbeholdninger og filtrerer dem, der er under minimumsbeholdning

        BuildStockTable("Produkter under minimumsbeholdning");

        if (!lowStockItems.Any())   // Tjekker om der er nogen produkter under minimumsbeholdning, og viser en besked hvis ikke
        {
            string msg = "Ingen produkter under minimumsbeholdning";
            Console.WriteLine($"{DimCyan}│{Reset}{Green} {msg.PadRight(totalWidth)}{DimCyan}│{Reset}");
        }
        else
        {
            foreach (var stock in lowStockItems)
            {
                var product = _productRepo.Get(stock.Id);
                string id = product.ProductNumber.ToString();
                string name = product.Name.Length > colName - 1         // Tjekker om produktnavnet er længere end kolonnebredden, og forkorter det med "…" hvis det er tilfældet
                               ? product.Name[..(colName - 2)] + "…"
                               : product.Name;
                string qty = stock.Amount.ToString();
                string min = stock.Product.Category.MinStockAmount.ToString();

                // Rød farve hvis kritisk lav (under halvdelen af minimum)
                string rowColor = stock.Amount <= stock.Product.Category.MinStockAmount / 2 ? Red : Yellow;    // Tjekker om lagerbeholdningen er kritisk lav (under halvdelen af minimumsbeholdningen) og sætter farven til rød, ellers gul
                Console.WriteLine(
                    $"{DimCyan}│{Reset} {rowColor}{id.PadRight(colId - 1)}{Reset}" +
                    $"{DimCyan}│{Reset} {rowColor}{name.PadRight(colName - 1)}{Reset}" +
                    $"{DimCyan}│{Reset} {rowColor}{qty.PadRight(colQty - 1)}{Reset}" +
                    $"{DimCyan}│{Reset} {rowColor}{min.PadRight(colMin - 1)}{Reset}" +
                    $"{DimCyan}│{Reset}"
                );
            }
        }
        BuildStockTableFooter();
    }
    private void BuildStockTableFooter()
    {
        Console.WriteLine($"{DimCyan}└{new string('─', colId)}┴{new string('─', colName)}┴{new string('─', colQty)}┴{new string('─', colMin)}┘{Reset}");
        Console.WriteLine($"\n{Gray}  Tryk Esc for at vende tilbage...{Reset}");

        while (true)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                ShowMenu();
                break;
            }
        }
    }
}
