using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyPrimerWebApi.Models.DTOS
{
    public class AutorDto : Recurso
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public List<LibroDto> Libros { get; set; }
    }
}