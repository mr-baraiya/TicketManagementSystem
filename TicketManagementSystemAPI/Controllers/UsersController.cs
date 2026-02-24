using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystemAPI.DTOs;
using TicketManagementSystemAPI.Models;

namespace TicketManagementSystemAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly TicketManagementSystemContext _context;

        public UsersController(TicketManagementSystemContext context)
        {
            _context = context;
        }

        #region GetAllUsers
        [Authorize(Roles = "MANAGER")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion

        #region CreateUser
        [Authorize(Roles = "MANAGER")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDTO model)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    return BadRequest("Email already exists");
                }
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),   
                    RoleId = _context.Roles.FirstOrDefault(r => r.Name == model.Role)?.Id ?? 1 
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok(user);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion

    }
}
