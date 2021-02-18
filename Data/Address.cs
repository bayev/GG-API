using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Api.Data
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }


    }
}
