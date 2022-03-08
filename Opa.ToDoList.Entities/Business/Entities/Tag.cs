

namespace Opa.ToDoList.Entities.Business.Entities
{
    using System.Collections.Generic;

    public class Tag
    {
        public int Id { get; set; }
       
        public string Name { get; set; }

        public ICollection<Task> Tasks { get; set; }

    }
}
