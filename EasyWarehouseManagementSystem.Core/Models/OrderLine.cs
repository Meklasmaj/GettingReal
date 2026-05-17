using System.Text.Json.Serialization;
using EasyWarehouseManagementSystem.Core.Interfaces;

namespace EasyWarehouseManagementSystem.Core.Models;

public class OrderLine
{
    public int Amount { get; private set; }
    public Stock? Stock { get; private set; }
    
    [JsonConstructor]

    public OrderLine(Stock stock)
    {
        Amount = stock.Product.Category.MinOrderAmount;
        Stock = stock;
    }

    public void EditAmount(int newAmount)
    {
        if(newAmount > 0)
        {
            Amount = newAmount;
        }
    }

    public override string ToString()
    {
        return $"{Stock.Product.Name} | På lager: {Stock.Amount} | Skal bestilles: {Amount}";
    }
}