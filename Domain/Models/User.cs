using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DOB { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public string Address { get; set; }
        public bool isActive { get; set; }
        public string? ProfilePictureFileName { get; set; }

        //public virtual ICollection<LeaveRequest> LeaveRequest { get; set; } = new List<LeaveRequest>();

    }
}
