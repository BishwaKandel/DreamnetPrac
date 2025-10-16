using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> RegisterAsync(RegisterDTO registerDTO);
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDTO loginDTO);

        Task<string> CreateRoleAsync(string roleName);

        Task<ApiResponse<string>> LogoutUserAsync();
    }
}
