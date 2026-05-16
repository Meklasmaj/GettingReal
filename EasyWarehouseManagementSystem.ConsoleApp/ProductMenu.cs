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
        int choice = ShowInteractiveMenu(Options);

        switch (choice)
        {
            case -1:
            case 0:
                break;
            case 1:
            //    ShowAllProducts();
                break;
            case 2:
            //    SearchProduct();
                break;
            case 3:
            //    CreateProduct();
                break;
            case 4:
            //    ToggleProductActive();
                break;
        }
    }
}
