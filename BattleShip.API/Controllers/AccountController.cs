namespace BattleShip.API.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using BattleShip.BusinessLogic.Interfaces;
    using BattleShip.Models.Entities;
    using BattleShip.WEB.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IPlayerService playerService;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IPlayerService service, IConfiguration config)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.playerService = service;
            this.config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            ApplicationUser user = new ApplicationUser { Email = model.Email, UserName = model.UserName };

            var result = await this.userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            this.playerService.CreatePlayer(user);

            return this.Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await this.signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (!result.Succeeded)
            {
                return this.Unauthorized("Incorrect login or password");
            }

            var userToVerify = await this.userManager.FindByNameAsync(model.UserName);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userToVerify.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userToVerify.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return this.Ok(new
            {
                Id = userToVerify.Id,
                userName = model.UserName,
                token = tokenHandler.WriteToken(token),
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id != null)
            {
                var user = await this.userManager.FindByIdAsync(id);
                var claims = new[]
               {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
               };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config.GetSection("AppSettings:Token").Value));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds,
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return this.Ok(new
                {
                    Id = Convert.ToInt32(id),
                    userName = user.UserName,
                    token = tokenHandler.WriteToken(token),
                });
            }

            return this.Unauthorized();
        }
    }
}
