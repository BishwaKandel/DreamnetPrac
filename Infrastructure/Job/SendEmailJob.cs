using Application.Interface;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Infrastructure.Job
{
    public class SendEmailJob : IJob
    {
        private readonly IEmailService _emailService;
        public SendEmailJob(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _emailService.SendEmailAsync(body: "This is a test email body", subject: "Test Email Subject", to: "bisswakandel123@gmail.com");
            Console.WriteLine("Sending email...");
            return Task.CompletedTask;
        }
    }
}
