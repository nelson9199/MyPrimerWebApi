using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyPrimerWebApi.Data;
using MyPrimerWebApi.helpers;
using MyPrimerWebApi.Models;
using MyPrimerWebApi.Models.DTOS;
using MyPrimerWebApi.Utils;

namespace MyPrimerWebApi.Controllers.v2
{
    [Route("api/v2/[controller]")]
    // [ApiController]
    // [HttpHeaderIsPresent("x-version", "2")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("primer", Name = "ObtenerPrimerAutor")]
        public async Task<ActionResult<Autor>> GetPrimerAutor()
        {
            return await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync();
        }
    }
}