using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VanEscolar.Data;
using VanEscolar.Domain;

namespace VanEscolar.Api.Controllers
{
    [Authorize(Policy = "_Managers")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManagerController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPut]
        [Route("travel/travelstatus/{studentID:guid}/{travelStatus:int}")]
        public IActionResult UpdateTravelStatus(Guid studentID, int travelStatus)
        {
            var travel = _context.Travels.FirstOrDefault(t => t.Student.Id == studentID);
            var student = _context.Students.FirstOrDefault(s => s.Id == studentID);

            if (travel == null || student == null)
                return NotFound();

            var status = new TravelStatus();
            TravelStudent ts = new TravelStudent();

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

            //TODO Melhorar Criação do Historico
            if (status == TravelStatus.IsComing)
            {
                ts.StartAt = DateTime.UtcNow.ToLocalTime();
                ts.Student = student;
                ts.Travel = travel;
                _context.TravelsStudent.Add(ts);
            }
            else if (status == TravelStatus.AtScholl || status == TravelStatus.AtHome)
            {
                ts.FinishAt = DateTime.UtcNow.ToLocalTime();
                _context.TravelsStudent.Update(ts);
            }
            // ---------------------------------

            travel.Status = status;
            _context.Travels.Update(travel);
            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();

            return Ok();
        }
        
        [HttpGet]
        [Route("travel/all")]
        public IActionResult GetTravels()
        {
            List<Travel> travels = _context.Travels.Where(t => t.Student.Parent.Link.User.IsAtuhorize == true).ToList();

            if (travels == null || travels.Count <= 0)
                return NotFound();

            return Ok(travels);
        }
        
        [Route("account/authorizeuser/{userID:guid}/{authorize:bool}")]
        [HttpPut]
        public async Task<IActionResult> AuthorizeUser(Guid userID, bool authorize)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userID.ToString());

            if (user == null)
                return NotFound("User not found");

            user.IsAtuhorize = authorize;
            _context.Users.Update(user);
            
            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();

            await _userManager.AddClaimAsync(user, new Claim("paid", authorize.ToString().ToLower()));
            return Ok();
        }
        
        [Route("account/deleteuser/{userID:guid}")]
        [HttpDelete]
        public IActionResult DeleteUser(Guid userID)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userID.ToString());

            if (user == null)
                return NotFound("User not found");

            _context.Users.Remove(user);
            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();

            return Ok();
        }
                
        [Route("parent/all")]
        [HttpGet]
        public IActionResult GetParents()
        {
            List<Parent> parents = _context.Parents
                .Include(p => p.Link)
                .Include(p => p.Messages)
                .Include(p => p.Students)
                .ToList();

            if (parents == null || parents.Count <= 0)
                return NotFound();

            return Ok(parents);
        }

        [Route("student/all")]
        [HttpGet]
        public IActionResult GetStudents()
        {
            List<Student> students = _context.Students
                .Include(s => s.Parent)
                .Include(s => s.School)
                .Include(s => s.Travel)
                .Include(s => s.TravelStudent)
                .ToList();

            if (students == null || students.Count <= 0)
                return NotFound();

            return Ok(students);
        }
    }
}
