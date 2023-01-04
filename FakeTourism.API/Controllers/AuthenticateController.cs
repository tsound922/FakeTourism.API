using FakeTourism.API.Dtos;
using Microsoft.AspNetCore.Authorization;
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

namespace FakeTourism.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticateController: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticateController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginDto loginDto) 
        {
            //1 Auth user and password
            //2 create jwt
            /*
             * Header
             * Payload
             * Signagure
             */
            var signinAlgorithm = SecurityAlgorithms.HmacSha256;
            //payload
            //Who owns the resource
            var claims = new[]
            {
                //sub - define the user role
                new Claim(JwtRegisteredClaimNames.Sub, "fake_user_id"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            //signature
            //get private key
            var secretByte = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
            //encrypt private key with symmetric
            var signinKey = new SymmetricSecurityKey(secretByte);
            //validation of the encrypted private key
            var signinCredentials = new SigningCredentials(signinKey, signinAlgorithm);

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signinCredentials
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            //3 return 200 ok status +jwt
            return Ok(tokenStr);
        }
    }
}
