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

            if (student == null)
                return NotFound("Student not found");

            travel.NeedTravel = true;
            travel.Student = student;
            travel.Status = TravelStatus.AtHome;

            _context.Travels.Add(travel);
            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();

            return Ok();
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

        [Authorize(Roles = "Manager")]
        [HttpPut]
        [Route("travelstatus/{studentID:guid}/{travelStatus:int}")]
        public IActionResult UpdateTravelStatus(Guid studentID, int travelStatus)
        {
            var travel = _context.Travels.FirstOrDefault(t => t.Student.Id == studentID);

            if (travel == null)
                return NotFound();

            var status = new TravelStatus();

            switch (travelStatus)
            {
                case 70:
                    status = TravelStatus.AtScholl;
                    break;

                case 75:
                    status = TravelStatus.AtHome;
                    break;

                case 80:
                    status = TravelStatus.Trasnporting;
                    break;

                case 85:
                    status = TravelStatus.IsComing;
                    break;
            }


            travel.Status = status;
            _context.Travels.Update(travel);
            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        [Route("all")]
        public IActionResult GetTravels()
        {
            List<Travel> travels = _context.Travels.Where(t => t.Student.Parent.Link.User.IsAtuhorize == true).ToList();

            if (travels == null || travels.Count <= 0)
                return NotFound();

            return Ok(travels);
        }
    }
}
