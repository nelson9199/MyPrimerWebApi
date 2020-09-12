using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyPrimerWebApi.helpers
{
    public class MiFiltroDeException : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            
        }
    }
}