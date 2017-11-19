using AutoMapper;
using ServiceAPI.Dtos;
using ServiceAPI.Entities;

namespace ServiceAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        // Mapping configuration used by ServiceApi:
        //   it maps user entities to dtos and viceversa.
        public AutoMapperProfile()
        {
            CreateMap<Admin, AdminDto>();
            CreateMap<AdminDto, Admin>();

            CreateMap<Student, StudentDto>();
            CreateMap<StudentDto, Student>();

            CreateMap<Teacher, TeacherDto>();
            CreateMap<TeacherDto, Teacher>();
        }
    }
}
