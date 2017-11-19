using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ServiceAPI.Services;
using ServiceAPI.Dtos;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using ServiceAPI.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using ServiceAPI.Entities;
using Microsoft.AspNetCore.Authorization;

namespace ServiceAPI.Controllers
{
    [Authorize]
    [Route("auth")]
    public class AuthController : Controller
    {
        private IAuthService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AuthController(
            IAuthService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserDto userDto)
        {
            var user = _userService.Authenticate(userDto.RegistrationNumber, userDto.Password, userDto.Role);

            if (user == null) { return (Unauthorized()); }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return (Ok(new
            {
                Id = user.Id,
                RegistrationNumber = user.RegistrationNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            }));
        }
    }
}
