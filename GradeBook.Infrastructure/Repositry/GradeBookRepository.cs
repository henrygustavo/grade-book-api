namespace GradeBook.Infrastructure.Repositry
{
    using GradeBook.Domain.Entity;
    using GradeBook.Domain.Repository;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utils;
    public class GradeBookRepository : BaseRepository<GradeBook>, IGradeBookRepository
    {
        public GradeBookRepository(GradeBookContext context) : base(context)
        {

        }

        public IEnumerable<GradeBook> GetAllByRole(int pageNumber, int pageSize, string sortBy, string sortDirection, string roleName, int userId)
        {
            var gradeBookDcitionary = new Dictionary<string, Func<IQueryable<GradeBook>>>();

            var skip = (pageNumber - 1) * pageSize;

            var gradebook = Context.Set<GradeBook>().Include(p => p.Teacher).Include(p => p.Student);

            gradeBookDcitionary.Add(Roles.Admin, () => gradebook.OrderBy(sortBy, sortDirection).Skip(skip).Take(pageSize));

            gradeBookDcitionary.Add(Roles.Teacher, () => gradebook.Where(p => p.Teacher.Id == userId).OrderBy(sortBy, sortDirection).Skip(skip).Take(pageSize));

            gradeBookDcitionary.Add(Roles.Student, () => gradebook.Where(p => p.Student.Id == userId).OrderBy(sortBy, sortDirection).Skip(skip).Take(pageSize));

            return gradeBookDcitionary[roleName]();

        }

        public int CountGetAllByRole(string roleName, int userId)
        {
            Dictionary<string, int> gradeBookDcitionary = new Dictionary<string, int>();

            var gradebook = Context.Set<GradeBook>().Include(p => p.Teacher).Include(p => p.Student);

            gradeBookDcitionary.Add(Roles.Admin, gradebook.Count());
            gradeBookDcitionary.Add(Roles.Teacher, gradebook.Where(p => p.Teacher.Id == userId).Count());
            gradeBookDcitionary.Add(Roles.Student, gradebook.Where(p => p.Student.Id == userId).Count());
            return gradeBookDcitionary[roleName];
        }
    }
}
