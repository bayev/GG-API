using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Api.Data
{
    public class CartToProduct
    {
        [Key]
        public string Id { get; set; }
        public int Timestamp { get; set; }
        public int Amount { get; set; }
        [ForeignKey("Id")]
        public virtual Cart Cart { get; set; }
        
        [ForeignKey("Id")]
        public virtual Product Product { get; set; }
        public CartToProduct()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
