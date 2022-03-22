using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Opa.ToDoList.Common.Models;
using Opa.ToDoList.Dal;
using Opa.ToDoList.Entities.Business.Entities;
using Opa.ToDoList.Web.Helpers;
using System;
using System.Linq;
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
                var user = await this.userHelper.GetUserByEmailAsync(request.Email);

                if (user == null)
                {
                    return BadRequest("Usuario no encontrado.");
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

        [HttpPost]
        [Route("RegisterOwner")]
        public async Task<IActionResult> RegisterOwner([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.userHelper.GetUserByEmailAsync(request.Email);
            if (user != null)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "Este correo ya se encuentra registrado."
                });
            }

            user = new User
            {
                Address = request.Address,
                Document = request.Document,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.Phone,
                UserName = request.Email
            };

            var result = await this.userHelper.AddUserAsync(user, request.Password);
            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors.FirstOrDefault().Description);
            }

            var nuevoUsuario = await this.userHelper.GetUserByEmailAsync(request.Email);

            await this.userHelper.AddUserToRoleAsync(nuevoUsuario, "Owner");
            
            this.dataContext.Owners.Add(new Owner { User = nuevoUsuario });

            await this.dataContext.SaveChangesAsync();


            return Ok(new Response<object>
            {
                IsSuccess = true,
                Message = "Usuario registrado con éxito."
            });
        }

        [HttpPut]
        public async Task<IActionResult> PutUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEntity = await this.userHelper.GetUserByEmailAsync(request.Email);
            if (userEntity == null)
            {
                return BadRequest("Usuario no encontrado.");
            }

            userEntity.FirstName = request.FirstName;
            userEntity.LastName = request.LastName;
            userEntity.Address = request.Address;
            userEntity.PhoneNumber = request.Phone;
            userEntity.Document = request.Document;

            var respose = await this.userHelper.UpdateUserAsync(userEntity);
            if (!respose.Succeeded)
            {
                return BadRequest(respose.Errors.FirstOrDefault().Description);
            }

            var updatedUser = await this.userHelper.GetUserByEmailAsync(request.Email);
            return Ok(updatedUser);
        }
    }
}
