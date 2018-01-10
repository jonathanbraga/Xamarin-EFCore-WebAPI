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
using Microsoft.Extensions.DependencyInjection;

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
        private readonly IServiceProvider _serviceProvider;

        public AccountController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IPasswordHasher<ApplicationUser> hasher,
            IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
            _hasher = hasher;
            _serviceProvider = serviceProvider;


            //Popular com o Manager User
            var db = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider.GetService<ApplicationDbContext>();            
            if(db.UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Role && uc.ClaimValue == Roles.Manager) == null)
            {
                ApplicationUser user = new ApplicationUser { Email = "osmildobraga@yahoo.com", UserName = "osmildobraga@yahoo.com", Name = "Osmildo Braga", Password = "#Vida123",
                CreateAt = DateTime.UtcNow.ToLocalTime(), IsAtuhorize = true, EmailConfirmed = true};

                _userManager.PasswordHasher = _hasher;
                var result = _userManager.CreateAsync(user, user.Password).Result;

                if(result.Succeeded)
                {
                    try
                    {
                        var roleResult = _userManager.AddToRoleAsync(user, Roles.Manager);
                        _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, Roles.Manager));
                    }
                    catch (Exception ex)
                    {

                        _logger.LogError($"Exception thrown while creating Manager: {ex}");
                    }                    
                }
            }
        }

        [Authorize]
        [Route("Me")]
        [HttpGet]
        public IActionResult Me()
        {
            //configurar para ser o email no lugar do nome
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));

            if (user == null)
                return NotFound();

            var model = new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.Name
            };

            return Ok(model);
        }

        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser { Name = user.Name, UserName = user.Email, Email = user.Email, CreateAt = DateTime.UtcNow.ToLocalTime() ,Link = new Link { User = user} };
                _userManager.PasswordHasher = _hasher;
                var result = await _userManager.CreateAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    var roleresult = await _userManager.AddToRoleAsync(newUser, Roles.Parent);
                    await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Role , Roles.Parent));
                    // Role relacionada a autenticação do usuário
                    await _userManager.AddClaimAsync(newUser, new Claim("paid", "false"));
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
                        var userClaims = await _userManager.GetClaimsAsync(currentUser);

                        var claims = _context.UserClaims.Where(u => u.UserId == currentUser.Id)
                            .Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
                        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, currentUser.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Email, currentUser.Email));
                        //claims.Add(new Claim("email", user.Email.ToLower()));
                        claims.Union(userClaims);

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
                            exepiration = token.ValidTo.ToLocalTime()
                     
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

        public IActionResult SignOut()
        {
            return null;
        }
    }
}
