using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using Application;
using Application.DTOs;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;


using Microsoft.AspNetCore.Authorization;
using API;
using API.TokenService;

namespace LuxonTasks.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    
    public class UsuarioController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepo _AuthRepo;
        private readonly IConfiguration _config;
        private readonly TokenService _TokenService;
       
        public UsuarioController(IMapper mapper, IAuthRepo AuthRepo, IConfiguration config, TokenService TokenService) 
        {
            _AuthRepo = AuthRepo;
            _mapper = mapper;
            // _config = config;
            _TokenService = TokenService;
        }
            [HttpPost]
            [Route("login")]
            public IActionResult LogIn([FromBody] UsuarioDTO userData)
            {
                // var data = JsonConvert.DeserializeObject<dynamic>(userData);
                if(userData == null) return BadRequest();

                var user = _AuthRepo.LogIn(userData.Email);

                if(user == null) return NotFound();

                var token = _TokenService.CreateToken(user);
                var res = new Res 
                {
                    Message = "Success!",
                    Data = token,
                };

                return Ok(res);
                //TOKEN SERVICE
                // var jwt = _config.GetSection("Jwt").Get<Jwt>();
                
                // var claims = new []
                // {
                //     new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                //     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //     new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                //     new Claim("id", user.Id),
                //     new Claim("userName", user.UserName)
                // };

                // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                // var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // var token = new JwtSecurityToken(
                //     jwt.Issuer,
                //     jwt.Audience,
                //     claims,
                //     expires: DateTime.Now.AddDays(10),
                //     signingCredentials: signIn
                // );

                // var response = new {
                //     Success = true,
                //     message = "exito",
                //     result = new JwtSecurityTokenHandler().WriteToken(token)
                // };
            }
            
            [HttpPost]
            [Route("register")]
            public IActionResult Register([FromBody] RegisterDTO userData)
            {
                if(userData == null) return BadRequest();

                var alreadyExists = _AuthRepo.alreadyExists(userData.UserName, userData.Email);
                if(alreadyExists) return StatusCode(409, $"User '{userData.UserName}' or mail '{userData.Email}' already exists.");

                var mappedUsuario = _mapper.Map<Usuario>(userData);

                var user = _AuthRepo.Register(mappedUsuario);
                if (user != true )
                {
                    ModelState.AddModelError("", "Something Went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                var res = new Res {
                    Message = "Success!",
                    Data = userData,
                };

                return Ok(res);
            }
            // [HttpGet]
            // [Route("listUsers")]
            // public IActionResult GetAllUsers()
            // {
            //     var users = _AuthRepo.GetAllUsers();
            //     return Ok(users);
            // }
            [AllowAnonymous]
            [HttpGet]
            [Route("listUsers")]
            [ProducesResponseType(200, Type = typeof(IEnumerable<Usuario>))]
            public IActionResult GetEveryoneTasks() 
            {
                // var tasks = _mapper.Map<List<Tasky>>(_TasksRepo.GetEveryoneTasks());
                var users = _AuthRepo.GetAllUsers();

                if (!ModelState.IsValid) return BadRequest(ModelState);

                return Ok(users);
            }
    }
}