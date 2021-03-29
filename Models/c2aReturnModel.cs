using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class c2aReturnModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public DateTime CreateDate { get; set; }
        public string Size { get; set; }
        public string Brand { get; set; }
    
        //////////////////////////////////
       
        public int Amount { get; set; }
        public string c2pID { get; set; }
    }
}
