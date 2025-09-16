using AutoMapper;
using Domain.DTO;
using Domain.Models;
//using Domain.Models.JoinTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CreateMap<SourceType, DestinationType>();
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<RegisterDTO, User>().ReverseMap();
            CreateMap<DepartmentDTO, Department>().ReverseMap();
            //CreateMap<AttendanceDTO, Attendance>().ReverseMap();
            //CreateMap<LeaveRequestDTO, LeaveRequest>().ReverseMap();
            //CreateMap<PayrollDTO, Payroll>().ReverseMap();
            //CreateMap<DeptEmpDTO, DeptEmp>().ReverseMap();
            CreateMap<LoginDTO, User>().ReverseMap();


        }
    }
}
