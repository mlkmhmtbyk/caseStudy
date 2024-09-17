using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using caseStudy.Data.Models;
using caseStudy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace caseStudy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UsersService _usersService;

        public LoginController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginModel model)
        {
            try 
            {
                var (user, token) = _usersService.Login(model.Username, model.Password);
                if (user != null)
                {
                    return Ok(token);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error while logging in:" + ex.Message);
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout(LoginModel model)
        {
            try
            {
                _usersService.Logout(model.Username);
                return Ok("Logged out successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error while logging out:" + ex.Message);
            }
        }
    }
}