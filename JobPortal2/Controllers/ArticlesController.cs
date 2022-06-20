﻿using System;
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
    public class ArticlesController : ControllerBase
    {
        private readonly JobPortalContext _context;

        public ArticlesController(JobPortalContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticle()
        {
            return await _context.Article.ToListAsync();
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticle(int? id)
        {
            var article =await _context.Article.Where(c => c.UserId == id).ToListAsync();

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        [HttpPost("ParticularArticle")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticle(Article art)
        {
            var article = await _context.Article.Where(c => c.UserId == art.UserId && c.Title == art.Title).ToListAsync();

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        // PUT: api/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [System.Web.Mvc.ValidateInput(false)]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            _context.Article.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.RowNumber }, article);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutArticle(int? id, Article article)
        //{
        //    if (id != article.RowNumber)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(article).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ArticleExists(id))
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

        //// POST: api/Articles
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        //// DELETE: api/Articles/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteArticle(int? id)
        //{
        //    var article = await _context.Article.FindAsync(id);
        //    if (article == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Article.Remove(article);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool ArticleExists(int? id)
        {
            return _context.Article.Any(e => e.RowNumber == id);
        }
    }
}
