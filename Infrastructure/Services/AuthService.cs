using Application.Interface;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        //private readonly AppDbContext context;
        //private readonly IConfiguration configuration;

        //public AuthService(AppDbContext context, IConfiguration configuration)
        //{
        //    this.context = context;
        //    this.configuration = configuration;
        //}


        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IImageService _imageService;


        public AuthService(UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           IConfiguration configuration, RoleManager<IdentityRole> roleManager,
                           AppDbContext context , IImageService imageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _context = context;
            _imageService = imageService;
        }
        public async Task<ApiResponse<string>> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO.Password != registerDTO.ConfirmPassword)
                return new ApiResponse<string>
                {
                    success = false,
                    message = "Passwords do not match"
                };
            //if(registerDTO.ProfilePicture!= null)
            //{
            //    string folder = "images/ProfilePicture";

            //}
                var user = new User
                {

                    Email = registerDTO.Email,
                    UserName = registerDTO.UserName,
                    Address = registerDTO.Address,
                    DOB = registerDTO.DOB,
                    Position = registerDTO.Position,
                    JoiningDate = registerDTO.JoiningDate,
                    PhoneNumber = registerDTO.PhoneNumber,
                    Name = registerDTO.Name,
                    ProfilePictureFileName = registerDTO.ProfilePictureFileName,
                    isActive = false
                };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
                return new ApiResponse<string>
                {
                    success = false,
                    message = string.Join(" ", result.Errors.Select(e => e.Description))
                };
            var roleName = Role.User.ToString();
            //await  CreateRoleAsync(roleName);
            //await _userManager.AddToRoleAsync(user, roleName);
            // Optionally assign default role
            await _userManager.AddToRoleAsync(user, "User");

            return new ApiResponse<string>
            {
                success = true,
                message = "User registered successfully"
            };
        }
        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
                return new ApiResponse<LoginResponseDto>
                {
                    success = false,
                    message = "User not found"
                };
            //if (user.isActive == false)
            //    return new ApiResponse<LoginResponseDto>
            //    {
            //        success = false,
            //        message = "User is not active. Please contact admin."
            //    };
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded)
                return new ApiResponse<LoginResponseDto>
                {
                    success = false,
                    message = "Invalid Login attempt"

                };

            var token = await GenerateToken(user);  
            var roles = await _userManager.GetRolesAsync(user);
            return new ApiResponse<LoginResponseDto>
            {
                success = true,
                message = "Login Successful",
                Data = new LoginResponseDto
                {
                    Token = token,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    Id = user.Id,
                    Name = user.Name
                }
            };
        }
        public Task LogoutAsync()
        {
            // Implementation for user logout
            throw new NotImplementedException();
        }

        //Role Creation logic
        public async Task<string> CreateRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    return string.Join(", ", result.Errors.Select(e => e.Description));

                }
                return $"Role Created {roleName}";
            }
            return $"Role Exists {roleName}";
        }

        //Token Generation logic 

        private async Task<string> GenerateToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);



            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role , role));
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //Logout 

        public async Task<ApiResponse<string>> LogoutUserAsync()
        {

            await _signInManager.SignOutAsync(); // Sign out the user from the current session

            return new ApiResponse<string>
            {
                success = true,
                message = "You are Logged out !"
            };
        }
    }
}
    
    

