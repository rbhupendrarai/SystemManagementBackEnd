using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SystemManagement.Data.Helper;
using SystemManagement.Data.ViewModel;
using SystemManagement.Service;

namespace SystemManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        public UserController(IConfiguration configuration,UserService userService, UserManager<IdentityUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
          
            _configuration = configuration;
        }
    
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromQuery]LoginViewModel loginViewModel)
        {
            var result = await _userService.Login(loginViewModel);

            var Message = string.Empty;

            //var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            //if (user != null)
            //{
            //    var password = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
            //    if (password != true)
            //    {
            //         Message = "NotMatch";
            //    }
            //    var userRole = await _userManager.GetRolesAsync(user);
            //    var authClaims = new List<Claim>
            //    {
            //        new Claim(ClaimTypes.Name,user.UserName),
            //        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            //    };
            //    foreach (var userRoles in userRole)
            //    {
            //        authClaims.Add(new Claim(ClaimTypes.Role, userRoles));
            //    }

            //    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            //    var token = new JwtSecurityToken(
            //        issuer: _configuration["JWT:ValidIssuer"],
            //        audience: _configuration["JWT:ValidAudience"],
            //        expires: DateTime.Now.AddHours(3),
            //        claims: authClaims,
            //        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));    

            //    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });


            //}
           // return Unauthorized();

            if (result == "Unauthorized")
            {
                return Unauthorized();
            }
            if (result == "NotMatch")
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Password Not Match!" });
            }

            return Ok(new { token =result});
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            var result = await _userService.Register(registerViewModel);
            if (result == "UserNameExist")
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            }
            if (result == "EmailExist")
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email already exists!" });
            }
        
            if (result == "Error")
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            }
            return Ok(new Response { Status = "Success", Message = "created successfully!" });
        }
       
    }
}
