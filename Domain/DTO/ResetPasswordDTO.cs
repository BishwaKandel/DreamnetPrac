using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ForgotPasswordDTO
    {
        public string Email { get; set; }
    }

    public class ResetPasswordDTO
    {
        public string Email { get; set; }

        public string Token { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
