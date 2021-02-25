using Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class QueryController : Controller
    {
        private Context _context;
        private readonly UserManager<User> _userManager;
        public QueryController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("search/{queryString}")] //querystring istället för all 
        public async Task<ActionResult> SearchProductResult([FromRoute] string queryString)
        {
            try
            {
                var q1 = _context.Products
                .Where(x => x.Category.Contains(queryString) || x.Name.Contains(queryString)).ToList();

                return Ok(q1);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Sorry, something happend. {ex.ToString()}" });
            }
        }
    }
}