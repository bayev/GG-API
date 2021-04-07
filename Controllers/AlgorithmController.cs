using Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AlgorithmController : Controller
    {
        private Context _context;
        private readonly UserManager<User> _userManager;
        public AlgorithmController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet("MostPopularProducts")]
        public async Task<ActionResult> MostPopularProducts()
        {
            var popularProducts = _context.Sales
                .OrderByDescending(x => x.AmountSold).ToList();
                
            if(popularProducts.Count() >= 3)
            {
                var topThree = popularProducts.Take(3);

                var productList = new List<Product>();

                foreach (var item in topThree)
                {
                    var product = _context.Products
                    .Where(x => x.Id == item.ProductId)
                    .FirstOrDefault();

                    productList.Add(product);
                }
                

                return Ok(productList);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("NewestArrivedProducts")]
        public async Task<ActionResult> NewestArrivedProducts()
        {
            var latestProducts = _context.Products
                .OrderByDescending(x => x.CreateDate)
                .ToList();

            if (latestProducts.Count() >= 3)
            {
                var selectedProducts = latestProducts
                    .Take(3)
                    .ToList();

                return Ok(selectedProducts);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
