using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Api.Data
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual IList<CartToProduct> CartToProducts { get; set; }

    }
}
