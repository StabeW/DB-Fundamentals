﻿using System.ComponentModel.DataAnnotations;

namespace P03_SalesDatabase.Data.Models
{
    public class Product
    {
        public Product()
        {
            Sales = new HashSet<Sale>();
            Description = "No description";
        }
        public int ProductId { get; set; }

        public string Name { get; set; }

        public double Quantity { get; set; }

        public decimal Price { get; set; }
        public ICollection<Sale> Sales { get; set; }

        public string Description { get; set; }
    }
}
