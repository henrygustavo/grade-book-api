namespace GradeBook.Infrastructure.Repositry
{
    using Domain.Entity;
    using Domain.Repository;

    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(GradeBookContext context) : base(context)
        {

        }
    }
}
