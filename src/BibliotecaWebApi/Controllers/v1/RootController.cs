using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyPrimerWebApi.Models;

namespace MyPrimerWebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "GetRoot")]
        public ActionResult<IEnumerable<Enlace>> Get()
        {
            List<Enlace> enlaces = new List<Enlace>();

            // Aqui colocamos los links
            enlaces.Add(new Enlace(href: Url.Link("GetRoot", new { }), rel: "self", metodo: "GET"));
            enlaces.Add(new Enlace(href: Url.Link("CrearAutor", new { }), rel: "crear-autores", metodo: "POST"));
            enlaces.Add(new Enlace(href: Url.Link("ObtenerAutores", new { }), rel: "obtener-autores", metodo: "GET"));

            return enlaces;
        }

    }
}
