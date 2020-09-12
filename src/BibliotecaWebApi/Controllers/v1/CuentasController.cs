using System.Text;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyPrimerWebApi.Data;
using MyPrimerWebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace MyPrimerWebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public CuentasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.userManager = userManager;
            _context = context;
        }
        [HttpPost("Crear")]
        public async Task<ActionResult<UserToken>> CreateUserAsync([FromBody] UserInfo userInfo)
        {
            var user = new ApplicationUser { UserName = userInfo.Email, Email = userInfo.Email };
            var result = await userManager.CreateAsync(user, userInfo.Password);
            if (result.Succeeded)
            {
                return BuildToken(userInfo, new List<string>());
            }
            else
            {
                return BadRequest("Username or password invalid");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> LoginAsync([FromBody] UserInfo userInfo)
        {
            var result = await signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var usuario = await userManager.FindByEmailAsync(userInfo.Email);
                var roles = await userManager.GetRolesAsync(usuario);

                return BuildToken(userInfo, roles);
            }
            else
            {
                return BadRequest("Username or password invalid");
            }
        }

        private ActionResult<UserToken> BuildToken(UserInfo userInfo, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("miValor", "Lo que yo quiera"),
                // Con el claim tipo Jti le asigno un identificador unico a un token mediante un GUID. Esto es util cuando deseo invalidar el token de un usuario
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiracion del token. En nuestro caso lo hacemos de una hora
            var expiracion = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiracion,
                signingCredentials: credential
            );

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiracion
            };
        }
    }
}
