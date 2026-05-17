using EasyWarehouseManagementSystem.Core.Interfaces;
using EasyWarehouseManagementSystem.Core.Models;
using EasyWarehouseManagementSystem.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWarehouseManagementSystem.ConsoleApp;

public class ProductMenu : Menu
{
    private static readonly string[] Options = ["Se alle produkter", "Søg efter produkt", "Opret produkt", "Markér produkt inaktivt"];
    private IGenericRepo<Product> _productRepo;
    private IGenericRepo<Stock> _stockRepo;
    private CategoryRepo _categoryRepo;

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
                    CreateProduct();
                    break;
                case 4:
                //    ToggleProductActive();
                    break;
            }
        }
    }
    private void ShowAllProducts()
    {
        Console.Clear();
        IEnumerable<Product> products = _productRepo.GetAll();

        if (!products.Any())
        {
            Console.WriteLine("Ingen produkter fundet.");
            Console.ReadKey();
            return;
        }

        // Group products by category
        var grouped = products.GroupBy(p => p.Category.Name);

        foreach (var group in grouped)
        {
            Console.WriteLine($"\n--- {group.Key} ---");
            foreach (var p in group)
            {
                Console.WriteLine(p);
            }
        }

        Console.WriteLine("\nTryk på en tast for at fortsætte...");
        Console.ReadKey();
    }

    // Searches for products by name or product number
    private void SearchProduct()
    {
        Console.Clear();
        Console.Write("Søgeord: ");
        string term = Console.ReadLine() ?? "";

        IEnumerable<Product> results = _productRepo.Search(term);

        if (!results.Any())
        {
            Console.WriteLine("Ingen produkter fundet.");
        }
        else
        {
            foreach (Product p in results)
            {
                Console.WriteLine(p);
            }
        }

        Console.WriteLine("\nTryk på en tast for at fortsætte...");
        Console.ReadKey();
    }
    // Creates a new product
    private void CreateProduct()
    {
        ShowHeader("Opret produkt");
        Console.Write("Produktnavn: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Produktnummer: ");
        string productNumber = Console.ReadLine() ?? "";

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

        Console.WriteLine($"\n✓ Produktet '{name}' er oprettet.");
        Console.ReadKey();
    }
}
