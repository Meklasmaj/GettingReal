namespace EasyWarehouseManagementSystem.Core.Models;

public class Category
{
    public string Name  { get; private set; }
    public int MinStockAmount { get; private set; }
    public int MinOrderAmount { get; private set; }

    public Category(string name, int minStockAmount, int minOrderAmount)
    {
        Name = name;
        MinStockAmount = minStockAmount;
        MinOrderAmount = minOrderAmount;
    }
}