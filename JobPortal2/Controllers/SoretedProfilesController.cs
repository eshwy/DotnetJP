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

        // POST: api/SoretedProfiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SoretedProfiles>> PostSoretedProfiles(SoretedProfiles soretedProfiles)
        {
            if (!SoretedProfilesExists(soretedProfiles))
            {
                _context.SoretedProfiles.Add(soretedProfiles);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetSoretedProfiles", new { id = soretedProfiles.RowNumber }, soretedProfiles);
            }
            else
            {
                return BadRequest();
            }
            
        }

        // DELETE: api/SoretedProfiles/5
        [HttpPost]
        [Route("Remove")]
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

        private bool SoretedProfilesExists(SoretedProfiles SP)
        {
            return _context.SoretedProfiles.Any(e => e.SeletedId == SP.SeletedId && e.SortedBy == SP.SortedBy );
        }
    }
}
