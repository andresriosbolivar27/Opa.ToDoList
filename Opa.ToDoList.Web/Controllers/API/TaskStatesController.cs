using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opa.ToDoList.Dal;
using Opa.ToDoList.Entities.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opa.ToDoList.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStatesController : ControllerBase
    {
        private readonly OpaToDoListDataContext dataContext;

        public TaskStatesController(OpaToDoListDataContext context)
        {
            this.dataContext = context;
        }

        [HttpGet]
        public IEnumerable<TaskState> GetTaskStates()
        {
            return this.dataContext.TaskStates.OrderBy(pt => pt.Name);
        }
    }
}
