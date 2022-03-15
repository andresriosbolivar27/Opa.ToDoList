
namespace Opa.ToDoList.Common.Models
{
    using Opa.ToDoList.Entities.Business.Entities;
    using System.Collections.Generic;

    public class OwnerResponse
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Document { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public ICollection<OpaTask> Tasks { get; set; }

        public int RoleId { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
