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
    public class AuthController : ControllerBase
    {

        private Context _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(Context context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Use [AllowAnonymous] for methods anyone can use, Such as login and register. 
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {

            User user = model.User.Contains("@") ? await _userManager.FindByEmailAsync(model.User) : await _userManager.FindByNameAsync(model.User);

            if (user != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (signInResult.Succeeded)
                {

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes("default-key-xxxx-aaaa-qqqq-default-key-xxxx-aaaa-qqqq");

                    var exp = DateTime.UtcNow.AddDays(1);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {


                        // Add your claims to the JWT token
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Email, user.Email)


                        }),
                        Expires = exp,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    return Ok(new { Token = tokenString, Expires = exp});
                }
                else
                {
                    return Ok("No user or password matched, try again.");
                }
            }
            else
            {
                return Ok("No such user exists");
            }
        
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel model)
        {
            // Always better with a global try catch

            User newUser = new User()
            {
                Email = model.Email,
                UserName = model.Username,
                // Its always best practice to have some form of verification. this is off for simplicity
                EmailConfirmed = false,
                // Set your customs
                //MyProperty = 13
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                User user = await _userManager.FindByNameAsync(newUser.UserName);

                if (user is not null)
                {
                    await _userManager.AddToRoleAsync(newUser, "User");

                    // Remember to set your custom data and relationships here

                    UserSettings settings = new UserSettings()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DarkMode = true,
                        User = user
                    };

                    UserGDPR gdpr = new UserGDPR()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UseMyData = false,
                        User = user
                    };
                    UserInfo info = new UserInfo()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FullName = model.FullName,
                        BillingAddress = model.BillingAddress,
                        DefaultShippingAddress = model.DefaultShippingAddress,
                        Country = model.Country,
                        Phone = model.Phone,
                        User = user
                    };

                    // Add it to the context
                    _context.UserSettings.Add(settings);
                    _context.UserGDPR.Add(gdpr);
                    _context.UserInfo.Add(info);

                    // Save the data
                    _context.SaveChanges();

                    return Ok(new { result = $"User {model.Username} has been created", Token = "xxx" });
                }
                else
                {
                    return Ok(new { message = "Registration failed for unknown reasons, please try again." });
                }
            }
            else
            {
                StringBuilder errorString = new StringBuilder();

                foreach (var error in result.Errors)
                {
                    errorString.Append(error.Description);
                }

                return Ok(new { result =  $"Register Fail: {errorString.ToString()}"});
            }

        }

        [AllowAnonymous]
        [HttpPost("changepassword")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {

            if (model.NewPassword == model.ConfirmNewPassword)
            {
                User user = ToolBox.IsValidEmail(model.User) ? await _userManager.FindByEmailAsync(model.User) : await _userManager.FindByNameAsync(model.User);

                if (user is not null)
                {
                    if (await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
                    {
                        await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                        return Ok(new { message = "Password has been updated." });
                    }
                    else
                    {
                        return Ok(new { message = $"Your password is incorrect. ({user.AccessFailedCount}) failed attempts." });
                    }
                }
                else
                {
                    return Ok(new { message = "No such user found." });
                }
            }
            else
            {
                return Ok(new { message = "Your password does not match." });
            }
        
        }

        [HttpPost("adminregister")] 
        public async Task<ActionResult> AdminRegister([FromBody] RegisterAdminModel model)
        {
            User newUser = new User()
            {
                Email = model.Email,
                UserName = model.Username,
                EmailConfirmed = false,
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                User user = await _userManager.FindByNameAsync(newUser.UserName);

                if (user is not null)
                {
                    await _userManager.AddToRoleAsync(newUser, "root");

                    UserSettings settings = new UserSettings()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DarkMode = true,
                        User = user
                    };

                    UserGDPR gdpr = new UserGDPR()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UseMyData = false,
                        User = user
                    };

                    // Add it to the context
                    _context.UserSettings.Add(settings);
                    _context.UserGDPR.Add(gdpr);

                    // Save the data
                    _context.SaveChanges();

                    return Ok(new { result = $"User {model.Username} has been created"});
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                StringBuilder errorString = new StringBuilder();

                foreach (var error in result.Errors)
                {
                    errorString.Append(error.Description);
                }
                return NotFound();
                //return Ok(new { result = $"Register Fail: {errorString.ToString()}" });
            }

        }
        [HttpDelete("admindelete")]
        public async Task<ActionResult> AdminDelete()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                try
                {
                    _context.Remove(user);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = $"Sorry, something happened. {ex.ToString()}" });

                }
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }

}
