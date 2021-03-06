﻿using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Policy = "_Parents")]
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
        public IActionResult CreateStudent(Guid parentID, [FromBody] Student student)
        {
            var parent = _context.Parents.FirstOrDefault(p => p.Id == parentID);

            if (parent == null)
                return NotFound();

            student.CreatedAt = DateTime.UtcNow.ToLocalTime();
            student.Parent = parent;
            student.NeedTravel = true;

            _context.Students.Add(student);

            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [Route("givenschool/{studentID:guid}/{schoolID:guid}")]
        [HttpPut]
        public IActionResult GivenSchool(Guid studentID, Guid schoolID)
        {
            var school = _context.Schools.FirstOrDefault(p => p.Id == schoolID);
            var student = _context.Students.FirstOrDefault(p => p.Id == studentID);

            if (school == null || student == null)
                return NotFound();

            student.School = school;
            _context.Students.Update(student);

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

        [Route("delete/{studentID:guid}")]
        [HttpDelete]
        public IActionResult DeleteStudent(Guid studentID)
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == studentID);

            if (student == null)
                return NotFound();

            _context.Students.Remove(student);
            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();

            return Ok();
        }
    }
}
