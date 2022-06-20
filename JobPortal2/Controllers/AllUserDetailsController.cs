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

    public class AllUserDetailsController : ControllerBase
    {
        private readonly JobPortalContext _context;

        public AllUserDetailsController(JobPortalContext context)
        {
            _context = context;
        }

        // GET: api/AllUserDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPersonalDetailsTable>>> GetAllUserDetails()
        {
            return await _context.UserPersonalDetailsTable.ToListAsync();
        }

        // GET: api/AllUserDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserPersonalDetailsTable>> GetAllUserDetails(string id)
        {
            var allUserDetails = await _context.UserPersonalDetailsTable.FindAsync(id);
            return allUserDetails;
        }

        //// PUT: api/AllUserDetails/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAllUserDetails(string id, UserPersonalDetailsTable allUserDetails)
        //{
        //    if (id != allUserDetails.Email)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(allUserDetails).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AllUserDetailsExists(id))
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

        //// POST: api/AllUserDetails
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<UserPersonalDetailsTable>> PostAllUserDetails(UserPersonalDetailsTable allUserDetails)
        //{
        //    _context.UserPersonalDetailsTable.Add(allUserDetails);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (AllUserDetailsExists(allUserDetails.Email))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetAllUserDetails", new { id = allUserDetails.Email }, allUserDetails);
        //}

        //// DELETE: api/AllUserDetails/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteAllUserDetails(string id)
        //{
        //    var allUserDetails = await _context.UserPersonalDetailsTable.FindAsync(id);
        //    if (allUserDetails == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.UserPersonalDetailsTable.Remove(allUserDetails);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool AllUserDetailsExists(string id)
        //{
        //    return _context.UserPersonalDetailsTable.Any(e => e.Email == id);
        //}
    }
}
