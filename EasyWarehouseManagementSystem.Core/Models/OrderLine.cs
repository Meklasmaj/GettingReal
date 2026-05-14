using EasyWarehouseManagementSystem.Core.Interfaces;

namespace EasyWarehouseManagementSystem.Core.Models;

public class OrderLine : IHasId
{
    public int Id { get; set; }
    public int Amount { get; private set; }
    public Stock? Stock { get; private set; }
    
    public OrderLine(){}

    public OrderLine(Stock stock)
    {
        Amount = stock.Product.Category.MinOrderAmount;
        Stock = stock;
    }

    public override string ToString()
    {
        return $"{Stock.Product.Name} | På lager: {Stock.Amount} | Skal bestilles: {Amount}";
    }
}