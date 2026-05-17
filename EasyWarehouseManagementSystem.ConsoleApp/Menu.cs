using System.ComponentModel;
using System.Text.RegularExpressions;

namespace EasyWarehouseManagementSystem.ConsoleApp;

public abstract class Menu
{
    public abstract void ShowMenu();

    // ANSI Colours and styles =================================
    protected const string Reset    = "\e[0m";
    protected const string Bold     = "\e[1m";
    protected const string Dim      = "\e[2m";

    protected const string White    = "\e[97m";
    protected const string Gray     = "\e[90m";
    protected const string Green    = "\e[92m";
    protected const string Cyan     = "\e[96m";
    protected const string DimCyan  = "\e[2;96m";
    protected const string Red      = "\e[1;91m";
    protected const string DimRed   = "\e[2;91m";
    protected const string Magenta  = "\e[95m";
    protected const string Blue     = "\e[94m";
    protected const string Yellow   = "\e[93m";

    // Header with logo and title ==============================
    protected void ShowHeader(string title)
    {
        Console.Clear();
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int width = 65;
        string line = new string('─', width - 2);

        Console.WriteLine($"{Cyan}┌{line}┐{Reset}");
        Console.WriteLine($"{Cyan}│{Reset}{Bold}{Gray}        [ EZ ] {White}Warehouse Management System        {Reset}{Cyan}│{Reset}");
        Console.WriteLine($"{Cyan}├{line}┤{Reset}");

        // Titel på aktuel menu
        string paddedTitle = title.PadRight(width - 4);
        Console.WriteLine($"{Cyan}│{Reset}  {Bold}{White}{paddedTitle}{Reset}{Cyan}│{Reset}");
        Console.WriteLine($"{Cyan}├{line}┤{Reset}");
    }

    //  Footer with navigation-hint ==============================
    protected void ShowFooter()
    {
        int width = 65;
        string line = new string('─', width - 2);
        Console.WriteLine($"{Cyan}├{line}┤{Reset}");
        Console.WriteLine($"{Cyan}│{Reset}{Gray}  ▲ ▼ Naviger  {Reset}{Cyan}│{Reset}{Gray}  Enter = Vælg  {Reset}{Cyan}│{Reset}{Gray}  Esc = Tilbage  {Reset}{Cyan}│{Reset}");
        Console.WriteLine($"{Cyan}└{line}┘{Reset}");
    }

    // Arrow-key based menu system
    protected int ShowInteractiveMenu(string[] options)
    {
        Console.CursorVisible = false;

        int width = 65;
        int menuChoice = 1;
        int count = options.Length;
        string selected = "\e[1;92m"; // Color

        (int left, int top) = Console.GetCursorPosition();

        while (true)
        {
            Console.SetCursorPosition(left, top);

            for (int i = 0; i < count; i++)
            {
                bool isSelected = (menuChoice == i + 1);
                Console.WriteLine($"{Cyan}│{Reset}  {(isSelected ? selected + "►" : " ")} {options[i].PadRight(width - 6)}{Reset}{Cyan}│{Reset}");
            }

            ShowFooter();
            Console.WriteLine();
            if (ShowHeader == Program.MainMenu.ShowHeader) // Tjekker om vi er i hovedmenuen, og viser notifikationer hvis det er tilfældet
            {
                ShowNotifications(Program.CheckDraftOrderNotifications(), Program.CheckLowStockNotifications());
            }
            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    menuChoice = (menuChoice == count ? 1 : menuChoice + 1);
                    break;

                case ConsoleKey.UpArrow:
                    menuChoice = (menuChoice == 1 ? count : menuChoice - 1);
                    break;

                case ConsoleKey.Enter:
                    return menuChoice;

                case ConsoleKey.Escape:
                    return -1;
            }
        }
    }

    // Notificantions ================================================'
    public void ShowNotifications(int notifications = 0, int lowStockCount = 0)
    {
        int width = 52;
        string line = new string('─', width - 2);
        string headerLine = $"{Red}Notifikationer{Reset}{DimRed}" + new string('─', width - "Notifikationer".Length - 4);
        Console.WriteLine($"{DimRed}┌──{Reset}{headerLine}┐{Reset}");

        // Notifikationsbanner hvis der er åbne kladdeordrer
        if (notifications >= 0)
        {
            string notificationText = $"  ● {notifications} kladdeordre(r) afventer godkendelse";
            string paddedNotif = notificationText.PadRight(width - 2);
            Console.WriteLine($"{DimRed}│{Reset}{Yellow}{Bold}{paddedNotif}{Reset}{DimRed}│{Reset}");
        }

        // Notifikationsbanner hvis der er produkter under minimumsbeholdning
        if (lowStockCount >= 0)
        {
            string lowStockText = $"  ● {lowStockCount} produkt(er) under minimumsbeholdning";
            string paddedLowStock = lowStockText.PadRight(width - 2);
            Console.WriteLine($"{DimRed}│{Reset}{Red}{Bold}{paddedLowStock}{Reset}{DimRed}│{Reset}");
        }
        Console.WriteLine($"{DimRed}└{line}┘{Reset}");
    }
}