namespace GradeBook.Infrastructure.Initializer
{
    using Domain.Entity;
    using Repositry;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using GradeBook.Domain.Repository;
    using System.Linq;

    public class DbInitializer
    {
        private readonly RoleManager<Role> _roleMgr;
        private readonly UserManager<User> _userInMgr;
        private readonly IUnitOfWork _unitOfWork;


        public DbInitializer(GradeBookContext context, UserManager<User> userMgr,
                             RoleManager<Role> roleMgr, IUnitOfWork unitOfWork)
        {
            _userInMgr = userMgr;
            _roleMgr = roleMgr;
            _unitOfWork = unitOfWork;

            context.Database.EnsureCreated();

        }

        public async Task Seed()
        {
            await SeedAdmin();
            await SeedTeacher();
            await SeedStudent();
            SeedCourses();

            _unitOfWork.Complete();
        }

        public async Task SeedAdmin()
        {
            var userName = "adminusr";
            var user = await _userInMgr.FindByNameAsync(userName);

            // Add User
            if (user == null)
            {
                string roleName = Roles.Admin;

                if (!await _roleMgr.RoleExistsAsync(roleName))
                {
                    var role = new Role { Name = roleName };
                    await _roleMgr.CreateAsync(role);
                }

                user = new User
                {
                    UserName = userName,
                    Email = "adminusr@test.com",
                    EmailConfirmed = true,
                    PhoneNumber = "530-685-2496"
                };

                var userResult = await _userInMgr.CreateAsync(user, "P@$$w0rd");
                var roleResult = await _userInMgr.AddToRoleAsync(user, roleName);
                var claimResult = await _userInMgr.AddClaimAsync(user, new Claim(ClaimTypes.Role, roleName));

                if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }

            }
        }

        public async Task SeedTeacher()
        {
            var userName = "mcteacher";
            var user = await _userInMgr.FindByNameAsync(userName);

            // Add User
            if (user == null)
            {
                string roleName = Roles.Teacher;

                if (!await _roleMgr.RoleExistsAsync(roleName))
                {
                    var role = new Role { Name = roleName };

                    await _roleMgr.CreateAsync(role);
                }

                user = new User
                {
                    UserName = userName,
                    Email = "mcteacher@test.com",
                    EmailConfirmed = true,
                    PhoneNumber = "530-685-2496"
                };

                var teacher = new Teacher
                {
                    User = user,
                    Name = "Manuel Caldas"
                };

                var userResult = await _userInMgr.CreateAsync(user, "P@$$w0rd");
                var roleResult = await _userInMgr.AddToRoleAsync(user, roleName);
                var claimResult = await _userInMgr.AddClaimAsync(user, new Claim(ClaimTypes.Role, roleName));

                _unitOfWork.Teachers.Add(teacher);

                if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }

            }
        }

        public async Task SeedStudent()
        {
            var userName = "hfuentes";
            var user = await _userInMgr.FindByNameAsync(userName);

            // Add User
            if (user == null)
            {
                string roleName = Roles.Student;

                if (!await _roleMgr.RoleExistsAsync(roleName))
                {
                    var role = new Role { Name = roleName };

                    await _roleMgr.CreateAsync(role);
                }

                user = new User
                {
                    UserName = userName,
                    Email = "hfuentes@test.com",
                    EmailConfirmed = true,
                    PhoneNumber = "530-685-2496"
                };

                var student = new Student
                {
                    User = user,
                    Name = "Henry Fuentes"
                };

                var userResult = await _userInMgr.CreateAsync(user, "P@$$w0rd");
                var roleResult = await _userInMgr.AddToRoleAsync(user, roleName);
                var claimResult = await _userInMgr.AddClaimAsync(user, new Claim(ClaimTypes.Role, roleName));

                _unitOfWork.Students.Add(student);

                if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }

            }
        }

        public void SeedCourses()
        {

           if(!_unitOfWork.Courses.GetAll().Any(p=>p.Code == "CC100" || p.Code == "CC101"))
            {
                _unitOfWork.Courses.Add(new Course
                {
                    Name = "Fundamentos de Cloud Computing",
                    Code = "CC100",
                    AverageWorkPercentage = 30,
                    PartialWorkPercentage = 35,
                    FinalWorkPercentage = 35
                });

                _unitOfWork.Courses.Add(new Course
                {
                    Name = "Arquitectura de Cloud Computing",
                    Code = "CC101",
                    AverageWorkPercentage = 40,
                    PartialWorkPercentage = 30,
                    FinalWorkPercentage = 30
                });
            }
        }
    }
}
