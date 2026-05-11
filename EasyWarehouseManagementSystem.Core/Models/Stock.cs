namespace EasyWarehouseManagementSystem.Core.Models
{
    internal class Stock
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
            Amount += amount;
        }

        // Method to remove stock
        public void RemoveStock(int amount)
        {
            if (Amount - amount < 0)
            {
                throw new InvalidOperationException("Cannot remove more stock than available.");
            }
            Amount -= amount;
        }

        // Method to change the stock status
        public void UpdateStock()
        {
            if (IsActive)
            {
                IsActive = false;
            }
            else
            {
                IsActive = true;
            }
        }

        // Method to check if the stock is below the minimum stock amount
        public bool IsBelowMinStock()
        {
            if (Product.Category.MinStockAmount == 0)
            {
                return false;
            }
            else
            {
                if (IsActive)
                    return Amount < Product.Category.MinStockAmount;
            }
            return false;
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
