using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal2.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using JobPortal2.Repository;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UserGetisterAndLoginTablesController : ControllerBase
    {
        private readonly JobPortalContext _context;
        private readonly IJWTManagerRepository _jWTManager;


        public UserGetisterAndLoginTablesController(JobPortalContext context,IJWTManagerRepository repo)
        {
            _context = context;
            _jWTManager = repo;
        }

        // GET: api/UserGetisterAndLoginTables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGetisterAndLoginTable>>> GetUserGetisterAndLoginTable()
        {
            return await _context.UserGetisterAndLoginTable.ToListAsync();
        }

        // GET: api/UserGetisterAndLoginTables/5
        [HttpGet("{UserName}")]
        public ActionResult<UserGetisterAndLoginTable> GetUserGetisterAndLoginTable(string UserName)
        {

            var data = _context.UserGetisterAndLoginTable.FirstOrDefault(c => c.UserName == UserName);

            Console.WriteLine(data);
            return data;
        }

        [AllowAnonymous]
        [HttpPost]        
        public IActionResult TestLogin(UserLogin user)
        {
            var token = _jWTManager.Authenticate(user);

            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }












        


            // PUT: api/UserGetisterAndLoginTables/5
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            //[HttpPut("{id}")]
            //public async Task<IActionResult> PutUserGetisterAndLoginTable(int id, UserGetisterAndLoginTable userGetisterAndLoginTable)
            //{
            //    if (id != userGetisterAndLoginTable.UserId)
            //    {
            //        return BadRequest();
            //    }

            //    _context.Entry(userGetisterAndLoginTable).State = EntityState.Modified;

            //    try
            //    {
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!UserGetisterAndLoginTableExists(id))
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

            //// POST: api/UserGetisterAndLoginTables
            //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            //[HttpPost]
            //public async Task<ActionResult<UserGetisterAndLoginTable>> PostUserGetisterAndLoginTable(UserGetisterAndLoginTable userGetisterAndLoginTable)
            //{
            //    _context.UserGetisterAndLoginTable.Add(userGetisterAndLoginTable);
            //    await _context.SaveChangesAsync();

            //    return CreatedAtAction("GetUserGetisterAndLoginTable", new { id = userGetisterAndLoginTable.UserId }, userGetisterAndLoginTable);
            //}

            //// DELETE: api/UserGetisterAndLoginTables/5
            //[HttpDelete("{id}")]
            //public async Task<IActionResult> DeleteUserGetisterAndLoginTable(int id)
            //{
            //    var userGetisterAndLoginTable = await _context.UserGetisterAndLoginTable.FindAsync(id);
            //    if (userGetisterAndLoginTable == null)
            //    {
            //        return NotFound();
            //    }

            //    _context.UserGetisterAndLoginTable.Remove(userGetisterAndLoginTable);
            //    await _context.SaveChangesAsync();

            //    return NoContent();
            //}

            //private bool UserGetisterAndLoginTableExists(int id)
            //{
            //    return _context.UserGetisterAndLoginTable.Any(e => e.UserId == id);
            //}
        
    }
}
