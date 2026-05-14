using EasyWarehouseManagementSystem.Core.Interfaces;
using System.Text.Json.Serialization;

namespace EasyWarehouseManagementSystem.Core.Models
{
    // Trying out the new constructor mode for DraftOrder class (C# 12.0 feature)
    public class DraftOrder : IHasId, ISearchable
    {
        public int Id { get; set; }
        // public set is nessary for JSON deserialization. Encapsulation is handled by the application layer,
        // since the DraftOrder class is only used for creating new orders, and the properties are set at the time of creation.
        public OrderStatus? Status
        {
            get;
            set
            {
                // Adds the new amount of products to the stock when Status is Done
                if (value == OrderStatus.Done && OrderLines != null && OrderLines.Count > 0)
                {
                    foreach (var orderLine in OrderLines)
                    {
                        orderLine.Stock.EditStockAmount(orderLine.Amount + orderLine.Stock.Amount);
                    }
                }
            }
        }

        public List<OrderLine>? OrderLines { get; set; }
        public Supplier? Supplier { get; set; }
        public DateTime? TimeStamp { get; set; }

        // Constructor for the DraftOrder class
        [JsonConstructor] // This attribute tells JsonSerializer which constructor to use during deserialization.
        public DraftOrder(int id, OrderStatus? status = null, List<OrderLine>? orderLines = null, Supplier? supplier = null, DateTime? timeStamp = null)
        {
            Id = id;
            Status = status ?? OrderStatus.Open;    // If null - use Open as default status
            OrderLines = orderLines ?? new List<OrderLine>();
            Supplier = supplier;
            TimeStamp = timeStamp ?? DateTime.Now;  // If null - use the current date and time as default timestamp
        }

        public override string ToString()
        {
            string line = $"Ordre ID: {Id} | Oprettet: {TimeStamp} | Status: {Status} | Leverandør: {Supplier?.Name}";
            if(OrderLines.Count > 0)
            {
                foreach (var orderLine in OrderLines)
                {
                    line += $" | Produkt: {orderLine.Stock.Product.Name}";
                    line += $" | Antal: {orderLine.Stock.Product.Category.MinOrderAmount}";
                }
            }
            return line;
        }
        public string GetSearchableText()
        {
            string line = $"{Id} {Supplier?.Name}";
            if(OrderLines.Count > 0)
            {
                foreach (var orderLine in OrderLines)
                {
                    line += $" {orderLine.Stock.Product.Name}";
                }
            }
            return line;
        }
    }
}
