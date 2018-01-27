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

        [HttpPost]
        [Route("travel/travelstatus/{studentID:guid}/{travelStatus:int}")]
        public IActionResult UpdateTravelStatus(Guid studentID, int travelStatus)
        {            
            var student = _context.Students.FirstOrDefault(s => s.Id == studentID);

            if(student.NeedTravel)
            {
                if (student == null)
                    return NotFound();

                var status = new TravelStatus();
                TravelStudent ts = new TravelStudent();

                switch (travelStatus)
                {
                    case 70:
                        status = TravelStatus.AtScholl;
                        CreateTravelStudentData(ts, student, status);
                        break;

                    case 75:
                        status = TravelStatus.AtHome;
                        CreateTravelStudentData(ts, student, status);
                        break;

                    case 80:
                        status = TravelStatus.Trasnporting;
                        CreateTravelStudentData(ts, student, status);
                        break;

                    case 85:
                        status = TravelStatus.IsComing;
                        CreateTravelStudentData(ts, student, status);
                        break;
                }

                var result = _context.SaveChanges();

                if (result == 0)
                    return BadRequest();

                return Ok();
            }
            else
            {
                return NotFound("Aluno não precisa");
            }

        }

        [HttpPost]
        [Route("travel/createqueue/{studentID:guid}")]
        public IActionResult CreateQueue([FromBody] Queue queue, Guid studentID)
        {            
            _context.Queues.Add(queue);
            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();

            return Ok();
        }
        
        [HttpGet]
        [Route("travel/all")]
        public IActionResult GetTravels()
        {
            List<TravelStudent> travels = _context.TravelsStudent
                .Include(t => t.Student)
                .Where(t => t.Student.Parent.Link.User.IsAtuhorize == true)
                .ToList();

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

            var clm = await _userManager.GetClaimsAsync(user);
            var paidClaim = clm.Where(c => c.Type == "paid");

            if (paidClaim == null)
                return NotFound("Claim not found");

            await _userManager.RemoveClaimsAsync(user, paidClaim);
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
                .Include(s => s.TravelsStudent)
                .ToList();

            if (students == null || students.Count <= 0)
                return NotFound();

            return Ok(students);
        }


        ///Local Method
        private void CreateTravelStudentData(TravelStudent ts, Student student, TravelStatus status)
        {
            ts.StartAt = DateTime.UtcNow.ToLocalTime();
            ts.Student = student;
            ts.Status = status;
            _context.TravelsStudent.Add(ts);
        }
    }
}
