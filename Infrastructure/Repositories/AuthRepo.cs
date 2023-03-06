using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.DTOs;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Infrastructure.Repositories
{
    public class AuthRepo : IAuthRepo
    {
        private readonly LuxonDB _LuxonDB;
        private readonly IMapper _mapper;
        // private readonly UserManager<Usuario> _userManager;
         
        public AuthRepo(LuxonDB LuxonDB, IMapper mapper) //UserManager<Usuario> userManager
        {
            _LuxonDB = LuxonDB;
            _mapper = mapper;
            // _userManager = userManager;
        }
        public Usuario LogIn(string email)
        {
            var usuario = _LuxonDB.Usuarios.Where(u => u.Email == email).FirstOrDefault();
            return usuario; 
        }

        public bool Register(Usuario usuario)
        {

            // var usuario = new Usuario{
            //     UserName = registerDto.UserName,
            //     Email = registerDto.Email,
            //     PasswordHash = registerDto.Password, 
            //     // Rol = registerDto.Rol,
            // };
            //  var result = await _userManager.CreateAsync(user, registerDto.Password);

            // if(result.Succeeded){
            //     return CreateUserOBJ(user);
            // }
            // return BadRequest(result.Errors); 
            var user = _LuxonDB.Usuarios.Add(usuario); 
            return Save();       
        }
        public bool Save()
        {
            var changes = _LuxonDB.SaveChanges();
            return changes > 0 ? true : false;
        }
        public ICollection<Usuario> GetAllUsers()
        {
            var users = _LuxonDB.Usuarios.ToList();
            return users;
        }
        public bool alreadyExists(string username, string email){
            // if(_userManager.Users.AnyAsync(x => x.UserName == username )) return true;
            // if( _userManager.Users.AnyAsync(x => x.Email == email )) return true;
            if(_LuxonDB.Usuarios.Any(u => u.UserName == username)) return true;
            if(_LuxonDB.Usuarios.Any(u => u.Email == email)) return true;

            return false;

        }

    }
}