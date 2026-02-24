using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TicketManagementSystemAPI.DTOs;
using TicketManagementSystemAPI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TicketManagementSystemAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {

        private readonly TicketManagementSystemContext _context;

        public TicketsController(TicketManagementSystemContext context)
        {
            _context = context;
        }

        #region CreateTicket
        [Authorize(Roles = "USER,MANAGER")]
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] Ticket ticket)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return BadRequest("Invalid user");
                }
                ticket.CreatedBy = int.Parse(userId);
                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region GetTickets
        [Authorize(Roles = "USER,MANAGER,SUPPORT")]
        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                if (userId == null || userRole == null)
                {
                    return BadRequest("Invalid user");
                }
                else if (userRole == "MANAGER")
                {
                    var tickets = await _context.Tickets.ToListAsync();
                    return Ok(tickets);
                }
                else if (userRole == "SUPPORT")
                {
                    var tickets = await _context.Tickets.Where(t => t.AssignedTo == int.Parse(userId)).ToListAsync();
                    return Ok(tickets);
                }
                else if (userRole == "USER")
                {
                    var tickets = await _context.Tickets.Where(t => t.CreatedBy == int.Parse(userId)).ToListAsync();
                    return Ok(tickets);
                }
                else
                {
                    return BadRequest("Invalid user role");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region AssignTicket
        [Authorize(Roles = "MANAGER, SUPPORT")]
        [HttpPatch("{id}/assign")]
        public async Task<IActionResult> AssignTicket(int id, AssignDto model)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket == null)
                {
                    return NotFound("Ticket not found");
                }
                ticket.AssignedTo = model.userId;
                await _context.SaveChangesAsync();
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region UpdateTicketStatus
        [Authorize(Roles = "MANAGER, SUPPORT")]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTicketStatus(int id, UpdateStatusDto model)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket == null)
                {
                    return NotFound("Ticket not found");
                }
                ticket.Status = model.status;
                await _context.SaveChangesAsync();
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region DeleteTicket
        [Authorize(Roles = "MANAGER")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket == null)
                {
                    return NotFound("Ticket not found");
                }
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

    }
}
