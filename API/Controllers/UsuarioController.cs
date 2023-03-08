using Application;
using Application.DTOs;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using API;
using API.TokenService;

using SendGrid;
using SendGrid.Helpers.Mail;
using API.EmailService;

namespace LuxonTasks.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    
    public class UsuarioController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepo _AuthRepo;
        private readonly TokenService _TokenService;
        private readonly ISendGridClient _sendGridClient;
       
        public UsuarioController(IMapper mapper, IAuthRepo AuthRepo, TokenService TokenService, ISendGridClient sendGridClient) 
        {
            _AuthRepo = AuthRepo;
            _mapper = mapper;
            _TokenService = TokenService;
            _sendGridClient = sendGridClient;
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
                    Message = token,
                    Data = user,
                };

                return Ok(res);
            }
            
            [HttpPost]
            [Route("register")]
            public async Task<IActionResult> Register([FromBody] RegisterDTO userData)

            {
                //creating user
                if(userData == null) return BadRequest();

                var alreadyExists = _AuthRepo.alreadyExists(userData.UserName, userData.Email);
                if(alreadyExists) return StatusCode(409, $"User '{userData.UserName}' or mail '{userData.Email}' already exists.");

                var mappedUsuario = _mapper.Map<Usuario>(userData);

                var user = _AuthRepo.Register(mappedUsuario);
                if (user != true )
                {
                    ModelState.AddModelError("", "Something Went wrong while creating USER");
                    return StatusCode(500, ModelState);
                }

                var token = _TokenService.CreateToken(mappedUsuario);

                //sending mail
                Res sendMailRES = await SendWelcomeMessageAsync(userData.Email, userData.UserName);
                if (sendMailRES.Message != "Success!")
                {
                    ModelState.AddModelError("", "Something Went wrong while sending MAIL");
                    return StatusCode(500, ModelState);
                }

                return Ok(new Res{Message = token, Data = sendMailRES.Data});
                // return Ok(sendMailRES);

            }


           
            private async Task<Res> SendWelcomeMessageAsync(string userEmail, string userName)
            {
                try{
                    var from = new EmailAddress("venecitakmino243@gmail.com", "Luxon Scrum");
                    var subject = "Welcome To Luxon Scrum";
                    var to = new EmailAddress(userEmail, userName);
                    var plainTextContent = "We want to welcome you to a new way of Organizing your work!";
                    var htmlContent = "<p>We want to welcome you to a new way of Organizing your work!</p>";


                    
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                    var response = await _sendGridClient.SendEmailAsync(msg);

                    return new Res{Message="Success!", Data = response};
                } catch (Exception ex){
                    return new Res{Message = $"Error: {ex.Message}", Data = null};
                }

            }

            [AllowAnonymous]
            [HttpGet]
            [Route("listUsers")]
            [ProducesResponseType(200, Type = typeof(IEnumerable<Usuario>))]
            public IActionResult GetAllUSERS() 
            {
                // var tasks = _mapper.Map<List<Tasky>>(_TasksRepo.GetEveryoneTasks());
                var users = _AuthRepo.GetAllUsers();

                if (!ModelState.IsValid) return BadRequest(ModelState);

                return Ok(users);
            }
    }

     // [HttpPost]
            // [Route("testMAILS")]
            // public async Task<IActionResult> CheckMailService([FromBody] RegisterDTO userData)
            // {
            //     //check user
            //     if(userData == null) return BadRequest();

            //     //sending mail
            //     var sendMail = await SendWelcomeMessageAsync(userData.Email, userData.UserName);
            //     var res = new Res {
            //         Message="av",
            //         Data = sendMail
            //     };
                
            //     return Ok(res);
            // }
}