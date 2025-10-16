using Application.Interface;
using AutoMapper;
using Azure;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public LeaveService(AppDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ApiResponse<LeaveRequestDTO>> CreateLeaveAsync(LeaveRequestDTO leave , string userId)
        {
            LeaveRequest leaveRequest = new LeaveRequest
            {
                Id = Guid.NewGuid(),
                RequestedById = userId,
                Description = leave.Description,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Reason = leave.Reason,
                AppliedOn = DateOnly.FromDateTime(DateTime.Now),
                LeaveType = leave.LeaveType,
                Status = LeaveStatus.Pending
            };
            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();
            var leaveDTO = _mapper.Map<LeaveRequestDTO>(leaveRequest);
            return new ApiResponse<LeaveRequestDTO>
            {
                Data = leaveDTO,
                message = "Leave request created successfully",
                success = true
            };
        }

        public async Task<ApiResponse<bool>> DeleteLeaveAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<LeaveDetailsDTO>>> GetAllLeavesAsync()
        {
            List<LeaveDetailsDTO> leaves = (from user in _context.Users
                                            join leaveRequest in _context.LeaveRequests
                                            on user.Id equals leaveRequest.RequestedById
                                            select new LeaveDetailsDTO
                                            {
                                                Id = leaveRequest.Id,
                                                RequestedById = leaveRequest.RequestedById,
                                                Name = user.Name, 
                                                Email = user.Email,
                                                AppliedOn = leaveRequest.AppliedOn,
                                                StartDate = leaveRequest.StartDate,
                                                EndDate = leaveRequest.EndDate,
                                                Reason = leaveRequest.Reason,
                                                Status = leaveRequest.Status,
                                                LeaveType = leaveRequest.LeaveType,
                                                Description = leaveRequest.Description
                                            }).
                                            OrderByDescending(l => l.Status == LeaveStatus.Pending)
                                            .ThenByDescending(l => l.AppliedOn)
                                            .ToList();
            
            var leaveDTOs = _mapper.Map<List<LeaveDetailsDTO>>(leaves);
            return new ApiResponse<List<LeaveDetailsDTO>>
            {
                Data = leaveDTOs,
                message = "Leaves retrieved successfully",
                success = true
            };
        }

        public async Task<ApiResponse<LeaveRequestDTO?>> GetLeaveByIdAsync(Guid id)
        {
            LeaveRequest? leave = _context.LeaveRequests.Find(id);
            if (leave == null)
            {
                return new ApiResponse<LeaveRequestDTO?>
                {
                    Data = null,
                    message = "Leave request not found",
                    success = false
                };
            }
            var leaveDTO = _mapper.Map<LeaveRequestDTO>(leave);
            return new ApiResponse<LeaveRequestDTO?>
            {
                Data = leaveDTO,
                message = "Leave request retrieved successfully",
                success = true
            };
        }

        public async Task<ApiResponse<LeaveRequestDTO>> ApproveLeave(Guid leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests
                .FirstOrDefaultAsync(lr => lr.Id == leaveRequestId && lr.Status == LeaveStatus.Pending);

            if (leaveRequest == null)
            {
                return new ApiResponse<LeaveRequestDTO>
                {
                    Data = null,
                    message = "Leave request not found or already processed.",
                    success = false
                };
            }

            leaveRequest.Status = LeaveStatus.Approved;
            await _context.SaveChangesAsync();
            var leaveDTO = _mapper.Map<LeaveRequestDTO>(leaveRequest);
            return new ApiResponse<LeaveRequestDTO>
            {
                Data = leaveDTO,
                message = "Leave request approved successfully.",
                success = true
            };
        }
        public async Task<ApiResponse<LeaveRequestDTO>> RejectLeave(Guid leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests
                .FirstOrDefaultAsync(lr => lr.Id == leaveRequestId && lr.Status == LeaveStatus.Pending);

            if (leaveRequest == null)
            {
                return new ApiResponse<LeaveRequestDTO>
                {
                    Data = null,
                    message = "Leave request not found or already processed.",
                    success = false
                };
            }

            leaveRequest.Status = LeaveStatus.Rejected;
            await _context.SaveChangesAsync();
            var leaveDTO = _mapper.Map<LeaveRequestDTO>(leaveRequest);
            return new ApiResponse<LeaveRequestDTO>
            {
                Data = leaveDTO,
                message = "Leave request Rejected successfully.",
                success = true
            };
        }

        public Task<ApiResponse<IEnumerable<LeaveRequestDTO>>> GetLeavesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        //public Task<ApiResponse<List<LeaveRequestDTO>>> GetLeavesByStatusAsync(string status)
        //{
        //    LeaveRequest? leave = _context.LeaveRequests.FirstOrDefault(l => l.Status.ToString().Equals(status, StringComparison.OrdinalIgnoreCase));
        //}


        public async Task<ApiResponse<List<LeaveRequestDTO>>> GetLeavesByEmployeeIdAsync(string employeeId)
        {
            var user = await _userManager.FindByIdAsync(employeeId);
            if (user == null)
            {
                return new ApiResponse<List<LeaveRequestDTO>>
                {
                    Data = null,
                    message = "Employee not found",
                    success = false
                };
            }

            var leaves = await _context.LeaveRequests
                .Where(l => l.RequestedById == employeeId)
                .OrderByDescending(l => l.Status == LeaveStatus.Pending)
                .ThenByDescending(l => l.AppliedOn)
                .ToListAsync();

            if (leaves == null || !leaves.Any())
            {
                return new ApiResponse<List<LeaveRequestDTO>>
                {
                    Data = null,
                    message = "No leave requests found for this employee",
                    success = false
                };
            }

            var leaveDTOs = _mapper.Map<List<LeaveRequestDTO>>(leaves);

            return new ApiResponse<List<LeaveRequestDTO>>
            {
                Data = leaveDTOs,
                message = "Leave requests retrieved successfully",
                success = true
            };
        }

        

        public Task<ApiResponse<LeaveRequestDTO>> UpdateLeaveAsync(Guid id, LeaveRequestDTO leave)
        {
            throw new NotImplementedException();
        }
    }
}
