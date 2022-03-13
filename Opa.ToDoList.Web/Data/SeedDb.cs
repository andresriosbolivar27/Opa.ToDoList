
namespace Opa.ToDoList.Web.Data
{
    using Opa.ToDoList.Dal;
    using Opa.ToDoList.Entities.Business.Entities;
    using Opa.ToDoList.Web.Helpers;
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
        }

        private async Task CheckOwnersAsync(User user)
        {
            if (!this.context.Owners.Any())
            {
                this.context.Owners.Add(new Owner { User = user });
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

        private async Task CheckRoles()
        {
            await userHelper.CheckRoleAsync("Owner");
        }
    }
}
