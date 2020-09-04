using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyPrimerWebApi.Models.DTOS
{
    public class AutorCreateDto
    {
        [Required]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public List<LibroCreateDto> Libros { get; set; }

    }
}