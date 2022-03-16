
using System;
using System.Collections.Generic;

namespace Opa.ToDoList.Entities.Business.Entities
{
    public class OpaTask
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime CompletedDate { get; set; }

        public TaskCategory Category { get; set; }

        public TaskState TaskState { get; set; }
        public Owner Owner { get; set; }

        public ICollection<Tag> Tags { get; set; }
    }
}
