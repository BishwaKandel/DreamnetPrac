//using Domain.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using System;

//namespace Infrastructure.Data
//{
//    public static class ModelBuilderExtensions
//    {
//        public static void Seed(this ModelBuilder modelBuilder)
//        {
//            string adminRoleId = Guid.NewGuid().ToString();
//            string userRoleId = Guid.NewGuid().ToString();

//            modelBuilder.Entity<IdentityRole>().HasData(
//                new IdentityRole
//                {
//                    Id = adminRoleId,
//                    Name = "Admin",
//                    NormalizedName = "ADMIN"
//                },
//                new IdentityRole
//                {
//                    Id = userRoleId,
//                    Name = "User",
//                    NormalizedName = "USER"
//                }
//            );

//            string adminUserId = Guid.NewGuid().ToString();

//            var hasher = new PasswordHasher<User>();
//            var admin = new User
//            {
//                Id = adminUserId,
//                UserName = "admin@gmail.com",
//                NormalizedUserName = "ADMIN@GMAIL.COM",
//                Email = "admin@gmail.com",
//                NormalizedEmail = "ADMIN@GMAIL.COM",
//                EmailConfirmed = true,
//                Name = "System Admin",
//                FirstName = "System",
//                LastName = "Admin",
//                DOB = new DateTime(1990, 1, 1),
//                JoiningDate = DateTime.UtcNow,
//                Position = "Administrator",
//                Salary = 0,
//                Address = "Head Office",
//                IsActive = true,
//                IsDeleted = false,
//                DepartmentId = null,
//                SecurityStamp = Guid.NewGuid().ToString()
//            };

//            admin.PasswordHash = hasher.HashPassword(admin, "admin123");

//            modelBuilder.Entity<User>().HasData(admin);

//            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
//                new IdentityUserRole<string>
//                {
//                    RoleId = adminRoleId,
//                    UserId = adminUserId
//                }
//            );
//        }
//    }
//}
