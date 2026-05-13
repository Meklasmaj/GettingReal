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
        public OrderStatus? Status { get; set; }
        public Product? Product { get; set; }
        public Supplier? Supplier { get; set; }
        public DateTime? TimeStamp { get; set; }

        // Constructor for the DraftOrder class
        [JsonConstructor] // This attribute tells JsonSerializer which constructor to use during deserialization.
        public DraftOrder(int id, OrderStatus? status = null, Product? product = null, Supplier? supplier = null, DateTime? timeStamp = null)
        {
            Id = id;
            Status = status ?? OrderStatus.Open;    // If null - use Open as default status
            Product = product;
            Supplier = supplier;
            TimeStamp = timeStamp ?? DateTime.Now;  // If null - use the current date and time as default timestamp
        }

        public override string ToString()
        {
            return $"Ordre ID: {Id} | Oprettet: {TimeStamp} | Status: {Status} | Leverandør: {Supplier?.Name} | Produkt: {Product?.Name} | Antal: {Product?.Category?.MinOrderAmount}";
        }
        public string GetSearchableText() => $"{Id} {Supplier?.Name} {Product?.Name}";
    }
}
