using EasyWarehouseManagementSystem.Core.Interfaces;
using EasyWarehouseManagementSystem.Core.Models;

namespace EasyWarehouseManagementSystem.Core.Repositories;

public class InMemoryProductRepo : IGenericRepo<Product>
{
    private List<Product> _products = new List<Product>();

    public Product Get(string id)
    {
        return _products.Find(p => p.Id == id);
    }

    public IEnumerable<Product> GetAll()
    {
        return _products;
    }

    public void Add(Product item)
    {
        _products.Add(item);
    }

    public void Update(Product item)
    {
        _products[(_products.IndexOf(_products.Find(p => p.Id == item.Id)))] = item;
    }

    public void Delete(string id)
    {
        _products.Remove(_products.Find(p => p.Id == id));
    }

    public IEnumerable<Product> Search(string term)
    {
        //Needs functionality after SearchEngine
        return new List<Product>();
    }
}