using System;
using System.Collections.Generic;
using System.Text;
using EasyWarehouseManagementSystem.Core.Interfaces;
using EasyWarehouseManagementSystem.Core.Models;

namespace EasyWarehouseManagementSystem.ConsoleApp;

public class SupplierMenu : Menu
{
    private static readonly string[] Options =
        ["Se alle leverandører", "Søg leverandør", "Opret leverandør", "Slet leverandør"];

    private IGenericRepo<Supplier> _supplierRepo;

    public SupplierMenu(IGenericRepo<Supplier> supplierRepo)
    {
        _supplierRepo = supplierRepo;
    }

    public override void ShowMenu()
    {
        bool running = true;
        while (running)
        {
            ShowHeader("Leverandører");
            int choice = ShowInteractiveMenu(Options);

            switch (choice)
            {
                case -1:
                    running = false;
                    break;
                case 1:
                    ShowAllSuppliers();
                    break;
                case 2:
                    SearchSupplier();
                    break;
                case 3:
                    CreateSupplier();
                    break;
                case 4:
                    DeleteSupplier();
                    break;
            }
        }
    }

    private void ShowAllSuppliers()
    {
        Console.Clear();
        IEnumerable<Supplier> suppliers = _supplierRepo.GetAll();

        if (!suppliers.Any())
        {
            Console.WriteLine("Ingen leverandører fundet.");
            Console.ReadKey();
            return;
        }

        foreach (Supplier s in suppliers)
        {
            Console.WriteLine(s);
        }

        Console.WriteLine("\nTryk på en tast for at fortsætte...");
        Console.ReadKey();
    }

    private void SearchSupplier()
    {
        Console.Clear();
        Console.Write("Søgeord: ");
        string term = Console.ReadLine() ?? "";

        IEnumerable<Supplier> results = _supplierRepo.Search(term);

        if (!results.Any())
        {
            Console.WriteLine("Ingen leverandører fundet.");
        }
        else
        {
            foreach (Supplier s in results)
            {
                Console.WriteLine(s);
            }
        }

        Console.WriteLine("\nTryk på en tast for at fortsætte...");
        Console.ReadKey();
    }

    private void CreateSupplier()
    {
        ShowHeader("Opret leverandør");

        Console.Write("Leverandørnavn: ");
        string name = Console.ReadLine() ?? "";
        while (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Navn må ikke være tomt. Prøv igen: ");
            name = Console.ReadLine() ?? "";
        }

        Console.Write("Minimum leveringsgrænse (kr.): ");
        if (!double.TryParse(Console.ReadLine(), out double lowerDeliveryLimit))
        {
            Console.WriteLine("Ugyldigt beløb.");
            Console.ReadKey();
            return;
        }

        // Indlæs mærker - tom linje afslutter
        List<string> brands = new List<string>();
        Console.WriteLine("Indtast mærker (tryk Enter med tom linje for at afslutte):");
        while (true)
        {
            Console.Write("Mærke: ");
            string brand = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(brand)) break;
            brands.Add(brand);
        }

        Supplier supplier = new Supplier(0, name, brands, lowerDeliveryLimit);
        _supplierRepo.Add(supplier);

        Console.WriteLine($"\n✓ Leverandøren '{name}' er oprettet.");
        Console.ReadKey();
    }

    private void DeleteSupplier()
    {
        ShowHeader("Slet leverandør");
        List<Supplier> suppliers = _supplierRepo.GetAll().ToList();

        if (!suppliers.Any())
        {
            Console.WriteLine("Ingen leverandører fundet.");
            Console.ReadKey();
            return;
        }

        string[] supplierOptions = suppliers.Select(s => s.Name ?? "Ukendt").ToArray();
        int choice = ShowInteractiveMenu(supplierOptions);
        if (choice == -1) return;

        Supplier selectedSupplier = suppliers[choice - 1];

        // Bekræft sletning
        Console.WriteLine($"\nEr du sikker på at du vil slette '{selectedSupplier.Name}'? (j/n): ");
        string? answer = Console.ReadLine()?.Trim().ToLower();

        if (answer == "j" || answer == "ja")
        {
            _supplierRepo.Delete(selectedSupplier.Id);
            Console.WriteLine($"\n✓ Leverandøren '{selectedSupplier.Name}' er slettet.");
        }
        else
        {
            Console.WriteLine("\nSletning annulleret.");
        }

        Console.ReadKey();
    }
}