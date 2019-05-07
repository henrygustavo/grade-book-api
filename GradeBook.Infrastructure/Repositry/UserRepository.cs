namespace GradeBook.Infrastructure.Repositry
{
    using Domain.Entity;
    using Domain.Repository;

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(GradeBookContext context): base(context)
        {

        }
    }
}
