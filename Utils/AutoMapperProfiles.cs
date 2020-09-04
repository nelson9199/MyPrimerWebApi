using AutoMapper;
using MyPrimerWebApi.Models;
using MyPrimerWebApi.Models.DTOS;

namespace MyPrimerWebApi.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Autor, AutorDto>().ReverseMap();
            CreateMap<Libro, LibroDto>().ReverseMap();
            CreateMap<LibroCreateDto, Libro>().ReverseMap();
            CreateMap<AutorCreateDto, Autor>()
            .ForMember(x => x.Id, opt => opt.Ignore()).ReverseMap();
        }
    }
}