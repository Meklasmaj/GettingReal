using EasyWarehouseManagementSystem.Core.Interfaces;
using System.Text.Json.Serialization;

namespace EasyWarehouseManagementSystem.Core.Models
{
    public class Stock : IHasId, ISearchable
    {
        // The variables and properties of the Stock class
        public int Id { get; set; }
        public Product? Product { get; private set; }
        public int Amount { get; private set; }
        public bool IsActive { get; private set; }

        // The constructor for the Stock class
        [JsonConstructor]
        public Stock(int id, Product product, int amount, bool isActive = true)
        {
            Id = id;
            Product = product ?? throw new ArgumentNullException(nameof(product), "Produktet kan ikke være null.");
            Amount = amount;
            IsActive = isActive;
        }

        // Method to edit the stock amount directly
        public void EditStockAmount(int newAmount)
        {
            if (newAmount < 0)
                throw new ArgumentException("Antal kan ikke være negativt.");
            Amount = newAmount;
        }

        // Method to change the stock status
        public void ToggleStockActivity() => IsActive = !IsActive;

        // Method to check if the stock is below the minimum stock amount
        public bool IsBelowMinStock()
        {
            return IsActive                                     // Tjekker om produktet er aktivt i forhold til lagerstyring
                && Product?.Category?.MinStockAmount > 0        // Tjekker om der er en gyldig minimum lagerbeholdning defineret for produktets kategori
                && Amount < Product?.Category?.MinStockAmount;  // Samlet tjek for at afgøre om lagerbeholdningen er under minimumsgrænsen
        }

        // ToString method to display the stock information
        public override string ToString()
        {
            return $"Varenr.: {Product.ProductNumber} | Produkt: {Product.Name} | Antal: {Amount} stk. | Aktiv: {IsActive}";
        }
        public string GetSearchableText()
        {
            return Product.GetSearchableText();
        }
    }
}
