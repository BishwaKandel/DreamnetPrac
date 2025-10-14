using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class UserDTO    
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Position { get; set; } 
        public decimal Salary { get; set; }
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public IFormFile? formFile { get; set; }

        public string? ProfilePictureFileName { get; set; }

    }


    public class UserUpdateDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime? DOB { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string? Position { get; set; }
        public decimal? Salary { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        //public string? ProfilePictureFileName { get; set; }
        //public IFormFile? formFile { get; set; }

    }

    public class ChangePpDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? ProfilePictureFileName { get; set; }
        public IFormFile? formFile { get; set; }
    }

   
    public class ChangePasswordDTO
    {
        public string Id { get; set; }

        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }

    }
}
