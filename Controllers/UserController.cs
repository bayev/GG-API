using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

        [HttpGet("profile/{id}")]
        public async Task<ActionResult> GetProfile(string Id)
        {

            //User user1 = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            //User user2 = await _userManager.FindByEmailAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            User user = await _userManager.FindByIdAsync(Id);

            //User user = user1 == null ? user2 : user1;

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
        [HttpPut("userupdate")]
        public async Task<ActionResult> UserUpdate([FromBody] RegisterModel model)
        {
            var UserCheck = await _userManager.FindByNameAsync(model.Username);
            var UserMailCheck = await _userManager.FindByEmailAsync(model.Email);

            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            User userEmail = await _userManager.FindByEmailAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email)).Value);

            bool sameEmail = user.Email == model.Email ? true : false;
            bool sameUsername = user.UserName == model.Username ? true : false;

            if (!sameEmail && UserMailCheck != null)
            {
                return BadRequest();
            }
            if (!sameUsername && UserCheck != null)
            {
                return BadRequest();
            }

            //Måste uppdatera claims? för att kunna uppdatera ändrat Username eller lösenord. Det du har när du loggar in är det som gäller hela tiden. Session?
            //User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            //var roles = await _userManager.GetRolesAsync(user);

            if (user is not null)
            {
                user.UserName = model.Username;
                user.NormalizedUserName = model.Username.ToUpper();
                user.Email = model.Email;
                user.NormalizedEmail = model.Email.ToUpper();
                user.EmailConfirmed = true;
                user.FullName = model.FullName;
                user.BillingAdress = model.BillingAddress;
                user.DefaultShippingAddress = model.DefaultShippingAddress;
                user.Country = model.Country;
                user.PhoneNumber = model.Phone;

                await _context.SaveChangesAsync();


                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("default-key-xxxx-aaaa-qqqq-default-key-xxxx-aaaa-qqqq");

                var exp = DateTime.UtcNow.AddDays(1);

                var tokenDescriptor = new SecurityTokenDescriptor
                {

                    // Add your claims to the JWT token
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                            new Claim(ClaimTypes.Name, model.Username),
                            new Claim(ClaimTypes.Email, model.Email)

                    }),
                    Expires = exp,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString, Expires = exp });
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
