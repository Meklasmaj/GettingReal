using EasyWarehouseManagementSystem.Core.Repositories;

namespace EasyWarehouseManagementSystem.Core.Models;

public class Product
{
    public string Name { get; private set; }
    public string Id { get; private set; }
    public double Price { get; private set; }
    public Popularity Popularity { get; private set; }
    public Category Category { get; private set; }
    private CategoryRepo _categoryRepo = new CategoryRepo();

    public Product(string name, string id, double price, Popularity popularity, Category category)
    {
        Name = name;
        Id = id;
        Price = price;
        Popularity = popularity;
        Category = category;
    }

    /// <summary>
    /// Method to create a string from a Product
    /// </summary>
    public override string ToString()
    {
        return $"{Name}|{Id}|{Price}|{Popularity}|{_categoryRepo.GetCategories().ToList().IndexOf(Category)}";
    }

    /// <summary>
    /// Method to create a Product from a string
    /// </summary>
    public Product FromString(string data)
    {
        string[] parts = data.Split('|');
        string name = parts[0];
        string id = parts[1];
        double price = Convert.ToDouble(parts[2]);
        Popularity popularity = (Popularity)Enum.Parse(typeof(Popularity), parts[3]);
        Category category = _categoryRepo.GetCategories().ToList()[Convert.ToInt32(parts[4])];
        return new Product(name, id, price, popularity, category);
    }
}