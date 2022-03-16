
namespace Opa.ToDoList.Web.Data
{
    using Opa.ToDoList.Dal;
    using Opa.ToDoList.Entities.Business.Entities;
    using Opa.ToDoList.Web.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SeedDb
    {
        private readonly OpaToDoListDataContext context;
        private readonly IUserHelper userHelper;

        public SeedDb(
            OpaToDoListDataContext context, 
            IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();
            await CheckRoles();
            var owner = await CheckUserAsync("2022", "Andres Felipe", "Rios Bolivar", "nacional12344@gmail.com", "3111234567", "Caldas", "Owner");
            await CheckOwnersAsync(owner);
            await CheckTaskCategory();
            await CheckTaskStates();
        }

        private async Task CheckOwnersAsync(User user)
        {
            if (!this.context.Owners.Any())
            {
                this.context.Owners.Add(new Owner { User = user, });
                await this.context.SaveChangesAsync();
            }
        }

        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            string role)
        {
            var user = await userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document
                };

                await userHelper.AddUserAsync(user, "123456");
                await userHelper.AddUserToRoleAsync(user, role);

            }

            return user;
        }

        private async Task CheckOwnerTasks(User user)
        {
            if (!this.context.Tasks.Any())
            {
                this.context.Tasks.Add(new OpaTask()
                {
                    Category = await this.context.TaskCategories.FindAsync(1),
                    CompletedDate = DateTime.Now.AddDays(2),
                    CreatedDate = DateTime.Now,
                    Description = "Tarea 1",
                    TaskState = await this.context.TaskStates.FindAsync(1)
                });
                this.context.Tasks.Add(new OpaTask()
                {
                    Category = await this.context.TaskCategories.FindAsync(2),
                    CompletedDate = DateTime.Now.AddDays(3),
                    CreatedDate = DateTime.Now,
                    Description = "Tarea 2",
                    TaskState = await this.context.TaskStates.FindAsync(2)
                });
                this.context.Tasks.Add(new OpaTask()
                {
                    Category = await this.context.TaskCategories.FindAsync(3),
                    CompletedDate = DateTime.Now.AddDays(4),
                    CreatedDate = DateTime.Now,
                    Description = "Tarea 3",
                    TaskState = await this.context.TaskStates.FindAsync(3)
                });
                await this.context.SaveChangesAsync();
            }
        }

        private async Task CheckRoles()
        {
            await userHelper.CheckRoleAsync("Owner");
        }

        private async Task CheckTaskCategory()
        {
            if (!this.context.TaskCategories.Any())
            {
                this.context.TaskCategories.Add(new TaskCategory { Name = "Estudio" });
                this.context.TaskCategories.Add(new TaskCategory { Name = "Trabajo" });
                this.context.TaskCategories.Add(new TaskCategory { Name = "Compras" });

                await this.context.SaveChangesAsync();
            }
        }

        private async Task CheckTaskStates()
        {
            if (!this.context.TaskStates.Any())
            {
                this.context.TaskCategories.Add(new TaskCategory { Name = "Cancelada" });
                this.context.TaskCategories.Add(new TaskCategory { Name = "En progreso" });
                this.context.TaskCategories.Add(new TaskCategory { Name = "Completada" });

                await this.context.SaveChangesAsync();
            }
        }
    }
}
