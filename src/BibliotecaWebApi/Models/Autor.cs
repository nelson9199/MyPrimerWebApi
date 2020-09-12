using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyPrimerWebApi.helpers;

namespace MyPrimerWebApi.Models
{
    public class Autor : IValidatableObject
    {
        public int Id { get; set; }

        // [PrimeraLetraMayuscula]
        // [StringLength(10, MinimumLength = 5, ErrorMessage = "El campo Nombre debe tener {1} caracteres como máximo y {2} caracteres como mínimo")]
        [Required]
        public string Nombre { get; set; }
        public string Identificacion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public List<Libro> Libros { get; set; }

        // [Range(18, 120)] // La edad debe estar entre 18 y 120
        // public int Edad { get; set; }

        // [Url]
        // public string Url { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe estar en mayúscula", new string[] { nameof(Nombre) });
                }
            }
        }
    }
}