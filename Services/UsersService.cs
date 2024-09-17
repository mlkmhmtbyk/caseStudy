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
        private readonly Logger _logger;

        public UsersService(IConfiguration config, DataContext context, Logger logger)
        {
            _config = config;
            _context = context;
            _logger = logger;
        }
        
        public (User, string) Login(string username, string password)
        {
            try{
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
                    _logger.Log("User not found");
                    throw new Exception("User not found");
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"An error occurred while logging in: {ex.Message}");
                throw;
            }
            
        }

        public void Logout(string username)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == username);
                    if (user != null)
                    {
                        user.IsActive = false;
                        _context.SaveChanges();                
                    }
                    
                    else
                    {
                        _logger.Log("User not found");
                        throw new Exception("User not found");
                    }
            }
            catch (Exception ex)
            {
                _logger.Log($"An error occurred while logging out: {ex.Message}");
                throw;
            }
            
        }

        private string GenerateToken()
        {
            try
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
            catch (Exception ex)
            {
                _logger.Log($"An error occurred while generating token: {ex.Message}");
                throw;
            }
        }
    }
}