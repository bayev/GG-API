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
            return null;
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
        public async Task<ActionResult> CreateProduct()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

            //var roles = await _userManager.GetRolesAsync(user);

            //roles.Contains("admin");

            if (user != null)
            {
                Product newProduct = new Product();

                //newProduct.Price = 99.5

                _context.Products.Add(newProduct);

                _context.SaveChanges();

                return Ok(new { message = $"Success! Product with ID {newProduct.Id} was created." });

            }
            else
            {
                return Unauthorized(new { message = "user was not found, sorry!" });
            }
        }


    }
}
