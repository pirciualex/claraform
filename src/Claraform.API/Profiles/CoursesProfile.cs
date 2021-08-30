using AutoMapper;

namespace Claraform.API.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Entitites.Course, Models.CourseDto>();
            CreateMap<Models.CourseCreateDto, Entitites.Course>();
        }
    }
}
