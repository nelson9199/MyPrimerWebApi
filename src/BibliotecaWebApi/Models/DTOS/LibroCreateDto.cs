using System.Linq;
using MyPrimerWebApi.Data;

namespace MyPrimerWebApi.Models.DTOS
{
    public class LibroCreateDto
    {
        private readonly ApplicationDbContext context;
        private int autorId;
        public LibroCreateDto(ApplicationDbContext context)
        {
            this.context = context;
        }
        public string Titulo { get; set; }
        public int AutorId
        {
            get { return autorId; }
            set
            {
                autorId = (context.Autores.Select(x => x.Id).Last()) + 1;
            }
        }
    }
}