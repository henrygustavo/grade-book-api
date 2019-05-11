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

            CreateMap<GradeBook, Output.GradeBookDto>()
                 .ForMember(
                    dest => dest.StudentId,
                    opts => opts.MapFrom(
                        src => src.Student.Id
                    )
                )
                .ForMember(
                    dest => dest.StudentName,
                    opts => opts.MapFrom(
                        src => src.Student.FullName
                    )
                )
                .ForMember(
                    dest => dest.TeacherId,
                    opts => opts.MapFrom(
                        src => src.Teacher.Id
                    )
                )
                .ForMember(
                    dest => dest.TeacherName,
                    opts => opts.MapFrom(
                        src => src.Teacher.FullName
                    )
                )
                 .ForMember(
                    dest => dest.CourseId,
                    opts => opts.MapFrom(
                        src => src.Course.Id
                    )
                )
                .ForMember(
                    dest => dest.CourseName,
                    opts => opts.MapFrom(
                        src => src.Course.Name
                    )
                )
                .ReverseMap();

            CreateMap<Student, Input.StudentDto>().ReverseMap();
            CreateMap<Student, Output.StudentDto>().ReverseMap();

            CreateMap<Teacher, Input.TeacherDto>().ReverseMap();
            CreateMap<Teacher, Output.TeacherDto>().ReverseMap();

        }
    }
}
