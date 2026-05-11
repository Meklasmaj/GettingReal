using EasyWarehouseManagementSystem.Core.Interfaces;
using EasyWarehouseManagementSystem.Core.Repositories;
using EasyWarehouseManagementSystem.Core.Models;
namespace EWMSTests;

[TestClass]
public sealed class ProductTests
{
    [TestMethod]
    public void Add_Products_InMemoryRepo_CorretAmountSaved()
    {
        IGenericRepo<Product> repo = new InMemoryRepo<Product>();
        CategoryRepo categoryRepo = new CategoryRepo();

        Product product1 = new Product("hammer", 1, 123, Popularity.Popular, categoryRepo.GetCategories().ElementAt(0),
            "hammernummer1");
        Product product2 = new Product("søm", 2, 123, Popularity.VeryPopular, categoryRepo.GetCategories().ElementAt(1),
            "sømnummer2");
        Product product3 = new Product("Skruetrækker", 3, 321, Popularity.NotPopular,
            categoryRepo.GetCategories().ElementAt(0), "Skruetrækker2");

        repo.Add(product1);
        repo.Add(product2);
        repo.Add(product3);

        Assert.AreEqual(3, repo.GetAll().Count());
    }

    [TestMethod]
    public void Remove_Products_InMemoryRepo_CorretAmountRemoved()
    {
        IGenericRepo<Product> repo = new InMemoryRepo<Product>();
        CategoryRepo categoryRepo = new CategoryRepo();

        Product product1 = new Product("hammer", 1, 123, Popularity.Popular, categoryRepo.GetCategories().ElementAt(0),
            "hammernummer1");
        Product product2 = new Product("søm", 2, 123, Popularity.VeryPopular, categoryRepo.GetCategories().ElementAt(1),
            "sømnummer2");
        Product product3 = new Product("Skruetrækker", 3, 321, Popularity.NotPopular,
            categoryRepo.GetCategories().ElementAt(0), "Skruetrækker2");

        repo.Add(product1);
        repo.Add(product2);
        repo.Add(product3);
        
        repo.Delete(2);

        Assert.AreEqual(2, repo.GetAll().Count());
    }
    
    [TestMethod]
    public void Remove_Products_InMemoryRepo_CorretProductRemoved()
    {
        IGenericRepo<Product> repo = new InMemoryRepo<Product>();
        CategoryRepo categoryRepo = new CategoryRepo();

        Product product1 = new Product("hammer", 1, 123, Popularity.Popular, categoryRepo.GetCategories().ElementAt(0),
            "hammernummer1");
        Product product2 = new Product("søm", 2, 123, Popularity.VeryPopular, categoryRepo.GetCategories().ElementAt(1),
            "sømnummer2");
        Product product3 = new Product("Skruetrækker", 3, 321, Popularity.NotPopular,
            categoryRepo.GetCategories().ElementAt(0), "Skruetrækker2");

        repo.Add(product1);
        repo.Add(product2);
        repo.Add(product3);
        
        repo.Delete(2);

        Assert.AreEqual(product1, repo.GetAll().ElementAt(0));
        Assert.AreEqual(product3, repo.GetAll().ElementAt(1));
    }
    
    [TestMethod]
    public void Update_Products_InMemoryRepo_CorretProductUpdated()
    {
        IGenericRepo<Product> repo = new InMemoryRepo<Product>();
        CategoryRepo categoryRepo = new CategoryRepo();

        Product product1 = new Product("hammer", 1, 123, Popularity.Popular, categoryRepo.GetCategories().ElementAt(0),
            "hammernummer1");
        Product product2 = new Product("søm", 2, 123, Popularity.VeryPopular, categoryRepo.GetCategories().ElementAt(1),
            "sømnummer2");
        Product product3 = new Product("Skruetrækker", 3, 321, Popularity.NotPopular,
            categoryRepo.GetCategories().ElementAt(0), "Skruetrækker2");
        Product product4 = new Product("Skruetrækker", 2, 321, Popularity.NotPopular,
            categoryRepo.GetCategories().ElementAt(0), "Skruetrækker2");

        repo.Add(product1);
        repo.Add(product2);
        repo.Add(product3);
        
        repo.Update(product4);

        Assert.AreEqual("Skruetrækker2", repo.GetAll().ElementAt(1).ProductNumber);
    }
    
    [TestMethod]
    public void Get_Products_InMemoryRepo_CorretProductReturned()
    {
        IGenericRepo<Product> repo = new InMemoryRepo<Product>();
        CategoryRepo categoryRepo = new CategoryRepo();

        Product product1 = new Product("hammer", 1, 123, Popularity.Popular, categoryRepo.GetCategories().ElementAt(0),
            "hammernummer1");
        Product product2 = new Product("søm", 2, 123, Popularity.VeryPopular, categoryRepo.GetCategories().ElementAt(1),
            "sømnummer2");
        Product product3 = new Product("Skruetrækker", 3, 321, Popularity.NotPopular,
            categoryRepo.GetCategories().ElementAt(0), "Skruetrækker2");

        repo.Add(product1);
        repo.Add(product2);
        repo.Add(product3);

        Assert.AreEqual(repo.Get(2), repo.GetAll().ElementAt(1));
    }
}