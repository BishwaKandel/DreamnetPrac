using Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : Controller
    {
        private readonly IEmailService _email;
        public EmailController(IEmailService email)
        {
            _email = email;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmails()
        {
            await _email.SendEmailAsync(body: "This is a test email body", subject: "Test Email Subject", to: "bisswakandel123@gmail.com");
            return Ok("Email sent.");
        }
    }
}
