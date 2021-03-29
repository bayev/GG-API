using Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json;
using Api.Models;

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

        [HttpGet("GetCart")]
        public async Task<ActionResult> GetAllProductsInCart()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

            Cart cart = _context.Carts
                .Where(x => x.UserId == user.Id)
                .FirstOrDefault();

            
            if(user != null)
            {

                if (cart == null)
                {
                    cart = new Cart();
                    cart.UserId = user.Id;
                    cart.CartToProducts = new List<CartToProduct>();
                    _context.Add(cart);

                    await _context.SaveChangesAsync();
                }
                var allC2P = _context.CartToProducts
                .Where(x => x.Cart == cart).ToList();

                var C2PList = new List<c2aReturnModel>();

                foreach (var c2p in allC2P)
                {
                    var product = _context.Products.Where(x => x.Id == c2p.ProductId).FirstOrDefault();
                    c2aReturnModel c2aRM = new c2aReturnModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Weight = product.Weight,
                        Description = product.Description,
                        Image = product.Image,
                        Category = product.Category,
                        CreateDate = product.CreateDate,
                        Size = product.Size,
                        Brand = product.Brand,
                        Amount = c2p.Amount,
                        c2pID = c2p.Id
                    };
                    C2PList.Add(c2aRM);
                }


                if (C2PList.Count != 0)
                    return Ok(C2PList);
                else
                    return BadRequest("cart empty");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("addtocart")]
        public async Task<ActionResult> CreateCartToProduct([FromBody] string productId)
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

            CartToProduct c2p = new CartToProduct();
            productInDb.Stock -= 1;
            c2p.Amount += 1;
            c2p.ProductId = productInDb.Id;
            c2p.Cart = cart;
            _context.Add(c2p);
            await _context.SaveChangesAsync();

            var allC2P = _context.CartToProducts
                .Where(x => x.Cart == cart).ToList();

            return Ok();
        }
        [HttpDelete("deletefromcart/{c2pId}")]
        public async Task<ActionResult> DeleteFromCart([FromRoute] string c2pId)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

            if(user != null)
            {
                CartToProduct c2p = _context.CartToProducts.Where(x => x.Id == c2pId).FirstOrDefault();
                Product product = _context.Products.Where(x => x.Id == c2p.ProductId).FirstOrDefault();
                if(c2p != null && product != null)
                {
                    product.Stock += 1;

                    _context.Remove(c2p);
                    await _context.SaveChangesAsync();
                    return Ok("Deleted");
                }
                else
                {
                    return BadRequest("c2p or product not found");
                }
            }
            else
            {
                return NotFound("User not found");
            }
        }
    }
}
