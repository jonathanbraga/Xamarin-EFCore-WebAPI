using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VanEscolar.Data;
using VanEscolar.Domain;

namespace VanEscolar.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TravelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public TravelController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("createtravel/{studentID:guid}")]
        public IActionResult CreateTravel(Guid studenID, [FromBody] Travel travel)
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == studenID);
            var currentUser = _context.Users.FirstOrDefault(u => u.Link.Parent.Students.Contains(student));

            if (student == null || currentUser == null)
                return NotFound("Student not found");

            if (currentUser.IsAtuhorize)
            {
                travel.NeedTravel = true;
                travel.Student = student;
                travel.Status = TravelStatus.AtHome;

                _context.Travels.Add(travel);
                var result = _context.SaveChanges();

                if (result == 0)
                    return BadRequest();

                return Ok();
            }

            return Forbid("User not authorized");
        }

        
        [HttpPut]
        [Route("needtravel/{studentID:guid}/{needTravel:bool}")]
        public IActionResult UpdateNeedTravel(Guid studentID, bool needTravel)
        {
            var travel = _context.Travels.FirstOrDefault(t => t.Student.Id == studentID);

            if (travel == null)
                return NotFound();

            travel.NeedTravel = needTravel;
            _context.Travels.Update(travel);
            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();

            return Ok();
        }        
    }
}
