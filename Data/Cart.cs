using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Api.Data
{
    public class Cart
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public virtual IList<CartToProduct> CartToProducts { get; set; }

        public Cart()
        {
            Id = Guid.NewGuid().ToString();
            
        }
    }
}
