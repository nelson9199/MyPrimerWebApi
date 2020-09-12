using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MyPrimerWebApi.Utils
{
    public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            // Ejemplo: "Controllers.v1"
            var controllerNamespace = controller.ControllerType.Namespace;
            var apiVersion = controllerNamespace.Split('.').Last().ToLower();
            controller.ApiExplorer.GroupName = apiVersion;
        }
    }
}