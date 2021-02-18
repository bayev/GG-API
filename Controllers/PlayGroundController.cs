using Api.Data;
using Api.Models;
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
    public class PlayGroundController : Controller
    {
        private Context _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public PlayGroundController(Context context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }



        [HttpGet("one")]
        public ActionResult RateCompany([FromBody] RateCompanyModel rateCompanyModel)
        {

            var result = new
            {
                Message = $"You gave {rateCompanyModel.Company} a {rateCompanyModel.Rating} star rating",
            };

            return Ok(result);
        }
    }
}
