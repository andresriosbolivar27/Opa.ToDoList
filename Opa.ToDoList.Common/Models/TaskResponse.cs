using Opa.ToDoList.Entities.Business.Entities;
using System;
using System.Collections.Generic;

namespace Opa.ToDoList.Common.Models
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Archived { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public TaskCategory Category { get; set; }
        public TaskState TaskState { get; set; }
        public Owner Owner { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
