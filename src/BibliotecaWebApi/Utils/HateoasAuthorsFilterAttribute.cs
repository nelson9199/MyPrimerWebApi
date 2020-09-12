using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyPrimerWebApi.Models.DTOS;

namespace MyPrimerWebApi.Utils
{
    public class HateoasAuthorsFilterAttribute : HateoasFilterAttribute
    {
        private readonly GeneradorEnlaces generadorEnlaces;

        public HateoasAuthorsFilterAttribute(GeneradorEnlaces generadorEnlaces)
        {
            this.generadorEnlaces = generadorEnlaces ?? throw new ArgumentNullException(nameof(generadorEnlaces));
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var incluirHATEOAS = DebeIncluirHateoas(context);

            if (!incluirHATEOAS)
            {
                await next();
                return;
            }

            var result = context.Result as ObjectResult;
            var model = result.Value as List<AutorDto> ?? throw new ArgumentNullException("Se esperaba una instancia de List<AutorDto>");
            result.Value = generadorEnlaces.GenerarEnlaces(model);
            await next();
        }

    }
}