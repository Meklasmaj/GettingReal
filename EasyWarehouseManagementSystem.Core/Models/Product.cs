using EasyWarehouseManagementSystem.Core.Interfaces;
using EasyWarehouseManagementSystem.Core.Repositories;

namespace EasyWarehouseManagementSystem.Core.Models;

public class Product : IHasId, ISearchable
{
    public string Name { get; private set; }
    public int Id { get; }
    public string ProductNumber { get; private set; }
    public double Price { get; private set; }
    public Popularity Popularity { get; private set; }
    public Category Category { get; private set; }
    private static CategoryRepo _categoryRepo = new CategoryRepo();

    public Product(string name, int id, double price, Popularity popularity, Category category, string productNumber)
    {
        Name = name;
        Id = id;
        Price = price;
        Popularity = popularity;
        Category = category;
        ProductNumber = productNumber;
    }
    
    public override string ToString()
    {
        return $"Produkt-navn : {Name} | Produkt-nummer : {ProductNumber} | Pris : {Price} |  Popularity : {Popularity} | Category : {Category.Name}";
    }

    public string GetSearchableText()
    {
        return $"{Name} {ProductNumber}";
    }
}