using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IBirthdayEmailService
    {
        Task SendBirthdayEmailsAsync();

        //Task NotifyBirthdaytoAdminAsync(Guid? deptId);
    }
}
