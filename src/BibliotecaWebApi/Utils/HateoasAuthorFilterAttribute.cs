using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyPrimerWebApi.Models.DTOS;

namespace MyPrimerWebApi.Utils
{
    public class HateoasAuthorFilterAttribute : HateoasFilterAttribute
    {
        private readonly GeneradorEnlaces generadorEnlaces;

        public HateoasAuthorFilterAttribute(GeneradorEnlaces generadorEnlaces)
        {
            this.generadorEnlaces = generadorEnlaces ?? throw new ArgumentNullException(nameof(generadorEnlaces));
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var incluirHateoas = DebeIncluirHateoas(context);

            if (!incluirHateoas)
            {
                // Paso al siguiente filtro y retorno
                await next();
                return;
            }

            var result = context.Result as ObjectResult;
            var model = result.Value as AutorDto;
            if (model == null)
            {
                throw new ArgumentNullException("Se esperaba una instancioa de AutorDto");
            }
            else
            {
                generadorEnlaces.GenerarEnlaces(model);
                await next();
            }
        }
    }
}