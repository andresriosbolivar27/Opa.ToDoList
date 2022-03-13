using Microsoft.AspNetCore.Identity;
using Opa.ToDoList.Entities.Business.Entities;
using Opa.ToDoList.Web.Models;
using System.Threading.Tasks;

namespace Opa.ToDoList.Web.Helpers
{
    public interface IUserHelper
    {
        Task<User> AddUser(AddUserViewModel view, string role);
        Task<IdentityResult> AddUserAsync(User user, string password);
        System.Threading.Tasks.Task AddUserToRoleAsync(User user, string roleName);
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        System.Threading.Tasks.Task CheckRoleAsync(string roleName);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<bool> DeleteUserAsync(string email);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(string userId);
        Task<bool> IsUserInRoleAsync(User user, string roleName);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        System.Threading.Tasks.Task LogoutAsync();
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<SignInResult> ValidatePasswordAsync(User user, string password);
    }
}