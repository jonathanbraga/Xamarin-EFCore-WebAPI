using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using VanEscolar.Data;
using VanEscolar.Domain;

namespace VanEscolar.Api.Controllers
{
    [Authorize]
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
            var parent = _context.Parents.FirstOrDefault(p => p.Id == parentID);

            if (parent == null)
                return NotFound();

            return Ok(parent);
        }
        

        [Route("students/{parentID:guid}")]
        [HttpGet]
        public IActionResult GetStudents(Guid parentID)
        {
            var parent = _context.Parents.FirstOrDefault(p => p.Id == parentID);

            if (parent == null)
                return NotFound();

            return Ok(parent.Students);
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

        //[Authorize(Roles = "Manage")]
        [Route("all")]
        [HttpGet]
        public IActionResult GetParents()
        {
            List<Parent> parents = _context.Parents.ToList();

            if (parents == null || parents.Count < 0)
                return NotFound();

            return Ok(parents);
        }
    }

}
