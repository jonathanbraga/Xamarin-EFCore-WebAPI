using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VanEscolar.Constants;
using VanEscolar.Data;
using VanEscolar.Domain;

namespace VanEscolar.Api.Controllers
{
    
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        
        [Route("createstudent/{parentID:guid}")]
        [HttpPost]
        public async Task<IActionResult> CreateStudent(Guid parentID, [FromBody] Student student)
        {
            var currentUser = await _userManager.FindByNameAsync(this.User.Identity.Name);
            if (currentUser != null)
            {
                var parent = _context.Parents.FirstOrDefault(p => p.Link.User.Id == currentUser.Id && p.Id == parentID);

                if (parent == null)
                    return NotFound();

                student.Parent = parent;
                _context.Students.Add(student);

                var result = _context.SaveChanges();

                if (result == 0)
                    return BadRequest();

                return Ok();
            }

            return Forbid();
        }

        [Route("edit/{studentID:guid}")]
        [HttpPut]
        public IActionResult EditStudent(Guid studentID, [FromBody] Student student)
        {
            var currentStudent = _context.Students.FirstOrDefault(s => s.Id == studentID);

            if (student == null)
                return NotFound();

            currentStudent = student;
            _context.Students.Update(currentStudent);
            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();
            return Ok();
        }

        [Authorize(Roles = "Manager")]
        [Route("all")]
        [HttpGet]
        public IActionResult GetStudents()
        {
            List<Student> students = _context.Students.ToList();

            if (students == null || students.Count < 0)
                return NotFound();

            return Ok(students);
        }
    }
}
