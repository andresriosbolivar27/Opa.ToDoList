using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Opa.ToDoList.Common.Models;
using Opa.ToDoList.Dal;
using Opa.ToDoList.Web.Helpers;
using System;
using System.Threading.Tasks;

namespace Opa.ToDoList.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly OpaToDoListDataContext dataContext;
        private readonly IUserHelper userHelper;

        public AccountController(
            OpaToDoListDataContext dataContext,
            IUserHelper userHelper)
        {
            this.dataContext = dataContext;
            this.userHelper = userHelper;
        }

        [HttpPost]
        [Route("ValidatePassword")]
        public async Task<IActionResult> ValidatePassword(ValidateRequest request)
        {
            try
            {
                //var userRequest = JsonConvert.DeserializeObject<ValidateRequest>(request);
                var user = await this.userHelper.GetUserByEmailAsync(request.Email);

                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                if (await this.userHelper.IsUserInRoleAsync(user, "Owner"))
                {
                    var result = await userHelper.ValidatePasswordAsync(
                       user,
                       request.Password);
                    
                    if (result.Succeeded)
                    {
                        return Ok(new Response<string>
                        {
                            IsSuccess = true,
                            
                        });
                    };                    
                }

                return BadRequest(new Response<string> 
                {
                    IsSuccess = false
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }
    }
}
