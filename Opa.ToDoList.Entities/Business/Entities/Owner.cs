
namespace Opa.ToDoList.Entities.Business.Entities
{
    using System.Collections.Generic;

    public class Owner
    {
        public int Id { get; set; }

        public User User { get; set; }

        public ICollection<OpaTask> Tasks { get; set; }
    }
}
