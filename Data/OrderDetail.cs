using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Data
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        //public int AddressId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductImgUrl { get; set; }

        public Order Order { get; set; }
        public Address Addresses { get; set; }

    }
}
