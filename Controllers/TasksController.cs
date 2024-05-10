using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using TaskManagerServiceApi.Context;
using Task = TaskManagerServiceApi.Context.Task;

namespace TaskManagerServiceApi.Controllers
{
    // abreviatura para Task Manager "TM"
    [Authorize]
    [Route("TM/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskManagerDbContext _context;

        public TasksController(TaskManagerDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasksForCurrentUser()
        {
            // obtener el nombre de usuario del claim NameIdentifier
            var userID = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (userID == null)
            {
                // no se pudo encontrar el nombre de usuario en los claims del usuario actual
                return BadRequest("User not authenticated or username not found.");
            }

            // retornar las tareas del usuario con el nombre de usuario proporcionado
            var allTasks = await 
                (from task in _context.Tasks
                join priority in _context.Priorities on task.PriorityId equals priority.PriorityId
                join status in _context.TaskStatuses on task.StatusId equals status.StatusId
                join user in _context.Users on task.UserId equals user.UserId
                where (task.User.UserId == int.Parse(userID))
                select new Task
                {
                    TaskId = task.TaskId,
                    Title = task.Title,
                    Description = task.Description,
                    DueDate = task.DueDate,
                    PriorityId = task.PriorityId,
                    StatusId = task.StatusId,
                    UserId = task.UserId,
                    TaskStatus = status,
                    Priority = priority,
                    User = user
                }).ToListAsync();
            
          //  var userTasks = await _context.Tasks.Where(x => x.User.UserName == userName).ToListAsync();

            return allTasks;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTask(int id)
        {
            var userID = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userID == null)
            {
                return Unauthorized();
            }
            var task = await _context.Tasks.Where(task => task.TaskId == id && task.UserId == int.Parse(userID)).FirstOrDefaultAsync();
            return task;
        }
        
        [HttpGet("Title={Title}")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTaskWhere(string Title)
        {
            // obtener el nombre de usuario del claim NameIdentifier
            var userID = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (userID == null)
            {
                // no se pudo encontrar el nombre de usuario en los claims del usuario actual
                return BadRequest("User not authenticated or username not found.");
            }

            // retornar las tareas del usuario con el nombre de usuario proporcionado
            var allTasks = await 
                (from task in _context.Tasks
                    join priority in _context.Priorities on task.PriorityId equals priority.PriorityId
                    join status in _context.TaskStatuses on task.StatusId equals status.StatusId
                    join user in _context.Users on task.UserId equals user.UserId
                    where (task.User.UserId == int.Parse(userID) && task.Title == Title)
                    select new Task
                    {
                        TaskId = task.TaskId,
                        Title = task.Title,
                        Description = task.Description,
                        DueDate = task.DueDate,
                        PriorityId = task.PriorityId,
                        StatusId = task.StatusId,
                        UserId = task.UserId,
                        TaskStatus = status,
                        Priority = priority,
                        User = user
                    }).ToListAsync();
            
            //  var userTasks = await _context.Tasks.Where(x => x.User.UserName == userName).ToListAsync();

            return allTasks;
        }

        [HttpPut()]
        public async Task<IActionResult> PutTask(Task task)
        {
            var userID = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value; 
            
            // si la tarea a actualizar tiene un userId diferente al del token no borrar la tarea 
            if (userID == null)
            {
                return Unauthorized();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists((int)task.TaskId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
        [HttpPost]
        public async Task<ActionResult<Task>> PostTask(Task task)
        {
            var userID = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userID == null)
            {
                return Unauthorized();
            }
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = task.TaskId }, task);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userID = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var task = await _context.Tasks.FindAsync(id);
            if (task == null || userID == null)
            {
                return BadRequest("User not authorized or task not found");
            }

            try
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest("Error. Task not deleted to contact an administrator");
            }
        }
        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}
