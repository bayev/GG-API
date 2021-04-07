using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Data
{
    public class Order
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ShippingAddress { get; set; }
        //public int AddressId { get; set; }
        public decimal Amount { get; set; }
        public string OrderEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentMethod { get; set; }

        public IList<OrderDetail> OrderDetails { get; set; }
        //public Address Addresses { get; set; }

        public Order()
        {
            Id = Guid.NewGuid().ToString();
            OrderDetails = new List<OrderDetail>();
        }
    }

    public class PostOrderModel
    {
        public string ShippingAddress { get; set; }
        public string paymentMethod { get; set; }
        public string totalAmount { get; set; }
    }

}
