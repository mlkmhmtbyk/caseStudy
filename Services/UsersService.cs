using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using caseStudy.Data;
using caseStudy.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace caseStudy.Services
{
    public class UsersService
    {
        private readonly DataContext _context;
        private IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersService(IConfiguration config, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            
        }
        
        public (User, string) Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                user.IsActive = true;
                _context.SaveChanges();
                var token = GenerateToken();
                return (user, token);
            }
            else 
            {
                throw new Exception("User not found");
            }
        }

        public void Logout(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                user.IsActive = false;
                _context.SaveChanges();                
            }
            else throw new Exception("User not found");
        }

        private string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(300),
              signingCredentials: credentials);

            var token =  new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;

        }
    }
}