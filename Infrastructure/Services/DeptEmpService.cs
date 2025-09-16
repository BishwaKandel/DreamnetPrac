//using Application.Interface;
//using AutoMapper;
//using Domain.DTO;
//using Domain.Models;
//using Domain.Models.JoinTables;
//using Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;

//namespace Infrastructure.Services
//{
//    public class DeptEmpService : IDeptEmpService
//    {
//        private readonly AppDbContext _context;
//        private readonly IMapper _mapper;
//        public DeptEmpService(AppDbContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }

//        public async Task<DeptEmpDTO> AddEmpToDept(DeptEmpDTO deptEmp)
//        {
//            DeptEmp deptemp = new DeptEmp
//            {
//                Id = deptEmp.Id,
//                DepartmentId = deptEmp.DepartmentId,
//                 = deptEmp.EmployeeId,
//                IsActive = deptEmp.IsActive
//            };
//            _context.DeptEmps.Add(deptemp);
//            await _context.SaveChangesAsync();
//            DeptEmpDTO deptempDTO = _mapper.Map<DeptEmpDTO>(deptemp);
//            return deptempDTO;
//        }

//        public async Task<IEnumerable<DeptEmpDTO>> GetDeptByEmployeeIdAsync(Guid EId)
//        {
//            List<DeptEmp> departments = await _context.DeptEmps
//                .Where(d => d.Id == EId)
//                .ToListAsync();
//            List<DeptEmpDTO> departmentDto = _mapper.Map<List<DeptEmpDTO>>(departments);
//            return departmentDto;


//        }
//    }
//}
