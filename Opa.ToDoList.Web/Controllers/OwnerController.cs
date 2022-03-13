
namespace Opa.ToDoList.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Opa.ToDoList.Dal;
    using Opa.ToDoList.Web.Helpers;

        public class OwnersController : Controller
        {
            private readonly OpaToDoListDataContext _dataContext;
            private readonly IUserHelper _userHelper;

            public OwnersController(
                OpaToDoListDataContext dataContext,
                IUserHelper userHelper)
            {
                _dataContext = dataContext;
                _userHelper = userHelper;
            }

            public IActionResult Index()
            {
                return View(_dataContext.Owners
                    .Include(o => o.User));
            }
        }
    }
