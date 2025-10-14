using Application.Interface;
using AutoMapper;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public AttendanceService(AppDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ApiResponse<AttendanceDTO>> CheckInAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var today = DateTime.UtcNow.Date;

            if (user == null)
            {
                return new ApiResponse<AttendanceDTO>
                {
                    Data = null,
                    message = "Employee not found",
                    success = false
                };
            }
            var existingcheckIn = await _context.Attendances.FirstOrDefaultAsync(a => a.UserId == userId && a.Date == today);
            var exisitingcheckOut = await _context.Attendances.FirstOrDefaultAsync(a => a.UserId == userId && a.Date == today);
            if (existingcheckIn != null  )
            {                                                                                        
                return new ApiResponse<AttendanceDTO>
                {
                    Data = null,
                    message = "You are already checked in today",
                    success = false
                };
            }
            else if (exisitingcheckOut != null)
            {
                return new ApiResponse<AttendanceDTO>
                {
                    Data = null,
                    message = "You are already checked out today",
                    success = false
                };
            }

            AttendanceStatus status;

            var checkInTime = TimeOnly.FromDateTime(DateTime.Now);

            if (checkInTime > TimeOnly.Parse("09:00 AM"))
            {
                status = AttendanceStatus.Late;
            }
            else
            {
                status = AttendanceStatus.Present;
            }

            Domain.Models.Attendance newAttendance = new Domain.Models.Attendance
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Date = DateTime.UtcNow.Date,
                CheckInTime = TimeOnly.FromDateTime(DateTime.Now),
                CheckOutTime = null,
                TotalHoursWorked = null,
                status = status
            };

            _context.Attendances.Add(newAttendance);
            await _context.SaveChangesAsync();
            var attendanceDto = _mapper.Map<AttendanceDTO>(newAttendance);
            return new ApiResponse<AttendanceDTO>
            {
                Data = attendanceDto,
                message = "Check-in successful",
                success = true
            };
        }

        public async Task<ApiResponse<AttendanceDTO>> CheckOutAsync(string userId )
        {
            var emp = await _userManager.FindByIdAsync(userId);
            if (emp == null)
            {
                return new ApiResponse<AttendanceDTO>
                {
                    Data = null,
                    message = "Employee not found",
                    success = false
                };
            }
            var today = DateTime.UtcNow.Date;
            var existingCheckIn = await _context.Attendances.FirstOrDefaultAsync(a => a.UserId == userId && a.Date == today);
            if (existingCheckIn == null)
            {
                return new ApiResponse<AttendanceDTO>
                {
                    Data = null,
                    message = "You haven't checked in today",
                    success = false
                };
            }
            if (existingCheckIn.CheckOutTime != null)
            {
                return new ApiResponse<AttendanceDTO>
                {
                    Data = null,
                    message = "You are already checked out today",
                    success = false
                };
            }
            existingCheckIn.CheckOutTime = TimeOnly.FromDateTime(DateTime.Now);
            existingCheckIn.TotalHoursWorked = existingCheckIn.CheckOutTime.Value.ToTimeSpan() - existingCheckIn.CheckInTime.ToTimeSpan();
            await _context.SaveChangesAsync();
            var attendanceDto = _mapper.Map<AttendanceDTO>(existingCheckIn);
            return new ApiResponse<AttendanceDTO>
            {
                Data = attendanceDto,
                message = "Check-out successful",
                success = true
            }; 
        }

        public async Task<ApiResponse<UserAttendanceDTO>> GetAttendancesByEmpIdAsync(string userId)
        {
           var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse<UserAttendanceDTO>
                {
                        Data = null,
                        message = "Employee not found",
                        success = false 
                };
            }
            var attendances = await _context.Attendances.Where(a => a.UserId == userId).ToListAsync();
           

            var userAttendanceDTO = new UserAttendanceDTO
            {
                userId = user.Id,
                Name = user.Name,
                attendances = _mapper.Map<List<AttendanceDTO>>(attendances)
            };

            return new ApiResponse<UserAttendanceDTO>
            {
                Data = userAttendanceDTO,
                message = "Attendances retrieved successfully",
                success = true
            };
        }
    }
}
