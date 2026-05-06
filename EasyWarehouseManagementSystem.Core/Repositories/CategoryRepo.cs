using EasyWarehouseManagementSystem.Core.Models;

namespace EasyWarehouseManagementSystem.Core.Repositories;

public class CategoryRepo
{
    private List<Category> _categories = new List<Category>()
    {
        new Category("Tøj", 4, 4),
        new Category("ElVærktøj", 3, 3)
    };

    public IEnumerable<Category> GetCategories()
    {
        return _categories;
    }
}