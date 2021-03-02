using Api.Data;
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
    [Authorize]
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
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
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

                        listOfProducts.Add(product);
                    }
                    return Ok(listOfProducts);

                }
                else
                    return NotFound(new { message = "Sorry, no products found" });
            }
            else
                return Unauthorized();
        }

        [HttpPut("update/{productid}")]
        public async Task<ActionResult> UpdateProduct([FromBody] PostProductModel model, string Productid)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                try
                {
                    Product product = _context.Products.Where(x => x.Id == Productid).FirstOrDefault();
                    product.Name = model.Name;
                    product.Price = Convert.ToDecimal(model.Price);
                    product.Weight = model.Weight;
                    product.Description = model.Description;
                    // upload image the proper way
                    product.Image = model.Image;
                    product.Category = model.Category;
                    product.Stock = model.Stock;
                    product.Size = model.Size;
                    product.Brand = model.Brand;

                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = $"Sorry, something happend. {ex.ToString()}" });
                }
            }
            else
                return Unauthorized();
        }

        [HttpGet("get/{productid}")]
        public async Task<ActionResult> GetProduct([FromRoute] string productid)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                Product product = _context.Products.Where(x => x.Id == productid).FirstOrDefault();

                return Ok(product);
            }
            else
                return Unauthorized();
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateProduct([FromBody] PostProductModel model)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                Product product = new Product();

                product.Name = model.Name;
                product.Price = Convert.ToDecimal(model.Price);
                product.Weight = model.Weight;
                product.Description = model.Description;

                // upload image the proper way
                product.Image = model.Image;
                product.Category = model.Category;
                product.Stock = model.Stock;
                product.Size = model.Size;
                product.Brand = model.Brand;

                try
                {
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = $"Sorry, something happend. {ex.ToString()}" });
                }

                return Ok();

            }
            else
            {
                return Unauthorized();
            }
        }
        [HttpDelete("delete/{productid}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] string productid)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                try
                {
                    Product product = _context.Products.Where(x => x.Id == productid).FirstOrDefault();
                    _context.Remove(product);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = $"Sorry, something happened. {ex.ToString()}"});

                }
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }


    }
}
