using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VanEscolar.Constants;
using VanEscolar.Data;
using VanEscolar.Domain;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace VanEscolar.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPasswordHasher<ApplicationUser> _hasher;
        private readonly ILogger _logger;

        public AccountController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IPasswordHasher<ApplicationUser> hasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
            _hasher = hasher;
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
                _userManager.PasswordHasher = _hasher;
                var result = await _userManager.CreateAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    var roleresult = await _userManager.AddToRoleAsync(newUser, Roles.Parent);
                    await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Role ,"Parent"));
                    if (roleresult.Succeeded)
                        return Ok();

                    return BadRequest(roleresult.Errors);
                }
                else
                    return BadRequest(result.Errors);
            }
            else
                return BadRequest(ModelState);
        }

        [Route("auth/login")]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody]ApplicationUser user)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);

                if (result.Succeeded)
                    return Ok();
            }

            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while logging in {ex}");
            }

            return BadRequest("Faled to login");
        }

        [Route("auth/token")]
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] ApplicationUser user)
        {
            try
            {
                var currentUser = await _userManager.FindByEmailAsync(user.Email);

                if (currentUser != null)
                {
                    if(_hasher.VerifyHashedPassword(currentUser, currentUser.PasswordHash, user.Password) == PasswordVerificationResult.Success)
                    {
                        var claims = _context.UserClaims.Where(u => u.UserId == currentUser.Id)
                            .Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
                        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, currentUser.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Email, currentUser.Email));

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("314159265358979323846264338327"));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            issuer: "https://jonathanbraga.com.br",
                            audience: "https://jonathanbraga.com.br",
                            claims: claims,
                            expires: DateTime.UtcNow.AddYears(1),
                            signingCredentials: creds
                        );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            exepiration = token.ValidTo
                     
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while creating JWT: {ex}");
            }

            return BadRequest();
        }

        [Authorize]
        [Route("authorizeuser/{userID:guid}/{authorize:bool}")]
        [HttpPut]
        public IActionResult AuthorizeUser(Guid userID, bool authorize)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userID.ToString());

            if (user == null)
                return NotFound("User not found");

            user.IsAtuhorize = authorize;
            _context.Users.Update(user);
            var result = _context.SaveChanges();

            if (result == 0)
                return BadRequest();
            return Ok();
        }

        [Authorize]
        [Route("delete/{userID:guid}")]
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


        public IActionResult SignOut()
        {
            return null;
        }
    }
}
