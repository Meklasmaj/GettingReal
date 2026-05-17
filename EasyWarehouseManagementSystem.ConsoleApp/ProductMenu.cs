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
    private static int colPrice = 16;
    private static int colPopularity = 16;
    private static int totalWidth = colNumber + colName + colPrice + colPopularity;

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

    // Waits for Escape input
    private void WaitForEscape()
    {
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

        Console.WriteLine($"\n  {Bold}{Gray}{"Varenr.".PadRight(colNumber)}{"Varenavn".PadRight(colName)}{"Pris".PadRight(colPrice)}{"Popularitet".PadRight(colPopularity)}Antal{Reset}");
        Console.WriteLine($"  {new string('─', totalWidth + 6)}");

        if (!products.Any())
        {
            Console.WriteLine($"  {Gray}Ingen produkter fundet.{Reset}");
            WaitForEscape();
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

                Stock? stock = _stockRepo.GetAll().FirstOrDefault(s => s.Product.Id == p.Id);
                string amount = stock != null ? $"{stock.Amount} stk." : "Ukendt";

                Console.WriteLine($"  {White}{p.ProductNumber.PadRight(colNumber)}{Gray}{p.Name.PadRight(colName)}{Reset}{$"{p.Price:F2} kr.".PadRight(colPrice)}{popularity.PadRight(colPopularity)}{amount}");
            }
        }
        WaitForEscape();
    }

    // Searches for products by name or product number
    private void SearchProduct()
    {
        BuildHeader("Søg efter produkt");
        Console.Write($"\n  {Gray}Søgeord: {Reset}");
        string term = Console.ReadLine() ?? "";

        IEnumerable<Product> results = _productRepo.Search(term);

        Console.WriteLine($"\n  {Bold}{Gray}{"Varenr.".PadRight(colNumber)}{"Varenavn".PadRight(colName)}{"Pris".PadRight(colPrice)}{"Popularitet".PadRight(colPopularity)}Antal{Reset}");
        Console.WriteLine($"  {new string('─', totalWidth + 6)}");

        if (!results.Any())
        {
            Console.WriteLine($"  {Gray}Ingen produkter fundet.{Reset}");
        }
        else
        {
            foreach (var p in results)
            {
                string popularity = p.Popularity switch
                {
                    Popularity.NotPopular => "Ikke populær",
                    Popularity.Popular => "Populær",
                    Popularity.VeryPopular => "Meget populær",
                    _ => "Ukendt"
                };

                Stock? stock = _stockRepo.GetAll().FirstOrDefault(s => s.Product.Id == p.Id);
                string amount = stock != null ? $"{stock.Amount} stk." : "Ukendt";
                string name = p.Name.Length > colName ? p.Name[..(colName - 1)] + "…" : p.Name;

                Console.WriteLine($"  {White}{p.ProductNumber.PadRight(colNumber)}{Gray}{name.PadRight(colName)}{Reset}{$"{p.Price:F2} kr.".PadRight(colPrice)}{popularity.PadRight(colPopularity)}{amount}");
            }
        }

        WaitForEscape();
    }

    // Creates a new product
    private void CreateProduct()
    {
        BuildHeader("Opret produkt");

        Console.Write($"\n  {Gray}Produktnavn: {Reset}");
        string name = Console.ReadLine() ?? "";
        while (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine($"  {Red}Produktnavn må ikke være tomt. Prøv igen:{Reset}");
            Console.Write($"  {Gray}Produktnavn: {Reset}");
            name = Console.ReadLine() ?? "";
        }

        Console.Write($"  {Gray}Produktnummer: {Reset}");
        string productNumber = Console.ReadLine() ?? "";
        while (string.IsNullOrWhiteSpace(productNumber))
        {
            Console.WriteLine($"  {Red}Produktnummer må ikke være tomt. Prøv igen:{Reset}");
            Console.Write($"  {Gray}Produktnummer: {Reset}");
            productNumber = Console.ReadLine() ?? "";
        }

        Console.Write($"  {Gray}Pris: {Reset}");
        if (!double.TryParse(Console.ReadLine(), out double price))
        {
            Console.WriteLine($"  {Red}Ugyldig pris.{Reset}");
            WaitForEscape();
            return;
        }

        Console.Write($"  {Gray}Antal på lager: {Reset}");
        if (!int.TryParse(Console.ReadLine(), out int amount) || amount < 0)
        {
            Console.WriteLine($"  {Red}Ugyldigt antal.{Reset}");
            WaitForEscape();
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

        // Creates a stock entry for the new product
        Stock stock = new Stock(0, product, amount);
        _stockRepo.Add(stock);

        BuildHeader("Opret produkt");
        Console.WriteLine($"\n  {Green}✓ Produktet '{name}' er oprettet med {amount} stk. på lager.{Reset}");
        WaitForEscape();
    }

    // Toggles a product's active status
    private void ToggleProductStatus()
    {
        ShowHeader("Skift produktstatus");
        List<Stock> stocks = _stockRepo.GetAll().ToList();

        if (!stocks.Any())
        {
            Console.WriteLine($"\n  {Gray}Ingen produkter fundet.{Reset}");
            WaitForEscape();
            return;
        }

        string[] stockOptions = stocks.Select(s => $"{s.Product.Name} ({(s.IsActive ? "Aktiv" : "Inaktiv")})").ToArray();
        int choice = ShowInteractiveMenu(stockOptions);
        if (choice == -1) return;
        Stock selectedStock = stocks[choice - 1];

        selectedStock.ToggleStockActivity();
        _stockRepo.Update(selectedStock);

        ShowHeader("Skift produktstatus");
        Console.WriteLine($"\n  {Green}✓ '{selectedStock.Product.Name}' er nu {(selectedStock.IsActive ? "aktiv" : "inaktiv")}.{Reset}");
        WaitForEscape();
    }

    // Deletes a product and its stock
    private void DeleteProduct()
    {
        ShowHeader("Slet produkt");
        List<Product> products = _productRepo.GetAll().ToList();

        if (!products.Any())
        {
            Console.WriteLine($"\n  {Gray}Ingen produkter fundet.{Reset}");
            WaitForEscape();
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
        Console.WriteLine($"\n  {Green}✓ '{selectedProduct.Name}' er slettet.{Reset}");
        WaitForEscape();
    }
}