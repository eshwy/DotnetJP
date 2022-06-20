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

    public class UserJobDetailsController : ControllerBase
    {
        private readonly JobPortalContext _context;

        public UserJobDetailsController(JobPortalContext context)
        {
            _context = context;
        }

        // GET: api/UserJobDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserJobDetails>>> GetUserJobDetail()
        {
            return await _context.UserJobDetails.ToListAsync();
        }

        // GET: api/UserJobDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<UserJobDetails>>> GetUserJobDetail(int id)
        {
            var userJobDetail = await _context.UserJobDetails.Where(x => x.User_Id == id).ToListAsync();            

            return userJobDetail;
        }

        //// PUT: api/UserJobDetails/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUserJobDetail(int? id, UserJobDetail userJobDetail)
        //{
        //    if (id != userJobDetail.User_Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(userJobDetail).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserJobDetailExists(id))
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

        //// POST: api/UserJobDetails
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<UserJobDetail>> PostUserJobDetail(UserJobDetail userJobDetail)
        //{
        //    _context.UserJobDetail.Add(userJobDetail);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUserJobDetail", new { id = userJobDetail.User_Id }, userJobDetail);
        //}

        //// DELETE: api/UserJobDetails/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUserJobDetail(int? id)
        //{
        //    var userJobDetail = await _context.UserJobDetail.FindAsync(id);
        //    if (userJobDetail == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.UserJobDetail.Remove(userJobDetail);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool UserJobDetailExists(int? id)
        //{
        //    return _context.UserJobDetail.Any(e => e.User_Id == id);
        //}
    }
}
