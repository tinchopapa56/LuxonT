using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


using AutoMapper;
using Application;
using Domain;
using Application.DTOs;

namespace LuxonTasks.Controllers
{
    // [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskyController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITaskyRepo _TasksRepo;
        public TaskyController(IMapper mapper, ITaskyRepo TasksRepo)
        {
            _TasksRepo = TasksRepo;
            _mapper = mapper;
        }
        //  SendGrid.

        // tests de los diferentes endpoints de la APP
            // - Campos faltantes o con un formato inválido en BODY de las peticiones
            // - Acceso a recursos inexistentes en endpoints de detalle
            // Los tests pueden realizarse utilizando UnitTesting.

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tasky>))]
        public IActionResult GetEveryoneTasks() 
        {
            var tasks = _mapper.Map<List<Tasky>>(_TasksRepo.GetEveryoneTasks());

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(tasks);
        }

        [Authorize]
        [HttpGet]
        [Route("myTasks")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tasky>))]
        public IActionResult GetAllMyTasks(string userId) 
        {
            var tasks = _TasksRepo.GetAllMyTasks(userId);
            if(tasks == null) return NotFound();

            // var tasksMAPPED = _mapper.Map<List<Tasky>>(tasks);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(tasks);
        }
       
        [HttpGet("{taskID}")]
        [ProducesResponseType(200, Type = typeof(Tasky))]
        public IActionResult GetTasky(Guid taskID)
        
        {
            var task = _TasksRepo.GetTask(taskID);
          if (task == null) return NotFound();
          if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(task);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTask([FromBody] TaskyDto task, string userID)
        {
            if (task == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var taskMAP = _mapper.Map<Tasky>(task);

           if (_TasksRepo.CreateTask(taskMAP, userID) == false)
           {
               ModelState.AddModelError("", "Something Went wrong while AHHH CONTROLLER saving");
               return StatusCode(500, ModelState);
           }
           return Ok("Succesfully created");
        }
        
        [HttpDelete("{taskID}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteTask([FromBody] Guid taskID)
        {
            if (taskID == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var taskDeleting = _TasksRepo.GetTask(taskID);

            if(!_TasksRepo.DeleteTask(taskDeleting)) 
            {
                return BadRequest();
            }
           
           return Ok("Deleted Succesfully");
        }

        [HttpPut("{taskID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTask(Guid taskID, [FromBody] TaskyDto updatedTask)
        {
           if (updatedTask == null) return BadRequest(ModelState);

        //    if (taskId != updatedTask.Id) return BadRequest(ModelState);

           if (_TasksRepo.GetTask(taskID) == null) return NotFound();

           if (!ModelState.IsValid) return BadRequest();

           var taskMap = _mapper.Map<Tasky>(updatedTask);

           if (!_TasksRepo.EditTask(taskMap))
           {
               ModelState.AddModelError("", "Something went wrong updating task");
               return StatusCode(500, ModelState);
           }

           return NoContent();

        }
    }
}
