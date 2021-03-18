using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Models;
using Api.Data;

namespace Api.Controllers
{

    [Route("[controller]")]
    [ApiController]
    // Give it the [Authorize] attribute. It will bypass the autentication without it.  
    [Authorize]
    public class UserController : ControllerBase
    {

        private Context _context;
        private readonly UserManager<User> _userManager;

        public UserController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("profile")]
        public async Task<ActionResult> GetProfile()
        {

            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

            if (user != null)
            {
                return Ok(user); 
                    //Ok(new { result = _context.Users.Where(x => x.Id == user.Id).FirstOrDefault() });

            }
            else
            {
                return StatusCode(404, new { message = "User does not exist" });
            }
        }

        [HttpGet("toggleGDPR")]
        public async Task<ActionResult> ToggleGDPR()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

            if (user != null)
            {
                UserGDPR gdpr = _context.UserGDPR.Where(x => x.User == user).FirstOrDefault();

                gdpr.UseMyData = !gdpr.UseMyData;

                _context.SaveChanges();

                return Ok(gdpr.UseMyData);

                //return Ok(new { message = $"GDPR has been toggled to {user.GDPR.UseMyData}" });
            }
            else
            {
                return StatusCode(404, new { message = "User does not exist" });
            }
        }

        [HttpDelete("userdelete")]
        public async Task<ActionResult> UserDelete()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

                try
                {
                    _context.Remove(user);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    return BadRequest(ex);

                }
                return Ok();

        }
    }
}
