using EasyWarehouseManagementSystem.Core.Interfaces;
using EasyWarehouseManagementSystem.Core.Models;
using EasyWarehouseManagementSystem.Core.Repositories;

namespace EasyWarehouseManagementSystem.ConsoleApp;

public class ProductMenu : Menu
{
    private static readonly string[] Options = ["Se alle produkter", "Søg efter produkt", "Skift produktstatus", "Opret produkt", "Slet produkt"];
    private IGenericRepo<Product> _productRepo;
    private IGenericRepo<Stock> _stockRepo;
    private CategoryRepo _categoryRepo;

    // Table widths
    private static int colNumber = 15;
    private static int colName = 25;
    private static int colPrice = 10;
    private static int colPopularity = 16;
    private static int totalWidth = colNumber + colName + colPrice + colPopularity;
    private static string divider = $"{DimCyan}├{new string('─', colNumber)}┼{new string('─', colName)}┼{new string('─', colPrice)}┼{new string('─', colPopularity)}┤{Reset}";

    public ProductMenu(IGenericRepo<Product> productRepo, IGenericRepo<Stock> stockRepo, CategoryRepo categoryRepo)
    {
        _productRepo = productRepo;
        _stockRepo = stockRepo;
        _categoryRepo = categoryRepo;
    }

    public override void ShowMenu()
    {
        bool running = true;
        while (running)
        {
            ShowHeader("Produkter");
            int choice = ShowInteractiveMenu(Options);

            switch (choice)
            {
                case -1:
                    running = false;
                    break;
                case 1:
                    ShowAllProducts();
                    break;
                case 2:
                    SearchProduct();
                    break;
                case 3:
                    ToggleProductStatus();
                    break;
                case 4:
                    CreateProduct();
                    break;
                case 5:
                    DeleteProduct();
                    break;
            }
        }
    }

    // Builds a simple header with title
    private void BuildHeader(string title)
    {
        Console.Clear();
        int dashCount = totalWidth - title.Length - 4;
        Console.WriteLine($"{DimCyan}┌──{Cyan}{title}{DimCyan}{new string('─', dashCount)}┐{Reset}");
        Console.WriteLine($"{DimCyan}└{new string('─', totalWidth - 2)}┘{Reset}");
    }

    // Builds a product table with column headers
    private void BuildProductTable(string title)
    {
        Console.Clear();
        string headerLine = $"{Cyan}{title}{Reset}{DimCyan}" + new string('─', totalWidth - title.Length - 2);
        Console.WriteLine($"{DimCyan}┌──{Reset}{headerLine}┐{Reset}");
        Console.WriteLine(
            $"{DimCyan}│{Reset}{Bold}{Gray}{"Varenr.".PadRight(colNumber)}{DimCyan}│{Reset}" +
            $"{Bold}{Gray}{"Varenavn".PadRight(colName)}{DimCyan}│{Reset}" +
            $"{Bold}{Gray}{"Pris".PadRight(colPrice)}{DimCyan}│{Reset}" +
            $"{Bold}{Gray}{"Popularitet".PadRight(colPopularity)}{Reset}{DimCyan}│{Reset}"
        );
        Console.WriteLine(divider);
    }

    // Prints a product row in the table
    private void PrintProductRow(Product p)
    {
        string number = p.ProductNumber.PadRight(colNumber);
        string name = p.Name.Length > colName ? p.Name[..(colName - 1)] + "…" : p.Name.PadRight(colName);
        string price = $"{p.Price:F2} kr.".PadRight(colPrice);
        string popularity = (p.Popularity switch
        {
            Popularity.NotPopular => "Ikke populær",
            Popularity.Popular => "Populær",
            Popularity.VeryPopular => "Meget populær",
            _ => "Ukendt"
        }).PadRight(colPopularity);

        Console.WriteLine(
            $"{DimCyan}│{Reset}{White}{number}{DimCyan}│{Reset}" +
            $"{White}{name}{DimCyan}│{Reset}" +
            $"{White}{price}{DimCyan}│{Reset}" +
            $"{White}{popularity}{DimCyan}│{Reset}"
        );
    }

    // Closes the table and waits for Escape input
    private void BuildProductTableFooter()
    {
        Console.WriteLine($"{DimCyan}└{new string('─', colNumber)}┴{new string('─', colName)}┴{new string('─', colPrice)}┴{new string('─', colPopularity)}┘{Reset}");
        Console.WriteLine($"\n{Gray}  Tryk Esc for at vende tilbage...{Reset}");

        while (true)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                break;
        }
    }

    // Shows all products grouped by category
    private void ShowAllProducts()
    {
        Console.Clear();
        IEnumerable<Product> products = _productRepo.GetAll();

        BuildHeader("Se alle produkter");

        Console.WriteLine($"\n  {Bold}{Gray}{"Varenr.".PadRight(15)}{"Varenavn".PadRight(25)}{"Pris".PadRight(15)}Popularitet{Reset}");
        Console.WriteLine($"  {new string('─', totalWidth - 4)}");

        if (!products.Any())
        {
            Console.WriteLine("  Ingen produkter fundet.");
            Console.ReadKey();
            return;
        }

        var grouped = products.GroupBy(p => p.Category.Name);
        foreach (var group in grouped)
        {
            Console.WriteLine($"\n  {Cyan}── {group.Key}{Reset}");
            Console.WriteLine();
            foreach (var p in group)
            {
                string popularity = p.Popularity switch
                {
                    Popularity.NotPopular => "Ikke populær",
                    Popularity.Popular => "Populær",
                    Popularity.VeryPopular => "Meget populær",
                    _ => "Ukendt"
                };
                Console.WriteLine($"  {White}{p.ProductNumber.PadRight(15)}{Gray}{p.Name.PadRight(25)}{Reset}{$"{p.Price:F2} kr.".PadRight(15)}{popularity}");
            }
        }

        Console.WriteLine($"\n{Gray}  Tryk Esc for at vende tilbage...{Reset}");
        while (true)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                break;
        }
    }

    // Searches for products by name or product number
    private void SearchProduct()
    {
        BuildHeader("Søg efter produkt");
        Console.Write("   Indtast søgeord: ");
        string term = Console.ReadLine() ?? "";

        IEnumerable<Product> results = _productRepo.Search(term);

        BuildProductTable("Søgeresultater");

        if (!results.Any())
        {
            Console.WriteLine($"{DimCyan}│{Reset}{White}{"Ingen produkter fundet.".PadRight(totalWidth)}{DimCyan}│{Reset}");
        }
        else
        {
            foreach (var p in results)
            {
                PrintProductRow(p);
            }
        }

        BuildProductTableFooter();
    }

    // Creates a new product
    private void CreateProduct()
    {
        ShowHeader("Opret produkt");
        Console.Write("Produktnavn: ");
        string name = Console.ReadLine() ?? "";
        while (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Produktnavn må ikke være tomt. Prøv igen: ");
            name = Console.ReadLine() ?? "";
        }

        Console.Write("Produktnummer: ");
        string productNumber = Console.ReadLine() ?? "";
        while (string.IsNullOrWhiteSpace(productNumber))
        {
            Console.WriteLine("Produktnummer må ikke være tomt. Prøv igen: ");
            productNumber = Console.ReadLine() ?? "";
        }

        Console.Write("Pris: ");
        if (!double.TryParse(Console.ReadLine(), out double price))
        {
            Console.WriteLine("Ugyldig pris.");
            Console.ReadKey();
            return;
        }

        ShowHeader("Opret produkt - Vælg popularitet");
        int popularityChoice = ShowInteractiveMenu(["Ikke populær", "Populær", "Meget populær"]);
        if (popularityChoice == -1) return;
        Popularity popularity = (Popularity)(popularityChoice - 1);

        ShowHeader("Opret produkt - Vælg kategori");
        List<Category> categories = _categoryRepo.GetCategories().ToList();
        string[] categoryOptions = categories.Select(c => c.Name).ToArray();
        int categoryChoice = ShowInteractiveMenu(categoryOptions);
        if (categoryChoice == -1) return;
        Category selectedCategory = categories[categoryChoice - 1];

        Product product = new Product(name, 0, price, popularity, selectedCategory, productNumber);
        _productRepo.Add(product);

        Stock stock = new Stock(0, product, 0);
        _stockRepo.Add(stock);

        ShowHeader("Opret produkt");
        Console.Write("Antal på lager: ");
        if (int.TryParse(Console.ReadLine(), out int amount) && amount >= 0)
        {
            stock.EditStockAmount(amount);
            _stockRepo.Update(stock);
        }

        Console.WriteLine($"\n{Green}✓ Produktet '{name}' er oprettet.{Reset}");
        Console.ReadKey();
    }

    // Toggles a product's active status
    private void ToggleProductStatus()
    {
        ShowHeader("Skift produktstatus");
        List<Stock> stocks = _stockRepo.GetAll().ToList();

        if (!stocks.Any())
        {
            Console.WriteLine($"{Gray}  Ingen produkter fundet.{Reset}");
            Console.ReadKey();
            return;
        }

        string[] stockOptions = stocks.Select(s => $"{s.Product.Name} ({(s.IsActive ? "Aktiv" : "Inaktiv")})").ToArray();
        int choice = ShowInteractiveMenu(stockOptions);
        if (choice == -1) return;
        Stock selectedStock = stocks[choice - 1];

        selectedStock.ToggleStockActivity();
        _stockRepo.Update(selectedStock);

        ShowHeader("Skift produktstatus");
        Console.WriteLine($"\n{Green}✓ '{selectedStock.Product.Name}' er nu {(selectedStock.IsActive ? "aktiv" : "inaktiv")}.{Reset}");
        Console.ReadKey();
    }

    // Deletes a product and its stock
    private void DeleteProduct()
    {
        ShowHeader("Slet produkt");
        List<Product> products = _productRepo.GetAll().ToList();

        if (!products.Any())
        {
            Console.WriteLine($"{Gray}  Ingen produkter fundet.{Reset}");
            Console.ReadKey();
            return;
        }

        string[] productOptions = products.Select(p => p.Name).ToArray();
        int choice = ShowInteractiveMenu(productOptions);
        if (choice == -1) return;
        Product selectedProduct = products[choice - 1];

        // Ask for confirmation before deleting
        ShowHeader("Slet produkt - Bekræft");
        int confirm = ShowInteractiveMenu([$"Ja, slet '{selectedProduct.Name}'", "Nej, gå tilbage"]);
        if (confirm == -1 || confirm == 2) return;

        _productRepo.Delete(selectedProduct.Id);
        _stockRepo.Delete(selectedProduct.Id);

        ShowHeader("Slet produkt");
        Console.WriteLine($"\n{Green}✓ '{selectedProduct.Name}' er slettet.{Reset}");
        Console.ReadKey();
    }
}