using Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class Admin : Controller
    {
        private Context _context;
        private readonly UserManager<User> _userManager;

        public Admin(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;

        }



        [HttpGet("allOrders")]
        public async Task<ActionResult> GetAllOrders([FromRoute] string IdUser)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                try
                {
                    var orders = _context.Orders.ToList();
                    return Ok(orders);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            else
            {
                return Unauthorized();
            }

        }
        [HttpGet("GetOrderStatus/{orderId}")]
        public async Task<ActionResult> GetOrderStatus([FromRoute] string orderId)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                try
                {
                    var order = _context.Orders
                        .Where(x => x.Id == orderId)
                        .FirstOrDefault();


                    var orderDetails = _context.OrderDetails
                        .Where(x => x.OrderId == order.Id).ToList();

                    order.OrderDetails = orderDetails;


                    return Ok(order);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);

                }
            }
            return Ok();
        }

        [HttpPut("ChangeOrderStatus/{orderId}")]
        public async Task<ActionResult> ChangeOrderStatus([FromRoute] string orderId)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                try
                {
                    var order = _context.Orders
                        .Where(x => x.Id == orderId)
                        .FirstOrDefault();

                    order.OrderStatus = "Skickad";
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Orderstatus har ändradrats till skickad!" });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);

                }

            }
            return Ok();
        }
    }



}
