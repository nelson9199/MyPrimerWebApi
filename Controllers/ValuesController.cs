using System.Threading;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyPrimerWebApi.Data;
using MyPrimerWebApi.Models;
using MyPrimerWebApi.Services;

namespace Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    // [EnableCors("PermitirApiRequest")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IHashService hashService;
        private readonly IDataProtector protector;

        public ValuesController(IConfiguration configuration, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IDataProtectionProvider protectionProvider, IHashService hashService)
        {
            this.configuration = configuration;
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.hashService = hashService;
            protector = protectionProvider.CreateProtector("Valor_unico_y_quiza_secreto");
        }

        [HttpGet]
        [EnableCors("PermitirApiRequest")]
        // [ResponseCache(Duration = 15)]
        public ActionResult<object> Get()
        {
            var protectorLimitadoPorTiempo = protector.ToTimeLimitedDataProtector();

            // Proteccion limitada por tiempo
            string textoPlano = "Nelson Marro";
            string textoCifrado = protectorLimitadoPorTiempo.Protect(textoPlano, TimeSpan.FromSeconds(5));
            Thread.Sleep(6000);
            string textoDesencriptado = protectorLimitadoPorTiempo.Unprotect(textoCifrado);

            // Proteccion normal
            // string textoPlano = "Nelson Marro";
            // string textoCifrado = protector.Protect(textoPlano);
            // string textoDesencriptado = protector.Unprotect(textoCifrado);
            return Ok(new { textoPlano, textoCifrado, textoDesencriptado });
        }

        [HttpGet("hash")]
        public async Task<ActionResult<IdentityRole>> GetHash()
        {
            // Hashing Data
            // string textoPlano = "Nelson Marro";
            // var hasResult1 = hashService.HashData(textoPlano).Hash;
            // var hasResult2 = hashService.HashData(textoPlano).Hash;
            // return Ok(new { textoPlano, hasResult1, hasResult2 });
            return await roleManager.FindByNameAsync("Admin");

        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}