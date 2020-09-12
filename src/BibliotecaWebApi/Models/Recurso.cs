using System.Collections.Generic;

namespace MyPrimerWebApi.Models
{
    public class Recurso
    {
        public List<Enlace> Enlaces { get; set; } = new List<Enlace>();
    }
}