using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VanEscolar.Data;
using VanEscolar.Domain;

namespace VanEscolar.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ParentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
        }

        [Route("account/{userID:guid}/createparent")]
        [HttpPost]
        public IActionResult CreateParent(Guid userID, [FromBody] Parent parent)
        {
            var link = _context.Links.FirstOrDefault(l => l.User.Id == userID.ToString());

            if (link == null)
                return NotFound();

            if (_context.Parents.Any(p => p.Email.Equals(parent.Email, StringComparison.CurrentCultureIgnoreCase)))
                return BadRequest("Email já existe");

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
    }

}
