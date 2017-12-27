using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VanEscolar.Constants;
using VanEscolar.Data;
using VanEscolar.Domain;
using System.Linq;

namespace VanEscolar.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [Route("Me")]
        [HttpGet]
        public IActionResult Me()
        {
            var email = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));

            if(user == null)
                return NotFound();

            var model = new
            {
                user.Id,
                Name = user.UserName,
                user.Email
            };

            return Ok(model);
        }

        [Route("SignUp")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser { UserName = user.Email, Email = user.Email, Link = new Link { User = user} };
                var result = await _userManager.CreateAsync(newUser, user.PasswordHash);

                if (result.Succeeded)
                {
                    var roleresult = await _userManager.AddToRoleAsync(newUser, Roles.Parent);
                    return Ok();
                }
                else
                    return BadRequest(result);
            }
            else
                return BadRequest(ModelState);
        }

        public IActionResult SignIn()
        {
            return null;
        }

        public IActionResult SignOut()
        {
            return null;
        }
    }
}
