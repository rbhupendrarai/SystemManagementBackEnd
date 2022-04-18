using CarManagementSystem.Data.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        
        [Authorize(Roles = "Admin")]
        [Route("GetUserDetail")]
        public IActionResult GetUserDetail()
        {
            var result = _userService.GetUsers();
            return Ok(result);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var result = await _userService.Login(loginViewModel);
            var userDetail = _userService.LoginUserDetail(loginViewModel);

            var Message = string.Empty;        

            if (result.Equals("Unauthorized"))
            {
                return Unauthorized();
            }
            if (result.Equals("NotMatch"))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "UserName Or Password Not Match!" });
            }
            return Ok(new { token =result,user=userDetail});
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("ActiveDeactiveUser/{id}")]
        public async Task<IActionResult> ActiveDeactiveUser(string id)
        {
            try
            {
                var user = await _userService.GetUserByID(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
                
                return StatusCode(StatusCodes.Status208AlreadyReported, new Response { Status = "Error", Message = "Email already exists!" });
            }
        
            if (result == "Error")
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            }
            return Ok(new Response { Status = "Success", Message = "created successfully!" });
        }

        [HttpPut]
        [Authorize]
        [Route("EditProfile/{id}")]    
        public async Task<IActionResult> EditProfile(string id,EditDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.EditProfile(id,model);

                if (result == "success")
                {
                     return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Profile Changed" });

                }
                if (result == "password")
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Password Change Faild" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "profile Updation failed! Please check user details and try again." });
                }

            }
            return Ok();
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("RemoveUser/{id}")]
        public IActionResult RemoveUser(string id)
        {
            try
            {
                _userService.DeleteUser(id);
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "User Deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetUserFilters")]
        public ActionResult GetUserFilters(string search)
        {
            try
            {
                var query = _userService.GetUsers();
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(c => c.UserName.Contains(search) || c.Email.Contains(search) || c.Role.Contains(search));
                }
                return Ok(query);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
