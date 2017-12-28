using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VanEscolar.Constants;
using VanEscolar.Data;
using VanEscolar.Domain;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace VanEscolar.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        [Route("Me")]
        [HttpGet]
        public IActionResult Me()
        {
            //configurar para ser o email no lugar do nome
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

        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser { UserName = user.Email, Email = user.Email, Link = new Link { User = user} };
                var result = await _userManager.CreateAsync(newUser, user.Password);

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

        [Route("auth/login")]
        public async Task<IActionResult> SignIn([FromBody]ApplicationUser user)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, false, false);

                if (result.Succeeded)
                    return Ok();
            }

            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while logging in {ex}");
            }

            return BadRequest("Faled to login");
        }

        public IActionResult SignOut()
        {
            return null;
        }
    }
}
