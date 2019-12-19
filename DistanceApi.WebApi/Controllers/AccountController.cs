using DistanceApi.Application.Common.Interfaces;
using DistanceApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DistanceApi.WebApi.Controllers
{
    public class AccountController : BaseApiController
    {
        private IConfiguration _configuration;
        private ICurrentUserService _currentUserService;
        private UserManager<User> _userManager;

        public AccountController(IConfiguration configuration,
            ICurrentUserService currentUserService,
            UserManager<User> userManager)
        {
            _configuration = configuration;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterVm registerVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Email = registerVm.Email,
                UserName = registerVm.Email,
                Name = registerVm.Name
            };
            
            var result = await _userManager.CreateAsync(user, registerVm.Password);

            if (result.Succeeded)
                return Ok();

            return BadRequest(result.Errors.Select(x => x.Description));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login([FromBody] LoginVm loginVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginVm.Email);
            if(user == null)
            {
                ModelState.AddModelError("", "user not found");
                return BadRequest(ModelState);
            }

            var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginVm.Password);
            if(result == PasswordVerificationResult.Success)
            {
                return Ok(GenerateJwtToken(user));
            }

            ModelState.AddModelError("", "wrong pass");
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<ActionResult<ProfileVm>> Profile()
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            var profile = new ProfileVm
            {
                Email = user.Email,
                Name = user.Name
            };

            return Ok(profile);
        }

        private string GenerateJwtToken(User user)
        {
            //اطلاعاتی که نیازه در توکن ذخیره بشه
            var claims = new List<Claim>
            {                            
                //آی دی منحصر به فرد
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                //آی دی اصلی یوزر
                new Claim(ClaimTypes.NameIdentifier, user.Id),



            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWT:JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JWT:JwtIssuer"],
                _configuration["JWT:JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

    public class ProfileVm
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
    public class RegisterVm
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
    }

    public class LoginVm
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
    }

}
