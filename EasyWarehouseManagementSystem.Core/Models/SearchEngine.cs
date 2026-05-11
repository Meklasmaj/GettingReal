using System;
using System.Collections.Generic;
using System.Text;
using EasyWarehouseManagementSystem.Core.Models;

namespace EasyWarehouseManagementSystem.Core.Models
{
    public static class SearchEngine
    {
        // Searches based on Name OR ProductNumber
        public static IEnumerable<Product> Search(string term, IEnumerable<Product> products)
        {
            term = term.ToLower();
            return products.Where(p => p.Name.ToLower().Contains(term) || p.ProductNumber.ToLower().Contains(term));
        }
    }
}
