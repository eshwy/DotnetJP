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

    public class ParticularDetailsController : ControllerBase
    {
        private readonly JobPortalContext _context;

        public ParticularDetailsController(JobPortalContext context)
        {
            _context = context;
        }

        // GET: api/ParticularDetailsModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticularDetails>>> GetParticularDetailsModel()
        {
            
            string storeprocedure = "exec jopportalsp @choice=view_user_deatils";

            var data = await _context.ParticularDetails.FromSqlRaw(storeprocedure).ToListAsync();
            return data;
        }

        // GET: api/ParticularDetailsModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ParticularDetails>>> GetParticularDetailsModel(string id)
        {
            string storeprocedure = $"exec jopportalsp @choice=Particular_detail,@UserInputValue={id}";

            var data =  await _context.ParticularDetails.FromSqlRaw(storeprocedure).ToListAsync();
            return data;      
        }

        
    }
}
