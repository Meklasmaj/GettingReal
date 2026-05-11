namespace EasyWarehouseManagementSystem.Core.Models
{
    public class Supplier
    {
        public int? Id { get; set; } = 0;
        public string? Name { get; private set; } = "Unknown";
        public List<string>? Brands { get; private set; } = new List<string>();
        public double? LowerDeliveryLimit { get; private set; } = 0.0;

        // Constructor for the Supplier class
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

        // ToString method to display the supplier information
        public override string ToString()
        {
            return $"Leverandør ID: {Id} | Navn: {Name} | Mærker: {string.Join(", ", Brands)} | Minimumsordre: {LowerDeliveryLimit}";
        }
    }
}
