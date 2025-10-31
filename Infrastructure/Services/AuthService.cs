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
        private readonly IEmailService _emailService;

        public AuthService(UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           IConfiguration configuration, RoleManager<IdentityRole> roleManager,
                           AppDbContext context , IImageService imageService, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _context = context;
            _imageService = imageService;
            _emailService = emailService;
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
            var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (existingUser != null)
            {
                return new ApiResponse<string>
                {
                    success = false,
                    message = "Email is already registered"
                };
            }
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
                    IsActive = false,
                    IsDeleted = false,
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
            if (user == null || user.IsDeleted==true)
                return new ApiResponse<LoginResponseDto>
                {
                    success = false,
                    message = "User not found"
                };
            if (user.IsActive == false)
                return new ApiResponse<LoginResponseDto>
                {
                    success = false,
                    message = "User is not active. Please contact admin."
                };
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
                new Claim(ClaimTypes.Name, user.Name??user.Email),
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

        public async Task<ApiResponse<string>> SendPasswordResetLinkAsync(ForgotPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    success = false,
                    message = "No user found with that email."
                };
            }

            // Generate reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Encode token for URL safety
            var encodedToken = System.Net.WebUtility.UrlEncode(token);

            string baseurl = _configuration["WebSettings:BaseUrl"];

            var resetLink = $"{baseurl}/Auth/ResetPassword?email={model.Email}&token={encodedToken}";
            // after computing `resetLink` string...

            // optional: a safely escaped user display name
            var displayName = string.IsNullOrWhiteSpace(user.Name) ? user.Email : System.Net.WebUtility.HtmlEncode(user.Name);

            // HTML email (inline CSS, button, fallback link)
            string htmlBody = $@"
                        <!doctype html>
                        <html>
                        <head>
                          <meta charset='utf-8'>
                          <meta name='viewport' content='width=device-width, initial-scale=1.0'/>
                          <title>Reset your password</title>
                        </head>
                        <body style='margin:0;padding:0;background:#f4f4f5;color:#333;font-family:Arial,Helvetica,sans-serif;'>
                          <table width='100%' cellpadding='0' cellspacing='0' role='presentation'>
                            <tr>
                              <td align='center' style='padding:20px 10px;'>
                                <table width='600' cellpadding='0' cellspacing='0' role='presentation' style='background:#ffffff;border-radius:8px;overflow:hidden;'>
                                  <tr>
                                    <td style='padding:28px 32px; text-align:left;'>
                                      <h2 style='margin:0 0 8px 0;font-size:20px;color:#111;'>Hi {displayName},</h2>
                                      <p style='margin:0 0 18px 0;line-height:1.5;color:#555;font-size:14px;'>
                                        We received a request to reset the password for your account. Click the button below to set a new password.
                                      </p>

                                      <div style='text-align:center;margin:22px 0;'>
                                        <a href='{resetLink}' target='_blank'
                                           style='display:inline-block;padding:12px 24px;background:#0d6efd;color:#ffffff;text-decoration:none;border-radius:6px;font-weight:600;'>
                                          Reset Password
                                        </a>
                                      </div>

                                      <p style='margin:0 0 12px 0;color:#777;font-size:13px;line-height:1.5;'>
                                        If the button above doesn't work, copy and paste the following URL into your browser:
                                      </p>

                                      <p style='word-break:break-all; font-size:12px; color:#0d6efd;'>
                                        <a href='{resetLink}' target='_blank' style='color:#0d6efd;text-decoration:underline;'>{resetLink}</a>
                                      </p>

                                      <hr style='border:none;border-top:1px solid #eee;margin:20px 0;' />
                                    </td>
                                  </tr>

                                  <tr>
                                    <td style='background:#fafafa;padding:14px 32px;font-size:12px;color:#888;text-align:center;'>
                                      &copy; {DateTime.UtcNow.Year} HRMS. All rights reserved.
                                    </td>
                                  </tr>
                                </table>
                              </td>
                            </tr>
                          </table>
                        </body>
                        </html>
                        ";

            // send as HTML
            await _emailService.SendEmailAsync(model.Email, "Reset your password", htmlBody);

            return new ApiResponse<string>
            {
                success = true,
                message = "Password reset link has been sent to your email."
            };
        }

        public async Task<ApiResponse<string>> ResetPassword(ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    success = false,
                    message = "Invalid request. User not found."
                };
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                return new ApiResponse<string>
                {
                    success = false,
                    message = "Passwords do not match."
                };
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                return new ApiResponse<string>
                {
                    success = true,
                    message = "Password has been reset successfully."
                };
            }

            return new ApiResponse<string>
            {
                success = false,
                message = string.Join("; ", result.Errors.Select(e => e.Description))
            };
        }
    }
}

    
    

