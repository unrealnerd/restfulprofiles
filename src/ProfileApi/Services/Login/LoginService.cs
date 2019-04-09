using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProfileApi.Models;

namespace ProfileApi.Services
{
    public class LoginService : ILoginService
    {
        //TODO: Move this data to DB and store encrypted
        private List<User> _users = new List<User>
        {
            new User {
                Id = 1,
                FirstName = "arjun",
                LastName = "shetty",
                Username = "arjun",
                Password = "password"
                }
        };

        private readonly Settings _settings;

        public LoginService(IOptions<Settings> settings)
        {
            _settings = settings.Value;
        }


        public string Login(string username, string password)
        {
            var user = _users.SingleOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
                return null;

            var tokenString = GenerateJSONWebToken(user);

            return tokenString;
        }


        private string GenerateJSONWebToken(User user)
        {
            //secret token and the algorithm to use
            var secretToken = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretToken));
            var credentials = new SigningCredentials(secretToken, SecurityAlgorithms.HmacSha256);

            // creating a with one day validity. TODO: Make this cocnfigurable and reduce validity
            var token = new JwtSecurityToken(_settings.TokenIssuer,
            null,
            null,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}