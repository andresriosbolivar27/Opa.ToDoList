using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opa.ToDoList.Dal;
using Opa.ToDoList.Entities.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opa.ToDoList.Web.Extensions
{
    public static class DbContextServicesExtension
    {
        public static IServiceCollection RegisterDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<OpaToDoListDataContext>(
                options =>
                {
                    options.UseSqlServer(OpaToDoListConfiguration.Instance.Connection);
                });

            return services;
        }
    }
}
