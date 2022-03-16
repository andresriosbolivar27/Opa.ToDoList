
namespace Opa.ToDoList.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Opa.ToDoList.Dal;
    using Opa.ToDoList.Web.Helpers;
    using System.Threading.Tasks;

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
                    .Include(o => o.User)
                    .Include(t => t.Tasks));
            }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _dataContext.Owners
                .Include(u => u.User)
                .Include(t => t.Tasks)
                .ThenInclude(c => c.Category)
                .Include(c => c.Tasks)
                .ThenInclude(c => c.TaskState)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }
    }
    }
