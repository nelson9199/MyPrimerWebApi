using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPrimerWebApi.Data;
using MyPrimerWebApi.Models;
using MyPrimerWebApi.Models.DTOS;

namespace MyPrimerWebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public UsuariosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        [HttpPost("AsignarUserRol")]
        public async Task<ActionResult> AsignarRolUsuario(EditarRolDto editarRolDto)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDto.UserId);

            if (usuario == null)
            {
                return NotFound();
            }
            //Este tipo de asignacion es para la utenticacion clasica de Identity
            await userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDto.RolName));
            //Este tipo de asignacion es para autenticacion mediante Jwt
            await userManager.AddToRoleAsync(usuario, editarRolDto.RolName);
            return Ok();
        }
        [HttpPost("RemoverUserRol")]
        public async Task<ActionResult> RemoverRolUsuario(EditarRolDto editarRolDto)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDto.UserId);
            if (usuario == null)
            {
                return NotFound();
            }
            //Este tipo de asignacion es para la utenticacion clasica de Identity
            await userManager.RemoveClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDto.RolName));
            //Este tipo de asignacion es para autenticacion mediante Jwt
            await userManager.RemoveFromRoleAsync(usuario, editarRolDto.RolName);
            return Ok();
        }
    }
}
