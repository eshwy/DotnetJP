using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal2.Models;

namespace JobPortal2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoretedProfilesController : ControllerBase
    {
        private readonly JobPortalContext _context;

        public SoretedProfilesController(JobPortalContext context)
        {
            _context = context;
        }

        // GET: api/SoretedProfiles
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<SoretedProfiles>>> GetSoretedProfiles()
        //{
        //    return await _context.SoretedProfiles.ToListAsync();
        //}

        // GET: api/SoretedProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<SoretedProfiles>>> GetSoretedProfiles(int id)
        {
            var soretedProfiles = await _context.SoretedProfiles.Where(c => c.SortedBy == id).ToListAsync();

            if (soretedProfiles == null)
            {
                return NotFound();
            }

            return soretedProfiles;
        }

        // PUT: api/SoretedProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutSoretedProfiles(int? id, SoretedProfiles soretedProfiles)
        //{
        //    if (id != soretedProfiles.RowNumber)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(soretedProfiles).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SoretedProfilesExists(id))
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

        // POST: api/SoretedProfiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SoretedProfiles>> PostSoretedProfiles(SoretedProfiles soretedProfiles)
        {
            _context.SoretedProfiles.Add(soretedProfiles);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSoretedProfiles", new { id = soretedProfiles.RowNumber }, soretedProfiles);
        }

        // DELETE: api/SoretedProfiles/5
        [HttpDelete]
        public async Task<IActionResult> DeleteSoretedProfiles(SoretedProfiles sp)
        {
            var soretedProfiles = _context.SoretedProfiles.Where(c => c.SortedBy == sp.SortedBy && c.SeletedId == sp.SeletedId).FirstOrDefault();
            if (soretedProfiles == null)
            {
                return NotFound();
            }

            _context.SoretedProfiles.Remove(soretedProfiles);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SoretedProfilesExists(int? id)
        {
            return _context.SoretedProfiles.Any(e => e.RowNumber == id);
        }
    }
}
