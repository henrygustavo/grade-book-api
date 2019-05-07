namespace GradeBook.Infrastructure.Repositry
{
    using Domain.Entity;
    using Domain.Repository;

    public class GradeBookRepository : BaseRepository<GradeBook>, IGradeBookRepository
    {
        public GradeBookRepository(GradeBookContext context) : base(context)
        {

        }
    }
}
