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
        Console.Clear();
        int choice = ShowInteractiveMenu(Options);

        switch (choice)
        {
            case -1:
            case 0:
                break;
            case 1:
                ShowAllProducts();
                break;
            case 2:
                SearchProduct();
                break;
            case 3:
            //    CreateProduct();
                break;
            case 4:
            //    ToggleProductActive();
                break;
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
        IEnumerable<IGrouping<string, Product>> grouped = products.GroupBy(p => p.Category.Name);

        foreach (IGrouping<string, Product> group in grouped)
        {
            Console.WriteLine($"\n--- {group.Key} ---");
            foreach (Product p in group)
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
}
