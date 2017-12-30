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
    //Only Manager
    [Authorize(Roles = "Manager")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SchoolController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public SchoolController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("createschool")]
        public async Task<IActionResult> CreateSchool([FromBody] School school)
        {
            try
            {
                school.CreatedAt = DateTime.UtcNow.ToLocalTime();
                _context.Schools.Add(school);
                var result = await _context.SaveChangesAsync();

                if (result == 0)
                    return BadRequest();

                return Ok();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception thrown while creating School: {ex}");
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("editschool/{schoolID:guid}")]
        public async Task<IActionResult> EditShool(Guid schoolID)
        {
            var school = _context.Schools.FirstOrDefault(s => s.Id == schoolID);

            if (school == null)
                return NotFound();

            _context.Schools.Update(school);
            var result = await _context.SaveChangesAsync();

            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [HttpDelete]
        [Route("deleteschool/{schoolID:guid}")]
        public async Task<IActionResult> DeleteShool(Guid schoolID)
        {
            var school = _context.Schools.FirstOrDefault(s => s.Id == schoolID);

            if (school == null)
                return NotFound();

            _context.Schools.Remove(school);
            var result = await _context.SaveChangesAsync();

            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetSchools()
        {
            List<School> schools = _context.Schools.ToList();

            if (schools == null || schools.Count <= 0)
                return NotFound();

            return Ok(schools);
        }

        [HttpGet]
        [Route("sutdentsfromschool/{schoolID:guid}")]
        public IActionResult GetStudentFromSchool(Guid schoolID)
        {
            List<Student> students = _context.Students.Where(s => s.School.Id == schoolID).ToList();

            if (students == null || students.Count < 0)
                return NotFound();

            return Ok(students);
        }
    }
}
