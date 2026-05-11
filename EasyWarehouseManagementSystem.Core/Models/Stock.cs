namespace EasyWarehouseManagementSystem.Core.Models
{
    public class Stock
    {
        // The variables and properties of the Stock class
        public Product Product { get; }
        public int Amount { get; private set; }
        public bool IsActive { get; private set; }

        // The constructor for the Stock class
        public Stock(Product product, int amount)
        {
            Product = product;
            Amount = amount;
            IsActive = true;
        }

        // Method to add stock
        public void AddStock(int amount)
        {
            if(amount <= 0)
                throw new ArgumentException("Antallet, der skal tilføjes, skal være større end 0.");
            Amount += amount;
        }

        // Method to remove stock
        public void RemoveStock(int amount)
        {
            if (Amount - amount < 0)
                throw new InsufficientStockException($"Kan ikke fjerne {amount} — kun {Amount} på lager.");
            Amount -= amount;
        }

        // Method to show the stock amount
        public int ShowStock() => Amount;

        // Method to change the stock status
        public void ToggleStockActivity()
        {
            IsActive = !IsActive;
        }

        // Method to check if the stock is below the minimum stock amount
        public bool IsBelowMinStock()
        {
            return IsActive 
                && Product.Category.MinStockAmount > 0
                && Amount < Product.Category.MinStockAmount;
        }

        // ToString method to display the stock information
        public override string ToString()
        {
            return $"Varenr.: {Product.Id} | Produkt: {Product.Name} | Antal: {Amount} stk. | Aktiv: {IsActive}";
        }

        // FromString method to create a Stock object from a string
        //public static Stock FromString(string stockString)
        //{
        //    if (!isActive)
        //    {
        //        stock.UpdateStock();
        //    }
        //    return stock;
        //}
    }
}
