﻿using Api.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QueryController : Controller
    {
        private Context _context;
        public QueryController(Context context)
        {
            _context = context;
        }

        [HttpGet("search/{queryString}")]
        public async Task<ActionResult> SearchProductResult([FromRoute] string queryString)
        {
            try
            {
                if(queryString == "rea")
                {
                    var q2 = _context.Products
                        .Where(x => x.Discount != default).ToList();
                    return Ok(q2);
                }
                else
                {
                    var q1 = _context.Products
                    .Where(x => x.Category.Contains(queryString) || x.Name.Contains(queryString)).ToList();
                    return Ok(q1);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Sorry, something happend. {ex.ToString()}" });
            }
        }
        [HttpGet("all")]
        public async Task<ActionResult> GetAllProducts()
        {

            List<Product> listOfProducts = new List<Product>();

            if (!listOfProducts.Any())
            {
                var query = _context.Products;

                foreach (var item in query)
                {
                    Product product = new Product();
                    product.Id = item.Id;
                    product.Name = item.Name;
                    product.Price = item.Price;
                    product.Weight = item.Weight;
                    product.Description = item.Description;
                    product.Image = item.Image;
                    product.Category = item.Category;
                    product.CreateDate = item.CreateDate;
                    product.Stock = item.Stock;
                    product.Size = item.Size;
                    product.Brand = item.Brand;
                    product.Discount = item.Discount;

                    listOfProducts.Add(product);
                }
                return Ok(listOfProducts);
            }
            else
            {
                return NotFound(new { message = "Sorry, no products found" });
            }
        }
        [HttpGet("get/{productid}")]
        public async Task<ActionResult> GetProduct([FromRoute] string productid)
        {

            Product product = _context.Products.Where(x => x.Id == productid).FirstOrDefault();

            return Ok(product);

        }
    }
}