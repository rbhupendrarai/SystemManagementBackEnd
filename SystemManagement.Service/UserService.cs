using CarManagementSystem.Data.ViewModel;
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
using SystemManagement.Data.Data;
using SystemManagement.Data.Helper;
using SystemManagement.Data.ViewModel;

namespace SystemManagement.Service
{
    public class UserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SystemManagementDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public UserService(IHttpContextAccessor httpContextAccessor, SystemManagementDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _httpContext = httpContextAccessor;
        }
     
        public async Task<IQueryable> getUsers()
        {

            return from user in _context.Users
                   join userRole in _context.UserRoles
                   on user.Id equals userRole.UserId
                   join role in _context.Roles
                   on userRole.RoleId equals role.Id
                   select new
                   {
                       Id = user.Id,
                       LockDate = user.LockoutEnd,
                       UserName = user.UserName,
                       Email = user.Email,
                       Role = role.Name

                   };
        }

        public async Task<string> Register(RegisterViewModel registerViewModel)
        {
            string Message = string.Empty;
            var user = new IdentityUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var userNameExist = await _userManager.FindByNameAsync(user.UserName);


            if (userNameExist != null)
            {
                return Message = "UserNameExist";
            }

            var userEmailExist = await _userManager.FindByEmailAsync(user.Email);
            if (userEmailExist != null)
            {
                return Message = "EmailExist";
            }
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (!result.Succeeded)
            {
                return Message = "Error";
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Message = "Success";
        }



        public async Task<string> Login(LoginViewModel loginViewModel)
        {
            var Message = string.Empty;
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user != null)
            {
                var password = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

                if (password != true)
                {
                    return Message = "NotMatch";
                }

                var userRole = await _userManager.GetRolesAsync(user);


                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName,user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };
                foreach (var userRoles in userRole)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRoles));
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
                var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);

                return tokenHandler;
            }
            return Message = "Unauthorized";


            //var tokenHandler = new JwtSecurityTokenHandler();
            //var tokenDescription = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //          new Claim(ClaimTypes.Name,user.UserName,user.Id.ToString()),
            //          new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            //    }),
            //    Issuer = _configuration["JWT:ValidIssuer"],
            //    Audience = _configuration["JWT:ValidAudience"],
            //    Expires = DateTime.Now.AddHours(3),
            //    SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            //};            
            //var token = tokenHandler.CreateToken(tokenDescription);
            //var tokenSting = new JwtSecurityTokenHandler().WriteToken(token);


            //List<string> lst = new List<string>(5);

            //lst.Add(user.Id);
            //lst.Add(tokenSting);

            //return lst;
        }
       
    }
}
