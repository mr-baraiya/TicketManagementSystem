using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketManagementSystemAPI.DTOs;
using TicketManagementSystemAPI.Models;

namespace TicketManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TicketManagementSystemContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(TicketManagementSystemContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        #region Login

        [HttpPost("login")]
        public IActionResult Login(LoginDto model)
        {
            try
            {
                var user = _context.Users
                    .FirstOrDefault(x => x.Email == model.Email);

                if (user == null)
                    return Unauthorized("Invalid Email");

                bool isValid = BCrypt.Net.BCrypt.Verify(
                    model.Password,
                    user.Password
                );

                if (!isValid)
                    return Unauthorized("Invalid Password");

                var jwtSettings = _configuration.GetSection("Jwt");

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, _context.Roles.FirstOrDefault(r => r.Id == user.RoleId)?.Name ?? "USER")
            };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["Key"])
                );

                var token = new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(
                        Convert.ToDouble(jwtSettings["TokenExpiryMinutes"])
                    ),
                    signingCredentials: new SigningCredentials(
                        key, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion

    }
}
