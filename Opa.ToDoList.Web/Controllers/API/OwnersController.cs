
namespace Opa.ToDoList.Web.Controllers.API
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Opa.ToDoList.Common.Models;
    using Opa.ToDoList.Dal;
    using Opa.ToDoList.Web.Helpers;
    using System;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly OpaToDoListDataContext dataContext;
        private readonly IUserHelper userHelper;

        public OwnersController(
            OpaToDoListDataContext dataContext,
            IUserHelper userHelper)
        {
            this.dataContext = dataContext;
            this.userHelper = userHelper;
        }

        [HttpPost]
        [Route("GetOwnerByEmail")]
        public async Task<IActionResult> GetOwner(EmailRequest emailRequest)
        {
            try
            {
                var user = await this.userHelper.GetUserByEmailAsync(emailRequest.Email);

                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                if (await this.userHelper.IsUserInRoleAsync(user, "Owner"))
                {
                    return await GetOwnerAsync(emailRequest);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private async Task<IActionResult> GetOwnerAsync(EmailRequest emailRequest)
        {
            var owner = await this.dataContext.Owners
                .Include(u => u.User)
                .FirstOrDefaultAsync(o => o.User.UserName.ToLower().Equals(emailRequest.Email.ToLower()));

            var response = new OwnerResponse
            {
                RoleId = 1,
                Id = owner.Id,
                FirstName = owner.User.FirstName,
                LastName = owner.User.LastName,
                Address = owner.User.Address,
                Document = owner.User.Document,
                Email = owner.User.Email,
                PhoneNumber = owner.User.PhoneNumber,
            };

            return Ok(response);
        }
    }

}
