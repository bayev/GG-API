using Api.Data;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class CartController : Controller
    {
        private Context _context;
        private readonly UserManager<User> _userManager;
        public CartController(Context context, UserManager<User> userManager)
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

        [HttpPost("addtocart")]
        public async Task<ActionResult> CreateProduct([FromBody] string productId)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

            var productInDb = _context.Products
                .Where(x => x.Id == productId)
                .FirstOrDefault();

            if (productInDb.Stock == 0)
            {
                return BadRequest();
            }


            Cart cart = _context.Carts
            .Where(x => x.UserId == user.Id)
            .FirstOrDefault();

            if (cart == null)
            {
                cart = new Cart();
                cart.UserId = user.Id;
                cart.CartToProducts = new List<CartToProduct>();
                _context.Add(cart);

                await _context.SaveChangesAsync();
            }

            CartToProduct c2a = new CartToProduct();
            productInDb.Stock -= 1;
            c2a.Amount += 1;
            c2a.ProductId = productInDb.Id;
            c2a.Cart = cart;
            _context.Add(c2a);
            await _context.SaveChangesAsync();

            var allC2P = _context.CartToProducts
                .Where(x => x.Cart == cart).ToList();

            return Ok();
        }
    }
}
