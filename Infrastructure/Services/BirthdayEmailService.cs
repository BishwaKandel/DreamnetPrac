using Application.Interface;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class BirthdayEmailService : IBirthdayEmailService
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmailService _emailService;
        private readonly BirthdayService _birthdayService;

        public BirthdayEmailService(IEmployeeService employeeService,
                                    IEmailService emailService,
                                    BirthdayService birthdayService)
        {
            _employeeService = employeeService;
            _emailService = emailService;
            _birthdayService = birthdayService;
        }

        public async Task SendBirthdayEmailsAsync(Guid deptId)
        {
            var response = await _employeeService.GetAllEmployeesAsync(deptId);
            if (!response.success || response.Data == null)
            {
                throw new InvalidOperationException("Failed to fetch employee data or data is null.");
            }

            var today = DateTime.UtcNow.Date;

            foreach (var user in response.Data)
            {
                if (_birthdayService.IsBirthdayToday(user, today))
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
