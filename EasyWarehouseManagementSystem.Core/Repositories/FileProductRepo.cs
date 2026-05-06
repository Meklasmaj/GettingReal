using EasyWarehouseManagementSystem.Core.Interfaces;
using EasyWarehouseManagementSystem.Core.Models;

namespace EasyWarehouseManagementSystem.Core.Repositories;

public class FileProductRepo : IGenericRepo<Product>
{
    private List<Product> _products = new List<Product>();
    private string _filePath;

    public FileProductRepo(string path)
    {
        _filePath = path;
    }

    public Product Get(string id)
    {
        //Not Done
        return _products.Find(p => p.Id == id);
    }

    public IEnumerable<Product> GetAll()
    {
        _products.Clear();
        using (StreamReader sr = new StreamReader(_filePath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    _products.Add(Product.FromString(line));
                }
            }
        }

        return _products;
    }

    public void Add(Product item)
    {
        //Not Done
        _products.Add(item);
    }

    public void Update(Product item)
    {
        //Not Done
        _products[(_products.IndexOf(_products.Find(p => p.Id == item.Id)))] = item;
    }

    public void Delete(string id)
    {
        //Not Done
        _products.Remove(_products.Find(p => p.Id == id));
    }

    public IEnumerable<Product> Search(string term)
    {
        //Needs functionality after SearchEngine
        return new List<Product>();
    }
}