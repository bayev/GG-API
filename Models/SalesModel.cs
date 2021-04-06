using Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class SalesModel
    {
        public string ProductId { get; set; }
        public int AmountSold { get; set; }
        public DateTime LastSold { get; set; }
        public int Discount { get; set; }
        public Product Product { get; set; }
    }
}
