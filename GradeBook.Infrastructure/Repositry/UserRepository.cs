namespace GradeBook.Infrastructure.Repositry
{
    using System.Collections.Generic;
    using System.Linq;
    using GradeBook.Domain.Entity;
    using GradeBook.Domain.Repository;
    using Microsoft.EntityFrameworkCore;
    using Utils;
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(GradeBookContext context): base(context)
        {

        }

        public User GetWithRoles(int id)
        {
            return Context.Set<User>().Include(p => p.UserRoles).FirstOrDefault(p=>p.Id == id);
        }

        public IEnumerable<User> GetAllWithRoles(int pageNumber, int pageSize,
                                          string sortBy, string sortDirection)
        {

            var skip = (pageNumber - 1) * pageSize;
            return Context.Set<User>().Include(p => p.UserRoles)
                .OrderBy(sortBy, sortDirection)
                .Skip(skip)
                .Take(pageSize);
        }
    }
}
