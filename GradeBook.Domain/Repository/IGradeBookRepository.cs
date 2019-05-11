namespace GradeBook.Domain.Repository
{

    using Entity;
    using System.Collections.Generic;

    public interface IGradeBookRepository : IRepository<GradeBook>
    {
        GradeBook GetComplete(int userId);
        IEnumerable<GradeBook> GetAllByRole(int pageNumber, int pageSize, string sortBy, string sortDirection, string roleName, int userId);
        int CountGetAllByRole(string roleName, int userId);
    }

}
