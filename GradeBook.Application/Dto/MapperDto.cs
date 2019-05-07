namespace GradeBook.Application.Dto
{
    using AutoMapper;
    using GradeBook.Domain.Entity;

    public class MapperDto : Profile
    {
        public MapperDto()
        {
            CreateMap<User, Input.UserDto>().ReverseMap();
            CreateMap<User, Output.UserDto>().ReverseMap();

            CreateMap<Role, Input.RoleDto>().ReverseMap();
            CreateMap<Role, Output.RoleDto>().ReverseMap();

            CreateMap<Course, Input.CourseDto>().ReverseMap();
            CreateMap<Course, Output.CourseDto>().ReverseMap();

            CreateMap<GradeBook, Input.GradeBookDto>().ReverseMap();
            CreateMap<GradeBook, Output.GradeBookDto>().ReverseMap();

            CreateMap<Student, Input.StudentDto>().ReverseMap();
            CreateMap<Student, Output.StudentDto>().ReverseMap();

            CreateMap<Teacher, Input.TeacherDto>().ReverseMap();
            CreateMap<Teacher, Output.TeacherDto>().ReverseMap();

        }
    }
}
