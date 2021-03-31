using Api.Data;
using Api.Models;
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


            if (user != null)
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

            var C2Pcheck = _context.CartToProducts
                .Where(x => x.Cart == cart && x.ProductId == productInDb.Id)
                .FirstOrDefault();

            if (C2Pcheck != null)
            {
                C2Pcheck.Amount += 1;
                productInDb.Stock -= 1;
            }
            else
            {
                CartToProduct c2p = new CartToProduct();
                productInDb.Stock -= 1;
                c2p.Amount += 1;
                c2p.ProductId = productInDb.Id;
                c2p.Cart = cart;
                _context.Add(c2p);
            }

            await _context.SaveChangesAsync();

            var allC2P = _context.CartToProducts
                .Where(x => x.Cart == cart).ToList();

            return Ok();
        }
        [HttpDelete("deletefromcart/{c2pId}")]
        public async Task<ActionResult> DeleteFromCart([FromRoute] string c2pId)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

            if (user != null)
            {
                CartToProduct c2p = _context.CartToProducts.Where(x => x.Id == c2pId).FirstOrDefault();
                Product product = _context.Products.Where(x => x.Id == c2p.ProductId).FirstOrDefault();

                if (c2p != null && product != null)
                {
                    product.Stock += c2p.Amount; //Tar bort samtliga produkter 

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
        [HttpPut("updateQuantity/{PlusMinus}/{c2pIdUpdate}")]
        public async Task<ActionResult> UpdateQuantity([FromRoute] string PlusMinus, string c2pIdUpdate)
        {
            bool input = PlusMinus == "true" ? true : false;

            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

            Cart cart = _context.Carts
            .Where(x => x.UserId == user.Id)
            .FirstOrDefault();

            CartToProduct c2p = _context.CartToProducts.Where(x => x.Id == c2pIdUpdate).FirstOrDefault();
            Product product = _context.Products.Where(x => x.Id == c2p.ProductId).FirstOrDefault();

      
            //Vill lägga till
            if (input && product.Stock > 0)
            {
                c2p.Amount += 1;
                product.Stock -= 1;
                await _context.SaveChangesAsync();
                return Ok("Increased");
            }
            //Ta bort en produkt eller hela CartToProduct(C2P).
            else if(!input)
            {
                if (c2p.Amount == 1)
                {
                    product.Stock += 1;

                    _context.Remove(c2p);
                    await _context.SaveChangesAsync();
                    return Ok("Deleted");
                }
                else
                {
                    c2p.Amount -= 1;
                    product.Stock += 1;
                    await _context.SaveChangesAsync();
                    return Ok("Reduced");
                }
            }
            else
            {
                return BadRequest("Not in stock");
            }
        }
        [HttpPost("placeOrder")]
        public async Task<ActionResult> PlaceOrder([FromBody] PostOrderModel postOrderModel)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);


            Cart cart = _context.Carts
                .Where(x => x.UserId == user.Id)
                .FirstOrDefault();

            var c2pList = _context.CartToProducts
                .Where(x => x.Cart == cart)
                .ToList();

            Order order = new Order()
            {
                UserId = user.Id,
                Amount = 55,
                OrderEmail = user.Email,
                OrderStatus = "Plockas",
                PaymentMethod = postOrderModel.paymentMethod,
                OrderDate = DateTime.Now
            };
            _context.Add(order);
            
            foreach (var item in c2pList)
            {
                var product = _context.Products
                    .Where(x => x.Id == item.ProductId)
                    .FirstOrDefault();

                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderId = order.Id,
                    ProductName = product.Name,
                    Quantity = item.Amount,
                    Price = (product.Price * item.Amount),
                };
                order.Amount += orderDetail.Price;
                order.OrderDetails.Add(orderDetail);
                _context.Add(orderDetail);
                _context.Remove(item);
            }

            await _context.SaveChangesAsync();
            return Ok("Order Placed");
        }
        [HttpGet("getOrder/{IdUser}")]
        public async Task<ActionResult> GetOrder([FromRoute] string IdUser)
        {
            var order = _context.Orders
                .Where(x => x.UserId == IdUser)
                .OrderByDescending(x => x.OrderDate)
                .FirstOrDefault();

            return Ok(order);
        }
    }
}
