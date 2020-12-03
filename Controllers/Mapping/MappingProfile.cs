using AutoMapper;
using ToDoList.Models;
using ToDoList.Controllers.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Controllers.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmployeeDTO, Employee>();
            CreateMap<Employee, EmployeeDTO>();

            CreateMap<WorkListDTO, WorkList>();
            CreateMap<WorkList, WorkListDTO>();

            CreateMap<Work, WorkDTO>();
            CreateMap<WorkDTO, Work>();

            CreateMap<Comment, CommentDTO>();
            CreateMap<CommentDTO, Comment>();

            CreateMap<WorkStatu, WorkStatusDTO>();
            CreateMap<WorkStatusDTO, WorkStatu>();
        }
    }
}
