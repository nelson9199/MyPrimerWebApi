using System.ComponentModel.DataAnnotations;

namespace MyPrimerWebApi.Models.DTOS
{
    public class EditarRolDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string RolName { get; set; }
    }
}