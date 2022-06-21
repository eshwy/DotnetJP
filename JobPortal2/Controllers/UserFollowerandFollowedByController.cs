using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal2.Models;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserFollowerandFollowedByController : ControllerBase
    {
        private readonly JobPortalContext _context;

        public UserFollowerandFollowedByController(JobPortalContext context)
        {
            _context = context;
        }

        // GET: api/UserFollowerandFollowedBy
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<UserFollowerandFollowedBy>>> GetUserFollowerandFollowedBy()
        {
            return await _context.UserFollowerandFollowedBy.ToListAsync();
        }

        // GET: api/UserFollowerandFollowedBy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<UserFollowerandFollowedBy>>> GetUserFollowerandFollowedBy(int? id)
        {
            var userFollowerandFollowedBy = await _context.UserFollowerandFollowedBy.Where(c => c.UserId == id).ToListAsync();

            if (userFollowerandFollowedBy == null)
            {
                return NotFound();
            }

            return userFollowerandFollowedBy;
        }        

        // POST: api/UserFollowerandFollowedBy
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserFollowerandFollowedBy>> PostUserFollowerandFollowedBy(UserFollowerandFollowedBy userFollowerandFollowedBy)
        {
            if (!UserFollowerandFollowedByExists(userFollowerandFollowedBy))
            {
                _context.UserFollowerandFollowedBy.Add(userFollowerandFollowedBy);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUserFollowerandFollowedBy", new { id = userFollowerandFollowedBy.RowNumber }, userFollowerandFollowedBy);
            }
            return BadRequest();            
        }

        // DELETE: api/UserFollowerandFollowedBy/5
        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> DeleteUserFollowerandFollowedBy(UserFollowerandFollowedBy UserData)
        {
            var userFollowerandFollowedBy =  _context.UserFollowerandFollowedBy.Where(c =>c.UserId == UserData.UserId && c.FollowerId==UserData.FollowerId).FirstOrDefault();
            
            if (userFollowerandFollowedBy == null)
            {
                return NotFound();
            }

            _context.UserFollowerandFollowedBy.Remove(userFollowerandFollowedBy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserFollowerandFollowedByExists(UserFollowerandFollowedBy ubb)
        {
            return _context.UserFollowerandFollowedBy.Any(e => e.UserId== ubb.UserId && e.FollowerId==ubb.FollowerId);
        }
    }
}
