using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class DatabaseSeeder
    {
        private readonly AppDbContext _context;

        public DatabaseSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await SeedRolesAsync();
            await SeedAdminUserAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (!await _context.Roles.AnyAsync())
            {
                var roles = new[]
                {
                    new IdentityRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new IdentityRole
                    {
                        Name = "User",
                        NormalizedName = "USER"
                    }
                };

                await _context.Roles.AddRangeAsync(roles);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedAdminUserAsync()
        {
            if (!await _context.Users.AnyAsync(u => u.Email == "admin@gmail.com"))
            {
                var hasher = new PasswordHasher<User>();
                var admin = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = true,
                    Name = "System Admin",
                    FirstName = "System",
                    LastName = "Admin",
                    DOB = new DateTime(1990, 1, 1),
                    JoiningDate = DateTime.UtcNow,
                    Position = "Administrator",
                    Salary = 10000,
                    Address = "Head Office",
                    IsActive = true,
                    IsDeleted = false,
                    DepartmentId = null,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                admin.PasswordHash = hasher.HashPassword(admin, "admin123");

                await _context.Users.AddAsync(admin);
                await _context.SaveChangesAsync();

                // Ensure role exists
                var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.NormalizedName == "ADMIN");
                if (adminRole != null)
                {
                    _context.UserRoles.Add(new IdentityUserRole<string>
                    {
                        UserId = admin.Id,
                        RoleId = adminRole.Id
                    });

                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
