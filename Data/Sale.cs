using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Data
{
    public class Sale
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public int AmountSold { get; set; }
        public DateTime LastSold { get; set; }
        public int Discount { get; set; }
        public Product Product { get; set; }
        public Sale()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
