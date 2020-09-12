using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using MyPrimerWebApi.Models;
using MyPrimerWebApi.Models.DTOS;

namespace MyPrimerWebApi.Utils
{
    public class GeneradorEnlaces
    {
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor actionContextAccessor;

        public GeneradorEnlaces(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccessor = actionContextAccessor;
        }

        private IUrlHelper ConstruirUrlHelper()
        {
            return urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public ColeccionDeRecursos<AutorDto> GenerarEnlaces(List<AutorDto> autores)
        {
            var urlHelper = ConstruirUrlHelper();
            var resultado = new ColeccionDeRecursos<AutorDto>(autores);
            autores.ForEach(a => GenerarEnlaces(a));
            resultado.Enlaces.Add(new Enlace(urlHelper.Link("ObtenerAutores", new { }), rel: "self", metodo: "GET"));
            resultado.Enlaces.Add(new Enlace(urlHelper.Link("CrearAutor", new { }), rel: "crear-autor", metodo: "POST"));
            return resultado;
        }

        public void GenerarEnlaces(AutorDto autor)
        {
            var urlHelper = ConstruirUrlHelper();
            autor.Enlaces.Add(new Enlace(href: urlHelper.Link("ObtenerAutor", new { id = autor.Id }), rel: "self", metodo: "GET"));
            autor.Enlaces.Add(new Enlace(href: urlHelper.Link("ActualizarAutor", new { id = autor.Id }), rel: "actualizar-autor", metodo: "PUT"));
            autor.Enlaces.Add(new Enlace(href: urlHelper.Link("BorrarAutor", new { id = autor.Id }), rel: "borrar-autor", metodo: "DELETE"));
        }
    }
}