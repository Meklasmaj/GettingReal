namespace EasyWarehouseManagementSystem.ConsoleApp;

public class StockMenu : Menu
{
    private static int _lowStockNotifications;
    public StockMenu(int lowStock)
    {
        _lowStockNotifications = lowStock;
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
                // SearchProduct();
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

    // Placeholder for low stock notification
    private void ShowLowStock()
    {
        var lowStockItems = Program.stockRepo.GetAll().Where(stock => stock.IsBelowMinStock()).ToList(); // Henter alle lagerbeholdninger og filtrerer dem, der er under minimumsbeholdning

        // Opsætter tabel bredder og divider
        int colId = 15;
        int colName = 28;
        int colQty = 9;
        int colMin = 18;

        string divider = $"{DimCyan}├{new string('─', colId)}┼{new string('─', colName)}┼{new string('─', colQty)}┼{new string('─', colMin)}┤{Reset}";

        // Udskriver tabel header
        string headerLine = $"{Cyan}Produkter under minimumsbeholdning{Reset}{DimCyan}" + new string('─', 75 - "Produkter under minimumsbeholdning".Length - 4);
        Console.WriteLine($"{DimCyan}┌──{Reset}{headerLine}┐{Reset}");
        Console.WriteLine($"{DimCyan}│{Reset}{Bold}{Gray} {"Varenr."}{DimCyan}│{Reset}{Bold}{Gray} {"Varenavn"}{DimCyan}│{Reset}{Bold}{Gray} {"Antal"}{DimCyan}│{Reset}{Bold}{Gray} {"Min. beholdning"}{Reset}{DimCyan}│{Reset}");
        Console.WriteLine(divider);

        if (!lowStockItems.Any())   // Tjekker om der er nogen produkter under minimumsbeholdning, og viser en besked hvis ikke
        {
            int totalWidth = colId + colName + colQty + colMin + 3;
            string msg = "Ingen produkter under minimumsbeholdning";
            Console.WriteLine($"{DimCyan}│{Reset}{Green} {msg.PadRight(totalWidth)}{DimCyan}│{Reset}");
        }
        else
        {
            foreach (var stock in lowStockItems)
            {
                var product = Program.productRepo.Get(stock.Id);
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

        Console.WriteLine($"{Cyan}└{new string('─', colId)}┴{new string('─', colName)}┴{new string('─', colQty)}┴{new string('─', colMin)}┘{Reset}");
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
