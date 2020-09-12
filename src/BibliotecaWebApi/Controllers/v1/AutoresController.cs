using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MyPrimerWebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    // [HttpHeaderIsPresent("x-version", "1")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, ILogger<AutoresController> logger, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
            this.logger = logger;
        }

        // Puedo ignorar la ruta base del controlador empezando la ruta de mi accion con un slash al principio: "/ruta"
        // /listado
        [HttpGet("/listado")]
        // api/autores/listado
        [HttpGet("listado")]
        // api/autores
        [HttpGet(Name = "ObtenerAutores")]
        // [ServiceFilter(typeof(MiFiltroDeAccion))]
        [ServiceFilter(typeof(HateoasAuthorsFilterAttribute))]
        public async Task<ActionResult<IEnumerable<AutorDto>>> Get(int numeroDePagina = 1, int cantidadDeRegistros = 2)
        {
            // throw new NotImplementedException();
            logger.LogInformation("Obteniendo los Autores");

            var query = context.Autores.AsQueryable();
            var totalRegistros = query.Count();

            var autoresDb = await query
            .Skip(cantidadDeRegistros * (numeroDePagina - 1))
            .Take(cantidadDeRegistros)
            .Include(x => x.Libros).ToListAsync();

            Response.Headers["x-total-registros"] = totalRegistros.ToString();
            Response.Headers["x-cantidad-paginas"] = ((int)Math.Ceiling((double)totalRegistros / cantidadDeRegistros)).ToString();

            List<AutorDto> autoresDto = mapper.Map<List<AutorDto>>(autoresDb);

            return autoresDto;
        }

        //Si pongo un signo de interrogacion "?" indico que el parametro es opcional
        //Pudedo definir tambien un parametro con un valor opcional: param2=nelson
        //GET api/autores/5 o GET api/autores/5/nelson
        [HttpGet("{id}", Name = "ObtenerAutor")]
        [ServiceFilter(typeof(HateoasAuthorFilterAttribute))]
        public async Task<ActionResult<AutorDto>> Get(int id /*[BindRequired] string param2*/)
        {
            var autorDb = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Id == id);

            if (autorDb == null)
            {
                logger.LogWarning($"El autor de id {id} no ha sido encontrado");
                return NotFound();
            }
            var autorDto = mapper.Map<AutorDto>(autorDb);

            return autorDto;
        }

        private void GenerarEnlaces(AutorDto autorDto)
        {
            autorDto.Enlaces.Add(new Enlace(href: Url.Link("ObtenerAutor", new { id = autorDto.Id }), rel: "self", metodo: "GET"));

            autorDto.Enlaces.Add(new Enlace(href: Url.Link("ActualizarAutor", new { id = autorDto.Id }), rel: "actualizar-autor", metodo: "PUT"));

            autorDto.Enlaces.Add(new Enlace(href: Url.Link("ActualizarPatchAutor", new { id = autorDto.Id }), rel: "actualizar-autores-patch", metodo: "PATCH"));

            autorDto.Enlaces.Add(new Enlace(href: Url.Link("BorrarAutor", new { id = autorDto.Id }), rel: "borrar-autor", metodo: "DELETE"));
        }

        [HttpPost(Name = "CrearAutor")]
        public async Task<ActionResult> Post([FromBody] AutorCreateDto autorCreate)
        {
            var autor = mapper.Map<Autor>(autorCreate);

            context.Add(autor);

            await context.SaveChangesAsync();

            var autorDto = mapper.Map<AutorDto>(autor);

            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autorDto);
        }

        [HttpPut("{id}", Name = "ActualizarAutor")]
        public async Task<ActionResult<Autor>> Put(int id, [FromBody] AutorCreateDto autorActualizaion)
        {
            var autorDb = await context.Autores.FindAsync(id);

            if (autorDb == null)
            {
                return NotFound();
            }
            mapper.Map(autorActualizaion, autorDb);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}", Name = "ActualizarPatchAutor")]
        public async Task<IActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<AutorCreateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var autorDb = await context.Autores.FindAsync(id);

            if (autorDb == null)
            {
                return NoContent();
            }
            var autorDto = mapper.Map<AutorCreateDto>(autorDb);

            patchDocument.ApplyTo(autorDto, ModelState);

            mapper.Map(autorDto, autorDb);
            // Validar el modelo cuando recibo info de un patchDocument
            var isValid = TryValidateModel(autorDb);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Borra un elemento especifico
        /// </summary>
        /// <param name="id">Id del elemento a borrar</param>
        [HttpDelete("{id}", Name = "BorrarAutor")]
        // [ProducesResponseType(404)]
        // [ProducesResponseType(typeof(AutorDto), 200)]
        public async Task<ActionResult<AutorDto>> Delete(int id)
        {
            var autorDb = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if (autorDb == null)
            {
                return NotFound();
            }
            context.Remove(autorDb);
            await context.SaveChangesAsync();
            var autorDto = mapper.Map<AutorDto>(autorDb);
            return autorDto;
        }
    }
}