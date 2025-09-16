using Domain.Models;
//using Domain.Models.JoinTables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext
    {

        public DbSet<User> Users { get; set; } = null!;
        //public DbSet<Role> Roles { get; set; } = null!;
        //public DbSet<UserRole> UserRoles { get; set; } = null!;
        //public DbSet<Attendance> Attendances { get; set; } = null!;
        //public DbSet<LeaveRequest> LeaveRequests { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        //public DbSet<Payroll> Payrolls { get; set; } = null!;

        //public DbSet<DeptEmp> DeptEmps { get; set; } = null!;
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }


    }
}
