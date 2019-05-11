namespace GradeBook.Infrastructure.Repositry
{
    using Domain.Entity;
    using Domain.Repository;

    public class AdministratorRepository : BaseRepository<Administrator>, IAdministratorRepository
    {
        public AdministratorRepository(GradeBookContext context) : base(context)
        {

        }
    }
}
