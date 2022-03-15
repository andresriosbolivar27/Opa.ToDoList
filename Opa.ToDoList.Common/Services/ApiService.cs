using Newtonsoft.Json;
using Opa.ToDoList.Common.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Opa.ToDoList.Common.Services
{
    public class ApiService : IApiService
    {
        public async Task<Response<OwnerResponse>> GetOwnerByEmailAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string email)
        {
            try
            {
                var request = new EmailRequest { Email = email };
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),
                    
                };
                var url = $"{servicePrefix}{controller}";
                
                var response = await client.PostAsync(url,content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<OwnerResponse>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var owner = JsonConvert.DeserializeObject<OwnerResponse>(result);
                return new Response<OwnerResponse>
                {
                    IsSuccess = true,
                    Result = owner
                };
            }
            catch (Exception ex)
            {
                return new Response<OwnerResponse>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<string>> ValidatePassword(
            string urlBase,
            string servicePrefix,
            string controller,
            string email, 
            string password)
        {
            try
            {
                var request =  new ValidateRequest 
                {
                    Email = email,
                    Password = password
                };
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),

                };
                var url = $"{servicePrefix}{controller}";

                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<string>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var validateResponse = JsonConvert.DeserializeObject<OwnerResponse>(result);
                return new Response<string>
                {
                    IsSuccess = true,
                    Result = string.Empty
                };
            }
            catch (Exception ex)
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
