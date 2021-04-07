using Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class SalesController : ControllerBase
    {

        private Context _context;
        private readonly UserManager<User> _userManager;
        public SalesController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("allSoldProducts")]
        public async Task<ActionResult> GetAllSoldProducts()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var allSold = _context.Sales
                    .Sum(x => x.AmountSold);

                return Ok(allSold);
            }
            else
            {
                return Unauthorized(new { message = $"Du har inte behörighet att komma åt denna informationen" });
            }
        }

        [HttpGet("allOrders")]
        public async Task<ActionResult> GetAllSales()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var orders = _context.Orders.Count();

                return Ok(orders);
            }
            else
            {
                return Unauthorized(new { message = $"Du har inte behörighet att komma åt denna informationen" });
            }
        }
        [HttpGet("totalPrice")]

        public async Task<ActionResult> GetTotalPrice()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var money = _context.Orders
                    .Sum(x => x.Amount);

                    return Ok(money);
            }
            else
            {
                return Unauthorized(new { message = $"Du har inte behörighet att komma åt denna informationen" });
            }
        }
        [HttpGet("totalMembers")]

        public async Task<ActionResult> totalMembers()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var members = _context.UserRoles
                    .Where(x => x.RoleId.Contains("user"))
                    .Count();
   
                return Ok(members);
            }
            else
            {
                return Unauthorized(new { message = $"Du har inte behörighet att komma åt denna informationen" });
            }
        }

    }
}
