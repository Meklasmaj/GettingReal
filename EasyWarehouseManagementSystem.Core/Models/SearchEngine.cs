using System;
using System.Collections.Generic;
using System.Text;
using EasyWarehouseManagementSystem.Core.Models;

namespace EasyWarehouseManagementSystem.Core.Models
{
    public static class SearchEngine
    {
        // Searches for products based on Name OR ProductNumber
        public static IEnumerable<Product> Search(string term, IEnumerable<Product> products)
        {
            term = term.ToLower();
            return products.Where(p => p.Name.ToLower().Contains(term) || p.ProductNumber.ToLower().Contains(term));
        }
        // Searches stock for product name matches
        public static IEnumerable<Stock> Search(string term, IEnumerable<Stock> stocks)
        {
            term = term.ToLower();
            return stocks.Where(s => s.Product.Name.ToLower().Contains(term));
        }
        // Searches for suppliers by name or brand. Includes null checks, as name and brand can be null in Supplier.cs
        public static IEnumerable<Supplier> Search(string term, IEnumerable<Supplier> suppliers)
        {
            term = term.ToLower();
            return suppliers.Where(s => (s.Name != null && s.Name.ToLower().Contains(term)) ||
                                        (s.Brands != null && s.Brands.Any(b => b.ToLower().Contains(term))));
        }
        // Searches for draft orders by supplier name or order ID
        public static IEnumerable<DraftOrder> Search(string term, IEnumerable<DraftOrder> orders)
        {
            term = term.ToLower();
            return orders.Where(o => o.Supplier.Name.ToLower().Contains(term) ||
                                     o.Id.ToString().Contains(term));
        }
    }
}
