
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
            await CheckTaskCategory();
            await CheckTaskStates();
            await CheckOwnersAsync(owner);

        }

        private async Task CheckOwnersAsync(User user)
        {
            if (!this.context.Owners.Any())
            {
                this.context.Owners.Add(new Owner
                {
                    User = user,
                    Tasks = CheckOwnerTasks(user),
                });
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

        private ICollection<OpaTask> CheckOwnerTasks(User user)
        {
            return new List<OpaTask>
            {
                new OpaTask()
                {
                    Name = "Tarea 1",
                    Category = this.context.TaskCategories.FirstOrDefault(c => c.Id == 1),
                    CompletedDate = DateTime.Now.AddDays(2),
                    CreatedDate = DateTime.Now,
                    Description = "Esta es la tarea 1",
                    TaskState = this.context.TaskStates.FirstOrDefault(t => t.Id == 1),
                    Owner = new Owner
                    {
                        User = user
                    },
                    Archived = false
                },
                new OpaTask()
                {
                    Name = "Tarea 2",
                    Category = this.context.TaskCategories.FirstOrDefault(c => c.Id == 2),
                    CompletedDate = DateTime.Now.AddDays(3),
                    CreatedDate = DateTime.Now,
                    Description = "Esta es la tarea 2",
                    TaskState = this.context.TaskStates.FirstOrDefault(t => t.Id == 2),
                    Owner = new Owner
                    {
                        User = user
                    },
                    Archived = true
                },
                new OpaTask()
                {
                    Name = "Tarea 3",
                    Category = this.context.TaskCategories.FirstOrDefault(c => c.Id == 3),
                    CompletedDate = DateTime.Now.AddDays(4),
                    CreatedDate = DateTime.Now,
                    Description = "Esta es la tarea 3",
                    TaskState = this.context.TaskStates.FirstOrDefault(t => t.Id == 3),
                    Owner = new Owner
                    {
                        User = user
                    },
                    Archived = false
                }
            };
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
                this.context.TaskStates.Add(new TaskState { Name = "Cancelada" });
                this.context.TaskStates.Add(new TaskState { Name = "En progreso" });
                this.context.TaskStates.Add(new TaskState { Name = "Completada" });

                await this.context.SaveChangesAsync();
            }
        }
    }
}
