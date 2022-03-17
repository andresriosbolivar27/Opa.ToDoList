using System;
using System.Collections.Generic;
using System.Text;

namespace Opa.ToDoList.Common.Models
{
    public class TaskRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime CompletedDate { get; set; }

        public int CategoryId { get; set; }

        public int TaskStateId { get; set; }
        public int OwnerId { get; set; }

    }
}
