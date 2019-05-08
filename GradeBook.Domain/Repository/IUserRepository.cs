namespace GradeBook.Domain.Repository
{
    using Entity;
    using System.Collections.Generic;

    public interface IUserRepository: IRepository<User>
    {
        User GetWithRoles(int id);

        IEnumerable<User> GetAllWithRoles(int pageNumber, int pageSize,
                                          string sortBy, string sortDirection);
    }
}
