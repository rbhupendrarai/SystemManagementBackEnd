using CarManagementSystem.Data.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;         
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDetail()
        {
            var result = await _userService.GetUsers();
            return Ok(result);

        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var result = await _userService.Login(loginViewModel);
            var userDetail = await _userService.GetUser(loginViewModel);

            var Message = string.Empty;        

            if (result == "Unauthorized")
            {
                return Unauthorized();
            }
            if (result == "NotMatch")
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "UserName Or Password Not Match!" });
            }
            return Ok(new { token =result,user=userDetail});
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
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

        [HttpPut]
        [Route("EditProfile/{id}")]    
        public async Task<IActionResult> EditProfile(string id,EditDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.EditProfile(id,model);

                if (result == true)
                {
                     return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Profile Changed" });

                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "profile Updation failed! Please check user details and try again." });
                }

            }
            return Ok();
        }


    }
}
