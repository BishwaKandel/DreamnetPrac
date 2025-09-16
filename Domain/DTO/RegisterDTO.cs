using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class RegisterDTO
    {
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public IFormFile formFile { get; set; }
        public string? ProfilePictureFileName { get; set; } 
        public string? Password { get; set; } 
        public string? ConfirmPassword { get; set; }
        
    }
    public enum Role
    {
        Admin,
        User
    }
    
}
