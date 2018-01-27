using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VanEscolar.Data;
using VanEscolar.Domain;

namespace VanEscolar.Api.Controllers
{
    [Authorize(Policy = "_Parents")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ParentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
        }

        [Route("createparent/{userID:guid}")]
        [HttpPost]
        public IActionResult CreateParent(Guid userID, [FromBody] Parent parent)
        {
            var link = _context.Links.FirstOrDefault(l => l.User.Id == userID.ToString());

            if (link == null)
                return NotFound();

            parent.CreatedAt = DateTime.UtcNow.ToLocalTime();

            _context.Parents.Add(parent);
            link.Parent = parent;
            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [Route("{parentID:guid}")]
        [HttpGet]
        public IActionResult GetParent(Guid parentID)
        {
            var parent = _context.Parents
                .Include(p => p.Students)
                .Include(p => p.Link)
                .Include(p => p.Messages)
                .FirstOrDefault(p => p.Id == parentID);

            if (parent == null)
                return NotFound();

            return Ok(parent);
        }
        

        [Route("students/{parentID:guid}")]
        [HttpGet]
        public IActionResult GetStudents(Guid parentID)
        {
            var students = _context.Students
                .Include(s => s.Parent)
                .Include(s => s.School)
                .Include(s => s.TravelsStudent)
                .Where(s => s.Parent.Id == parentID);

            if (students == null)
                return NotFound();

            return Ok(students);
        }

        [Route("edit/{parentID:guid}")]
        [HttpPut]
        public IActionResult EditParent(Guid parentID, [FromBody] Parent parent)
        {
            var currentParent = _context.Parents.FirstOrDefault(p => p.Id == parentID);

            if (currentParent == null)
                return NotFound();

            currentParent = parent;
            _context.Parents.Update(currentParent);
            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();
            return Ok();

        }

        [Route("delete/{parentID:guid}")]
        [HttpDelete]
        public IActionResult DeleteParent(Guid parentID)
        {
            var parent = _context.Parents.FirstOrDefault(p => p.Id == parentID);
            var link = _context.Links.FirstOrDefault(l => l.Parent.Id == parentID);

            if (parent == null || link == null)
                return NotFound();

            _context.Links.Remove(link);
            _context.Parents.Remove(parent);
            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();

            return Ok();
        }       
    }

}
