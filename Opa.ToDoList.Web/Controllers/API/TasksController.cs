using Microsoft.AspNetCore.Mvc;
using Opa.ToDoList.Common.Models;
using Opa.ToDoList.Dal;
using Opa.ToDoList.Entities.Business.Entities;
using Opa.ToDoList.Web.Helpers;
using System;
using System.Threading.Tasks;

namespace Opa.ToDoList.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly OpaToDoListDataContext dataContext;
        private readonly IUserHelper userHelper;

        public TasksController(
            OpaToDoListDataContext dataContext,
            IUserHelper userHelper)
        {
            this.dataContext = dataContext;
            this.userHelper = userHelper;
        }

        [HttpPost]
        [Route("RegisterTask")]
        public async Task<IActionResult> RegisterTask([FromBody] TaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var owner = await this.dataContext.Owners.FindAsync(request.OwnerId);
            if (owner == null)
            {
                return BadRequest("Usuario no valido.");
            }

            var taskCategory = await this.dataContext.TaskCategories.FindAsync(request.CategoryId);
            if (taskCategory == null)
            {
                return BadRequest("Categoria no valida.");
            }

            var taskState = await this.dataContext.TaskStates.FindAsync(request.TaskStateId);
            if (taskState == null)
            {
                return BadRequest("Categoria no valida.");
            }

            var task = new OpaTask
            {
                Category = taskCategory,
                CreatedDate = DateTime.Now,
                Description = request.Description,
                Name = request.Name,
                Owner = owner,
                TaskState = taskState
            };

            this.dataContext.Tasks.Add(task);

            await this.dataContext.SaveChangesAsync();

            return Ok(new Response<object>
            {
                IsSuccess = true,
                Message = "Tarea registrada con éxito."
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask([FromRoute] int id, [FromBody] TaskRequest  request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            if (id != request.Id)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var owner = await this.dataContext.Owners.FindAsync(request.OwnerId);
            if (owner == null)
            {
                return BadRequest("Usuario no valido.");
            }

            var oldTask = await this.dataContext.Tasks.FindAsync(request.Id);
            if (oldTask == null)
            {
                return BadRequest("Tarea no existe.");
            }

            var taskCategory = await this.dataContext.TaskCategories.FindAsync(request.CategoryId);
            if (taskCategory == null)
            {
                return BadRequest("Categoria no valida.");
            }

            var taskState = await this.dataContext.TaskStates.FindAsync(request.TaskStateId);
            if (taskState == null)
            {
                return BadRequest("Categoria no valida.");
            }

            oldTask.Category = taskCategory;
            oldTask.CreatedDate = request.CreatedDate;
            oldTask.Description = request.Description;
            oldTask.Name = request.Name;
            oldTask.Owner = owner;
            oldTask.TaskState = taskState;
            oldTask.CompletedDate = request.CompletedDate;
            oldTask.Archived = request.Archived;


            this.dataContext.Tasks.Update(oldTask);

            await this.dataContext.SaveChangesAsync();

            return Ok(new Response<object>
            {
                IsSuccess = true,
                Message = "Tarea actualizada con éxito."
            });
        }
    }
}
