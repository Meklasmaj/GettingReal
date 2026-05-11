using EasyWarehouseManagementSystem.Core.Interfaces;
using System.Text.Json.Serialization;

namespace EasyWarehouseManagementSystem.Core.Models
{
    public class Supplier : IHasId
    {
        public int Id { get; }
        public string? Name { get; private set; } = "Unknown";
        public List<string>? Brands { get; private set; } = new List<string>();
        public double? LowerDeliveryLimit { get; private set; } = 0.0;

        // Constructor for the Supplier class
        [JsonConstructor] // This attribute allows the constructor to be used during JSON deserialization, since we have private setters for the properties.
        public Supplier(int id, string name, List<string> brands, double lowerDeliveryLimit)
        {
            Id = id;
            Name = name;
            Brands = brands;
            LowerDeliveryLimit = lowerDeliveryLimit;
        }

        // Method to get brands as an IEnumerable<string>
        public IEnumerable<string> GetBrands()
        {
            return Brands ?? Enumerable.Empty<string>(); // Return an empty enumerable if Brands is null
        }

        // Method to add a brand to the supplier's list of brands
        public void AddBrand(string brand)
        {
            if (Brands == null)
                Brands = new List<string>();
            Brands.Add(brand);
        }

        // ToString method to display the supplier information
        public override string ToString()
        {
            return $"Leverandør ID: {Id} | Navn: {Name} | Mærker: {string.Join(", ", Brands ?? Enumerable.Empty<string>())} | Minimumsordre: {LowerDeliveryLimit} kr.";
        }
    }
}
