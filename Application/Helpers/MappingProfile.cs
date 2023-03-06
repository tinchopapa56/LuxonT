using AutoMapper;
using Domain;
using Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Application.Helpers
{
    public class MappingProfile : Profile
    {
        private static string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<Usuario>();
            return passwordHasher.HashPassword(null, password);
        }
        public MappingProfile() 
        {

            CreateMap<Tasky, TaskyDto>();
            CreateMap<TaskyDto, Tasky>();       //TaskyDto => a => Tasky

            CreateMap<Usuario, RegisterDTO>();
            CreateMap<RegisterDTO, Usuario>()
                 //recibi un regiserDTO y CREA un Usuario
                // .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => new PasswordHasher<Usuario>().HashPassword(null, src.Password)));
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => HashPassword(src.Password)));
        }
        
    }
}