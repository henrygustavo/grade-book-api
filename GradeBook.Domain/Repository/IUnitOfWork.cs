namespace GradeBook.Domain.Repository
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        ICourseRepository Courses { get; }
        IGradeBookRepository GradeBooks { get; }
        IStudentRepository Students { get; }
        ITeacherRepository Teachers { get; }

        int Complete();
    }
}
