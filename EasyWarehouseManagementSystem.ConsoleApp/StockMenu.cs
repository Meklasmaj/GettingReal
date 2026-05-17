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
                PrintStockList();
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

    // Footer Builder: prints a footer with navigation hint and waits for Esc key to return to menu
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

    // Searches for a product by name or product number and displays its stock information, including current quantity and minimum stock level, with color coding for low stock
    private void SearchProduct()
    {
        Console.Clear();
        BuildHeader("Søg produkt");

        Console.Write("   Indtast søgeord: ");
        string term;

        if (Console.ReadKey().Key == ConsoleKey.Escape)
        {
            ShowMenu();
            return;
        }

        term = Console.ReadLine() ?? "";

        IEnumerable<Product> results = _productRepo.Search(term);
        var stockResults = _stockRepo.GetAll().Where(s => results.Any(r => r.Id == s.Product.Id)); // Finder lagerbeholdningen for de fundne produkter

        BuildStockTable("Søgeresultater");
        if (!results.Any())
        {
            string msg = "Ingen produkter fundet...";
            Console.WriteLine($"{DimCyan}│{Reset}{White} {msg.PadRight(totalWidth)}{DimCyan}│{Reset}");
        }
        else if (!stockResults.Any())
        {
            string msg = "Ingen lagerinformation tilgængelig for dette produkt.";
            Console.WriteLine($"{DimCyan}│{Reset}{White} {msg.PadRight(totalWidth)}{DimCyan}│{Reset}");
        }
        else
        {
            foreach (var stock in stockResults)
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
                if (stockResults.Last() != stock)
                {
                    Console.WriteLine(divider);
                }
            }
        }
        Console.WriteLine($"{DimCyan}└{new string('─', colId)}┴{new string('─', colName)}┴{new string('─', colQty)}┴{new string('─', colMin)}┘{Reset}");
        Console.WriteLine("\n   Vil du ændre beholdningen på et produkt?\n   Enter = Ja  |  Escape = Nej");

        if (Console.ReadKey().Key == ConsoleKey.Enter)
        {
            EditStock(stockResults);
        }
        else
        {
            ShowMenu();
            return;
        }
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
                if (lowStockItems.Last() != stock)
                {
                    Console.WriteLine(divider);
                }
            }
        }
        BuildStockTableFooter();
    }
    private void PrintStockList()
    {
        Console.Clear();
        BuildStockTable("Lagerliste");
        var stockItems = _stockRepo.GetAll().ToList();
        if (!stockItems.Any())
        {
            string msg = "Ingen lagerinformation tilgængelig.";
            Console.WriteLine($"{DimCyan}│{Reset}{White} {msg.PadRight(totalWidth)}{DimCyan}│{Reset}");
        }
        else
        {
            foreach (var stock in stockItems)
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
                if (stockItems.Last() != stock)
                {
                    Console.WriteLine(divider);
                }
            }
        }
        BuildStockTableFooter();
    }
    private void EditStock(IEnumerable<Stock> stockResults)
    {
        // ── Trin 1: Naviger til produkt med piletaster ──────────────────────
        var stocks = stockResults.ToList();
        int selected = 0;

        while (true)
        {
            Console.Clear();
            BuildHeader("Rediger lagerbeholdning");
            Console.WriteLine($"{Gray}  Vælg produkt med ▲ ▼ og tryk Enter{Reset}\n");

            for (int i = 0; i < stocks.Count; i++)
            {
                var s = stocks[i];
                string name = s.Product.Name.Length > 30
                    ? s.Product.Name[..29] + "…"
                    : s.Product.Name;
                string qty = s.Amount.ToString();
                string min = s.Product.Category.MinStockAmount.ToString();
                string rowColor = s.Amount <= s.Product.Category.MinStockAmount / 2 ? Red : Yellow;

                if (i == selected)
                    Console.WriteLine($"{Bold}{White}  ► {name.PadRight(30)} Antal: {qty.PadRight(6)} Min: {min}{Reset}");
                else
                    Console.WriteLine($"{Gray}    {name.PadRight(30)} Antal: {qty.PadRight(6)} Min: {min}{Reset}");
            }

            ShowFooter();

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selected = selected == 0 ? stocks.Count - 1 : selected - 1;
                    break;
                case ConsoleKey.DownArrow:
                    selected = selected == stocks.Count - 1 ? 0 : selected + 1;
                    break;
                case ConsoleKey.Enter:
                    goto EditSelected;
                case ConsoleKey.Escape:
                    ShowMenu();
                    return;
            }
        }

    EditSelected:

        // ── Trin 2: Vis nuværende status og indtast nyt antal ───────────────
        var stock = stocks[selected];
        var product = stock.Product;

        Console.Clear();
        BuildHeader("Rediger lagerbeholdning");

        int colW = 46;
        Console.WriteLine($"{DimCyan}┌{new string('─', colW)}┐{Reset}");
        Console.WriteLine($"{DimCyan}│{Reset} {Bold}{White}{"Produkt:",-12}{Reset}{product.Name.PadRight(colW - 13)}{DimCyan}│{Reset}");
        Console.WriteLine($"{DimCyan}│{Reset} {Bold}{White}{"Varenr.:",-12}{Reset}{product.ProductNumber.ToString().PadRight(colW - 13)}{DimCyan}│{Reset}");
        Console.WriteLine($"{DimCyan}│{Reset} {Bold}{White}{"Antal:",-12}{Reset}{stock.Amount.ToString().PadRight(colW - 13)}{DimCyan}│{Reset}");
        Console.WriteLine($"{DimCyan}│{Reset} {Bold}{White}{"Min. beh.:",-12}{Reset}{product.Category.MinStockAmount.ToString().PadRight(colW - 13)}{DimCyan}│{Reset}");
        Console.WriteLine($"{DimCyan}└{new string('─', colW)}┘{Reset}");

        // ── Trin 3: Indtast nyt antal (kun int tilladt) ─────────────────────
        int newAmount = -1;
        while (true)
        {
            Console.Write($"\n  {White}Indtast nyt antal: {Reset}");
            string input = ReadIntOnly();

            if (input == null) // Escape trykket
            {
                ShowMenu();
                return;
            }

            if (int.TryParse(input, out int parsed) && parsed >= 0)
            {
                newAmount = parsed;
                break;
            }

            Console.WriteLine($"{Red}  Ugyldigt antal – indtast et helt tal større end eller lig med 0{Reset}");
        }

        // ── Trin 4: Verificering ────────────────────────────────────────────
        Console.Clear();
        BuildHeader("Bekræft ændring");

        Console.WriteLine($"\n  {White}Produkt:{Reset}  {product.Name}");
        Console.WriteLine($"  {White}Gammelt antal:{Reset}  {stock.Amount}");
        Console.WriteLine($"  {Green}Nyt antal:{Reset}     {newAmount}\n");
        Console.WriteLine($"  {Yellow}Er du sikker? Enter = Bekræft  |  Escape = Annullér{Reset}");

        var confirm = Console.ReadKey(true).Key;
        if (confirm == ConsoleKey.Enter)
        {
            //stock.Amount = newAmount;
            _stockRepo.Update(stock);
            Console.Clear();
            BuildHeader("Rediger lagerbeholdning");
            Console.WriteLine($"\n  {Green}✓ Lagerbeholdning opdateret til {newAmount} stk.{Reset}");
            Console.WriteLine($"\n  {Gray}Tryk en tast for at fortsætte...{Reset}");
            Console.ReadKey(true);
        }
        else
        {
            Console.WriteLine($"\n  {Gray}Ændring annulleret.{Reset}");
            Console.WriteLine($"\n  {Gray}Tryk en tast for at fortsætte...{Reset}");
            Console.ReadKey(true);
        }

        ShowMenu();
    }

    // ── Hjælpemetode: Læs kun cifre og tillad Escape ───────────────────────────
    private string ReadIntOnly()
    {
        var input = new System.Text.StringBuilder();
        Console.CursorVisible = true;

        while (true)
        {
            var keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                Console.CursorVisible = false;
                return null;
            }

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.CursorVisible = false;
                Console.WriteLine();
                return input.ToString();
            }

            if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input.Remove(input.Length - 1, 1);
                Console.Write("\b \b");
                continue;
            }

            // Tillad kun cifre
            if (char.IsDigit(keyInfo.KeyChar))
            {
                input.Append(keyInfo.KeyChar);
                Console.Write(keyInfo.KeyChar);
            }
            // Ignorer alt andet (bogstaver, specialtegn osv.)
        }
    }
}
