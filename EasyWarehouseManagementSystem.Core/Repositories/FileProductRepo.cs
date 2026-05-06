using EasyWarehouseManagementSystem.Core.Interfaces;
using EasyWarehouseManagementSystem.Core.Models;

namespace EasyWarehouseManagementSystem.Core.Repositories;

public class FileProductRepo : IGenericRepo<Product>
{
    private string _filePath;

    public FileProductRepo(string path)
    {
        _filePath = path;
    }

    public Product? Get(string id)
    {
        return GetAll().FirstOrDefault(p => p.Id == id);
    }

    public IEnumerable<Product> GetAll()
    {
        List<Product> products = new List<Product>();
        try
        {
            //Read file and add products to list
            using (StreamReader sr = new StreamReader(_filePath))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        products.Add(Product.FromString(line));
                    }
                }
            }
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return products;
    }

    public void Add(Product item)
    {
        List<Product> products = GetAll().ToList();
        products.Add(item);
        WriteToFile(products);
    }

    public void Update(Product item)
    {
        List<Product> products = GetAll().ToList();
        products[(products.IndexOf(products.Find(p => p.Id == item.Id)))] = item;
        WriteToFile(products);
    }

    public void Delete(string id)
    {
        List<Product> products = GetAll().ToList();
        products.Remove(products.Find(p => p.Id == id));
        WriteToFile(products);
    }

    public void WriteToFile(List<Product> products)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter("products.tmp"))
            {
                foreach (Product product in products)
                {
                    sw.WriteLine(product.ToString());
                }
            }
            File.Move("products.tmp", _filePath, overwrite:true);
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message);
            File.Delete("products.tmp");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            File.Delete("products.tmp");
        }
    }

    public IEnumerable<Product> Search(string term)
    {
        //Needs functionality after SearchEngine
        return new List<Product>();
    }
}