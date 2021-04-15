﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Api.Data
{
    public class Product
    {  
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public DateTime CreateDate { get; set; }
        public int Stock { get; set; }
        public string Size { get; set; }
        public string Brand { get; set; }
        public decimal Discount { get; set; }
        public bool Highlighted { get; set; }

        public Product()
        {
            Id = Guid.NewGuid().ToString();
            Name = String.Empty;
            CreateDate = DateTime.Now;
        }
    }


    public class PostProductModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public int Stock { get; set; }
        public string Size { get; set; }
        public string Brand { get; set; }
        public decimal Discount { get; set; }
        public bool Highlighted { get; set; }

    }
    public class ExposedProducts
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
