﻿using Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductsController : Controller
    {
        private  Context _context;
        private readonly UserManager<User> _userManager;
        public ProductsController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAllProducts()
        {
            
            List<Product> poop = new List<Product>();

            var query = _context.Products;

            foreach (var item in query)
            {
                Product product = new Product();
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

                poop.Add(product);
            }
            return Ok(poop);
        }

        [HttpPut("update/{productid}")]
        public async Task<ActionResult> UpdateProduct([FromRoute] string productid)
        {
            return Ok($"Hello, {productid}");
        }

        [HttpGet("get/{productid}")]
        public async Task<ActionResult> GetProduct([FromRoute] string productid)
        {
            Product product = _context.Products.Where(x => x.Id == productid).FirstOrDefault();

            return Ok(product);
        }
        [HttpPost("create")]
        public async Task<ActionResult> CreateProduct([FromRoute]StringContent value)
        {
            
            Product product = JsonConvert.DeserializeObject<Product>(value.ToString());
            //string message = "poop";

            _context.Add(product);
            _context.SaveChangesAsync();
            return Ok();
            //User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

            //var roles = await _userManager.GetRolesAsync(user);

            //roles.Contains("admin");

            //if (user != null)
            //{
            //    Product newProduct = new Product();

            //    //newProduct.Price = 99.5

            //    _context.Products.Add(newProduct);

            //    _context.SaveChanges();

            //    return Ok(new { message = $"Success! Product with ID {newProduct.Id} was created." });

            //}
            //else
            //{
            //    return Unauthorized(new { message = "user was not found, sorry!" });
            //}
        }


    }
}
