//using HRMSwebAPI.Data;
//using HRMSwebAPI.DTO;
//using HRMSwebAPI.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Data;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;

//namespace HRMSwebAPI.Services
//{
//    public class AuthService : IAuthService
//    {
//        private readonly AppDbContext context;
//        private readonly IConfiguration configuration;

//        public AuthService(AppDbContext context, IConfiguration configuration)
//        {
//            this.context = context;
//            this.configuration = configuration;
//        }

//        public async Task<User?> RegisterAsync(UserDto request)
//        {
//            if (context.Users.Any(u => u.Email == request.Email))
//            {
//                return null; // User already exists
//            }
//            var user = new User();
//            user.Email = request.Email;
//            //user.Name = request.Name;
//            user.PasswordHash = new PasswordHasher<User>()
//                .HashPassword(user, request.Password);
//            //user = new List<UserRole>();
//            //var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == request.role);

//            //if (role != null)
//            //{
//            //    user.Add(new UserRole
//            //    {
//            //        Id = user.Id,
//            //        RoleId = role.RoleId,
//            //        Role = role
//            //    });
//            //}
//            await context.Users.AddAsync(user);
//            await context.SaveChangesAsync();
//            return user;
//        }

//        public async Task<LoginResponseDto?> LoginAsync(LoginDto request)
//        {
//            User? user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

//            if (user is null || user.PasswordHash == null ||
//                new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password)
//                != PasswordVerificationResult.Success)
//            {
//                return null;
//            }

//            string token = GenerateToken(user);

//            return new LoginResponseDto
//            {
//                Token = token,
//                Email = user.Email,
//                Id = user.Id
//            };
//        }

//private string GenerateToken(User user)
//{
//    var roles = context.UserRoles
//       .Where(ur => ur.UserId == user.Id)
//       .Include(ur => ur.Role)
//       .Select(ur => ur.Role.Name)
//       .ToList();

//    var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                new Claim(ClaimTypes.Email, user.Email),
//                //new Claim(ClaimTypes.Role, user.UserRoles)
//            };

//    foreach (var role in roles)
//    {
//        claims.Add(new Claim(ClaimTypes.Role, role));
//    }

//    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
//    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//    var token = new JwtSecurityToken(
//        issuer: configuration["Jwt:Issuer"],
//        audience: configuration["Jwt:Audience"],
//        claims: claims,
//        expires: DateTime.Now.AddMinutes(30),
//        signingCredentials: creds
//    );
//    return new JwtSecurityTokenHandler().WriteToken(token);
//}
//    }
//}
