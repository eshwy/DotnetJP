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
    public class UserFollowerandFollowedBiesController : ControllerBase
    {
        private readonly JobPortalContext _context;

        public UserFollowerandFollowedBiesController(JobPortalContext context)
        {
            _context = context;
        }

        // GET: api/UserFollowerandFollowedBies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFollowerandFollowedBy>>> GetUserFollowerandFollowedBy()
        {
            return await _context.UserFollowerandFollowedBy.ToListAsync();
        }

        // GET: api/UserFollowerandFollowedBies/5
        [HttpGet("{id}")]        
        public async Task<ActionResult<UserFollowerandFollowedBy>> GetUserFollowerandFollowedBy(int id)
        {
            var userFollowerandFollowedBy = await _context.UserFollowerandFollowedBy.FindAsync(id);

            if (userFollowerandFollowedBy == null)
            {
                return NotFound();
            }

            return userFollowerandFollowedBy;
        }

        

        // POST: api/UserFollowerandFollowedBies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserFollowerandFollowedBy>> PostUserFollowerandFollowedBy(UserFollowerandFollowedBy userFollowerandFollowedBy)
        {
            _context.UserFollowerandFollowedBy.Add(userFollowerandFollowedBy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserFollowerandFollowedBy", new { id = userFollowerandFollowedBy.RowNumber }, userFollowerandFollowedBy);
        }

        //// PUT: api/UserFollowerandFollowedBies/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUserFollowerandFollowedBy(int id, UserFollowerandFollowedBy userFollowerandFollowedBy)
        //{
        //    if (id != userFollowerandFollowedBy.RowNumber)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(userFollowerandFollowedBy).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserFollowerandFollowedByExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}
        //// DELETE: api/UserFollowerandFollowedBies/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUserFollowerandFollowedBy(int id)
        //{
        //    var userFollowerandFollowedBy = await _context.UserFollowerandFollowedBy.FindAsync(id);
        //    if (userFollowerandFollowedBy == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.UserFollowerandFollowedBy.Remove(userFollowerandFollowedBy);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool UserFollowerandFollowedByExists(int id)
        {
            return _context.UserFollowerandFollowedBy.Any(e => e.RowNumber == id);
        }
    }
}
