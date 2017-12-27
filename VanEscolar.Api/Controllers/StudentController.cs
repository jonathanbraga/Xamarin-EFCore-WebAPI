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
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("createstudent/{parentID:guid}")]
        [HttpPost]
        public IActionResult CreateStudent(Guid parentID, [FromBody] Student student)
        {
            var parent = _context.Parents.FirstOrDefault(p => p.Id == parentID);

            if (parent == null)
                return NotFound();

            student.Parent = parent;
            _context.Students.Add(student);

            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();

            return Ok();
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

        //MANAGE
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
