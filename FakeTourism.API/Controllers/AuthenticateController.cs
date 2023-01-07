using FakeTourism.API.Dtos;
using FakeTourism.API.Models;
using FakeTourism.API.Services;
using Microsoft.AspNetCore.Authorization;
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

namespace FakeTourism.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticateController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITouristRouteRepository _touristRouteRepository;

        public AuthenticateController(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITouristRouteRepository touristRouteRepository
            ) 
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _touristRouteRepository = touristRouteRepository;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto) 
        {
            //1 Auth user and password
            var loginResult = await _signInManager.PasswordSignInAsync(
                loginDto.Email,
                loginDto.Password,
                false,
                false
            );
            if (!loginResult.Succeeded) 
            {
                return BadRequest("Login failed, please check your login detail");
            }

            var userDetail = await _userManager.FindByNameAsync(loginDto.Email);


            //2 create jwt
            /*
             * Header
             * Payload
             * Signagure
             */
            var signinAlgorithm = SecurityAlgorithms.HmacSha256;
            //payload
            //Who owns the resource
            var claims = new List<Claim>
            {
                //sub - define the user role
                new Claim(JwtRegisteredClaimNames.Sub, userDetail.Id),
                /*new Claim(ClaimTypes.Role, "Admin")*/
            };
            var roleNames = await _userManager.GetRolesAsync(userDetail);
            foreach (var roleName in roleNames) 
            {
                var roleClaim = new Claim(ClaimTypes.Role, roleName);
                claims.Add(roleClaim);
            }

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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) 
        {
            //1 use username create user object
            var user = new ApplicationUser()
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
            };

            //2 hash password to protect user information
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) 
            {
                return BadRequest("User register failed");
            }

            //3 initialize shopping cart for new user register
            var shoppingCart = new ShoppingCart()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id
            };

            await _touristRouteRepository.CreateShoppingCart(shoppingCart);
            await _touristRouteRepository.SaveAsync();
            //4 return
            return Ok("User successfully registered");
        }

    }
}
