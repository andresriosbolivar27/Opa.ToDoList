

namespace Opa.ToDoList.Entities.Business.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class TaskState
    {
        public int Id { get; set; }

        [Display(Name = "Property Type")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Name { get; set; }

        public ICollection<OpaTask> Tasks { get; set; }
    }
}
