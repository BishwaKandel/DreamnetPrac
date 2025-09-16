//using HRMSwebAPI.Models;
//using Microsoft.EntityFrameworkCore;

//namespace HRMSwebAPI.Data
//{
//    public class AppDbContext : DbContext
//    {
//        public DbSet<User> Users { get; set; } = null!;
//        public DbSet<Role> Roles { get; set; } = null!;
//        public DbSet<UserRole> UserRoles { get; set; } = null!;

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            modelBuilder.Entity<Role>()
//                .HasData(
//                new Role { RoleId = 1, Name = "Admin" },
//                new Role { RoleId = 2, Name = "User" }
//                );
//        }
//        public AppDbContext(DbContextOptions<AppDbContext> options)
//            : base(options)
//        {

//        }
//    }
//}
