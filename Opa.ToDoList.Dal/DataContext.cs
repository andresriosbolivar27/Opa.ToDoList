
namespace Opa.ToDoList.Dal
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Opa.ToDoList.Entities.Business.Entities;
    using Opa.ToDoList.Entities.Services.Configuration;

    public class OpaToDoListDataContext : IdentityDbContext<User>
    {
        public OpaToDoListDataContext()
        {
        }
        public OpaToDoListDataContext(DbContextOptions<OpaToDoListDataContext> options) : base(options)
        {
        }  

        public DbSet<Owner> Owners { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskCategory> TaskCategories { get; set; }
        public DbSet<TaskState> TaskStates { get; set; }
        public DbSet<Tag> Tags { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
            options.UseSqlServer(OpaToDoListConfiguration.Instance.Connection);
        }
    }
}
