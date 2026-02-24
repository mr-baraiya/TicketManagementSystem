using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Xml.Linq;
using TicketManagementSystemAPI.Models;

namespace TicketManagementSystemAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly TicketManagementSystemContext _context;

        public CommentsController(TicketManagementSystemContext context)
        {
            _context = context;
        }

        #region  AddCommentToTicket

        [Authorize(Roles = "USER,MANAGER,SUPPORT")]
        [HttpPost("/api/tickets/{id}/comments")]
        public async Task<IActionResult> AddCommentToTicket(int id, [FromBody] string commentText)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return BadRequest("Invalid user");
                }
                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket == null)
                {
                    return NotFound("Ticket not found");
                }
                var comment = new TicketComment
                {
                    TicketId = id,
                    UserId = int.Parse(userId),
                    Comment = commentText,
                    CreatedAt = DateTime.UtcNow
                };
                _context.TicketComments.Add(comment);
                await _context.SaveChangesAsync();
                return Ok(comment);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        #endregion

        #region ListCommentsForTicket

        [Authorize(Roles = "USER,MANAGER,SUPPORT")]
        [HttpGet("/api/tickets/{id}/comments")]
        public async Task<IActionResult> ListCommentsForTicket(int id)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket == null)
                {
                    return NotFound("Ticket not found");
                }
                var comments = await _context.TicketComments.Where(c => c.TicketId == id).ToListAsync();
                return Ok(comments);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        #endregion

        #region EditComment
        [Authorize(Roles = "USER,MANAGER")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> EditComment(int id, [FromBody] string newComment)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                if (userId == null || userRole == null)
                {
                    return BadRequest("Invalid user");
                }
                var comment = await _context.TicketComments.FindAsync(id);
                if (comment == null)
                {
                    return NotFound("Comment not found");
                }
                if (userRole == "MANAGER" || (userRole == "USER" && comment.UserId == int.Parse(userId)))
                {
                    comment.Comment = newComment;
                    await _context.SaveChangesAsync();
                    return Ok("Comment updated successfully");
                }
                else
                {
                    return Forbid("You are not authorized to edit this comment");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        #endregion

        #region DeleteComment
        [Authorize(Roles = "USER,MANAGER")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                if (userId == null || userRole == null)
                {
                    return BadRequest("Invalid user");
                }
                var comment = await _context.TicketComments.FindAsync(id);
                if (comment == null)
                {
                    return NotFound("Comment not found");
                }
                if (userRole == "MANAGER" || (userRole == "USER" && comment.UserId == int.Parse(userId)))
                {
                    _context.TicketComments.Remove(comment);
                    await _context.SaveChangesAsync();
                    return Ok("Comment deleted successfully");
                }
                else
                {
                    return Forbid("You are not authorized to delete this comment");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        #endregion

    }
}
