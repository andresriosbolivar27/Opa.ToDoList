using Opa.ToDoList.Common.Models;
using System.Threading.Tasks;

namespace Opa.ToDoList.Common.Services
{
    public interface IApiService
    {
        Task<Response<OwnerResponse>> GetOwnerByEmailAsync(string urlBase, string servicePrefix, string controller, string email);
        Task<Response<string>> ValidatePassword(string url, string v1, string v2, string email, string password);
    }
}