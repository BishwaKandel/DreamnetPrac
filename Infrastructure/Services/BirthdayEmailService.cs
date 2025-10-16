using Application.Interface;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class BirthdayEmailService : IBirthdayEmailService
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;



        public BirthdayEmailService(AppDbContext context,
                                    IEmailService emailService,
                                    UserManager<User> userManager
                                    )
        {
            _emailService = emailService;
            _userManager = userManager;
            _context = context;

        }

        public async Task SendBirthdayEmailsAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tommorow = today.AddDays(1);

            List<User> todayBirthdayUsersList = await _context.Users.Where(u =>
                                                           u.DOB.Month == today.Month
                                                        && u.DOB.Day == today.Day
                                                       ).ToListAsync();

            List<User> tommorowBirthdayUsersList = await _context.Users.Where(u => 
                                                        u.DOB.Month == tommorow.Month
                                                        && u.DOB.Day == tommorow.Day
                                                       ).ToListAsync();

            List<User> admins = _userManager.GetUsersInRoleAsync("Admin").Result
                .Select(u => new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                })
                .ToList();

            // Notify admins about upcoming birthdays

            if (tommorowBirthdayUsersList.Count>0)
            {
                await _emailService.SendEmailAsync(
                string.Join(",", admins.Select(a => a.Email)),
                "Upcoming Birthdays",
               $"The following employees have birthdays tomorrow: {string.Join(", ", tommorowBirthdayUsersList.Select(u => u.Name))}.");
            }

            if (todayBirthdayUsersList.Count > 0)
            {
                foreach (var user in todayBirthdayUsersList)
                {
                    await _emailService.SendEmailAsync(
                        user.Email,
                        "Happy Birthday!",
                        $"Dear {user.Name}, we wish you a very Happy Birthday!"
                    );
                }

            }   
        }
    }
}



