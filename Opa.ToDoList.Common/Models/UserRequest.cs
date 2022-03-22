namespace Opa.ToDoList.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserRequest
    {
        [Required]
        public string Document { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
       
        public string Phone { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }
       
    }
}
