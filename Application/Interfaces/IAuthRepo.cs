using Application.DTOs;
using Domain;

namespace Application
{
    public interface IAuthRepo
    {
        public Usuario LogIn(string email);
        public bool Register(Usuario usuario);
        public bool alreadyExists (string username, string email);
        public bool Save();
        public ICollection<Usuario> GetAllUsers();
    }
}