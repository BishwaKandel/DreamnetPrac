﻿namespace HRMSwebAPI.DTO
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;    
        public string Email { get; set; } = string.Empty;
        public Guid Id { get; set; }
    }
}
