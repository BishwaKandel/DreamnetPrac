﻿using System.ComponentModel.DataAnnotations;

namespace HRMSmvc.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]

        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
