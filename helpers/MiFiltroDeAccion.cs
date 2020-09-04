using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MyPrimerWebApi.helpers
{
    public class MiFiltroDeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltroDeAccion> logger;

        public MiFiltroDeAccion(ILogger<MiFiltroDeAccion> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogError("OnActionExecuted");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogError("OnActionExecuting");
        }
    }
}