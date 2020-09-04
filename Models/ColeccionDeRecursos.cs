using System.Collections.Generic;

namespace MyPrimerWebApi.Models
{
    public class ColeccionDeRecursos<T> : Recurso where T : Recurso
    {
        // Cuando haga un return en un controlador de un valor que sea del tipo de esta clase se van a retornar todas las propiedades que tenga esta Clase
        public List<T> Valores { get; set; }
        public ColeccionDeRecursos(List<T> valores)
        {
            Valores = valores;
        }
    }
}