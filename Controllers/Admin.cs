using Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var orders = _context.Orders.ToList();

            return Ok(orders);
        }
    }
}
