namespace GradeBook.Infrastructure.Repositry
{

    using Domain.Entity;
    using Domain.Repository;

    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(GradeBookContext context) : base(context)
        {

        }
    }
}
