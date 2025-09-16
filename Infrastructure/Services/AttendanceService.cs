using Application.Interface;
using AutoMapper;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Data;
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
        public AttendanceService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AttendanceDTO> CreateAttendanceAsync(AttendanceDTO attendance)
        {
            throw new NotImplementedException();

        }

        public Task<bool> DeleteAttendanceAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AttendanceDTO>> GetAllAttendancesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AttendanceDTO?> GetAttendanceByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AttendanceDTO>> GetAttendancesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AttendanceDTO>> GetAttendancesByEmployeeIdAsync(Guid employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<AttendanceDTO> UpdateAttendanceAsync(Guid id, AttendanceDTO attendance)
        {
            throw new NotImplementedException();
        }
    }
}
