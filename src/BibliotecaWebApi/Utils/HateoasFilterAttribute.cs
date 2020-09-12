using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyPrimerWebApi.Utils
{
    public class HateoasFilterAttribute : ResultFilterAttribute
    {
        protected bool DebeIncluirHateoas(ResultExecutingContext context)
        {
            var result = context.Result as ObjectResult;
            if (!EsRespuestaExitosa(result))
            {
                return false;
            }

            var header = context.HttpContext.Request.Headers["incluirHateoas"];

            if (header.Count == 0)
            {
                return false;
            }
            // Accedo a los headers como un arreglo
            var accept = header[0];

            if (!accept.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }
        private bool EsRespuestaExitosa(ObjectResult result)
        {
            if (result == null || result.Value == null)
            {
                return false;
            }

            if (result.StatusCode.HasValue && !result.StatusCode.Value.ToString().StartsWith("2"))
            {
                return false;
            }

            return true;
        }
    }
}