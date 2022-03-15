
using System.ComponentModel.DataAnnotations;

namespace Opa.ToDoList.Common.Models
{

    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
