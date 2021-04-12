﻿using Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("RecommendedProducts")]
        public async Task<ActionResult> GetRecommendedProducts()
        {
            try
            {
                
                User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
                List<Product> RecommendedProductsList = new List<Product>();
                RecommendedProductsList = _context.Products.ToList();

                var order = _context.Orders
                    .Where(x => x.UserId == user.Id).ToList();


                foreach (var item1 in order)
                {
                    var orderDetails = _context.OrderDetails
                    .Where(x => x.OrderId == item1.Id).ToList();

                    if (user != null)
                    {
                        foreach (var item in orderDetails)
                        {
                            var usersOrderedProducts = _context.Products
                                .Where(x => x.Name == item.ProductName).FirstOrDefault();
                            RecommendedProductsList.Remove(usersOrderedProducts);
                        }
                    }
                    else
                    {
                        return StatusCode(404, new { message = "User does not exist" });
                    }
                }
                if (RecommendedProductsList == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(RecommendedProductsList.Take(3));
                }
            }
            catch (System.Exception)
            {
                var deafultProducts = _context.Products.ToList();
                return Ok(deafultProducts.Take(3));
            }
        }

        [HttpGet("MostPopularProducts")]
        public async Task<ActionResult> MostPopularProducts()
        {
            var popularProducts = _context.Sales
                .OrderByDescending(x => x.AmountSold).ToList();

            if (popularProducts.Count() >= 3)
            {
                var topThree = popularProducts.Take(3);
                var productList = new List<Product>();

                foreach (var item in topThree)
                {
                    var product = _context.Products
                    .Where(x => x.Id == item.ProductId)
                    .FirstOrDefault();
                    if (product != null)
                    {
                        productList.Add(product);
                    }
                    else
                    {
                        return BadRequest();
                    }


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
