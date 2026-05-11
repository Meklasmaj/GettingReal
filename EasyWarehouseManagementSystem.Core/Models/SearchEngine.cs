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

    }
}
