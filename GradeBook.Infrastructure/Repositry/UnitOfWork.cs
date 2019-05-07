namespace GradeBook.Infrastructure.Repositry
{
    using Domain.Repository;

    public class UnitOfWork: IUnitOfWork
    {
        private readonly GradeBookContext _context;

        public UnitOfWork(GradeBookContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Roles = new RoleRepository(_context);
            Courses = new CourseRepository(_context);
            GradeBooks = new GradeBookRepository(_context);
            Students = new StudentRepository(_context);
            Teachers = new TeacherRepository(_context);

        }

        public IUserRepository Users { get; }
        public IRoleRepository Roles { get; }
        public ICourseRepository Courses { get; }
        public IGradeBookRepository GradeBooks { get; }
        public IStudentRepository Students { get; }
        public  ITeacherRepository Teachers { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
