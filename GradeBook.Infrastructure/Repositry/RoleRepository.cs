namespace GradeBook.Infrastructure.Repositry
{
    using Domain.Entity;
    using Domain.Repository;

    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(GradeBookContext context): base(context)
        {

        }
    }
}
