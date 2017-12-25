using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers
{
    [Route("v1/token")]
    public class TokenController : Controller
    {
        private readonly AppSettings _settings;

        public TokenController(AppSettings settings)
        {
            _settings = settings;
        }

        [HttpGet("profile")][Authorize]
        public IActionResult Profile()
        {
            return Ok(new {firstName = "mits", favoriteSandwich = "bread"});
        }

        [AllowAnonymous]
        [HttpPost("")]
        public IActionResult CreateToken([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                var p = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.NameId, user?.Name),
                    new Claim(JwtRegisteredClaimNames.Email, user?.Email),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                });
                HttpContext.User = new ClaimsPrincipal(p);

                response = Ok(new { token = tokenString });
            }



            return response;
        }

        private string BuildToken(UserModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_settings.Jwt.Issuer,
                _settings.Jwt.Issuer,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(LoginModel login)
        {
            UserModel user = null;

            if (login.Username == "mitsbits" && login.Password == "Passw0rd")
            {
                user = new UserModel { Name = "mitsbits", Email = "mitsbits@gmail.com" };
            }
            return user;
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private class UserModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime Birthdate { get; set; }
        }
    }
}

