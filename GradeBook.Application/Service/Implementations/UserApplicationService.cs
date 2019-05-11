namespace GradeBook.Application.Service.Implementations
{
    using AutoMapper;
    using GradeBook.Application.Service.Interfaces;
    using GradeBook.Domain.Entity;
    using GradeBook.Domain.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Notification;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;

    public class UserApplicationService: IUserApplicationService

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userInMgr;
        private readonly IConfiguration _config;

        public UserApplicationService(IUnitOfWork unitOfWork, UserManager<User> userInMgr, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
   
            _userInMgr = userInMgr;
            _config = config;
        }

        public int Add(Dto.Input.UserDto entity)
        {
            var newEntity = Mapper.Map<User>(entity);

            _unitOfWork.Users.Add(newEntity);

            _unitOfWork.Complete();

            return newEntity.Id;
        }

        public async Task<string> PerformRegistration(Dto.Input.UserDto model)
        {
            Notification notification = ValidateRegistration(model);

            if (notification.HasErrors())
            {
                throw new ArgumentException(notification.ErrorMessage());
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email
            };

            Role role = await CreateNewUser(model, notification, user);

            InsertUserByRole(model.FullName, user, role.Name);

            _unitOfWork.Complete();

            return "user was created successfully";
        }

        private async Task<Role> CreateNewUser(Dto.Input.UserDto model, Notification notification, User user)
        {
            var userResult = await _userInMgr.CreateAsync(user, model.Password);

            if (!userResult.Succeeded)
            {
                notification.AddError(string.Join(". ", from item in userResult.Errors select item.Description));

                throw new ArgumentException(notification.ErrorMessage());
            }

            var role = _unitOfWork.Roles.Get(model.RoleId);

            var roleResult = await _userInMgr.AddToRoleAsync(user, role.Name);

            if (!roleResult.Succeeded)
            {
                notification.AddError(string.Join(". ", from item in roleResult.Errors select item.Description));

                throw new ArgumentException(notification.ErrorMessage());
            }

            var claimResult = await _userInMgr.AddClaimAsync(user, new Claim(ClaimTypes.Role, role.Name));

            if (!claimResult.Succeeded)
            {
                notification.AddError(string.Join(". ", from item in claimResult.Errors select item.Description));

                throw new ArgumentException(notification.ErrorMessage());
            }

            return role;
        }

        private void InsertUserByRole(string fullName, User user, string roleName)
        {
            Dictionary<string, Action> dictionryInsert = new Dictionary<string, Action>
            {
                {
                    Roles.Admin,
                    () => _unitOfWork.Administrators.Add(new Administrator
                    {
                        User = user,
                        FullName = fullName
                    })
                },

                {
                    Roles.Student,
                    () => _unitOfWork.Students.Add(new Student
                    {
                        User = user,
                        FullName = fullName
                    })
                },

                {
                    Roles.Teacher,
                    () => _unitOfWork.Teachers.Add(new Teacher
                    {
                        User = user,
                        FullName = fullName
                    })
                }
            };

            dictionryInsert[roleName]();
        }

        private string GetUserFullNameByRole(int userId, string roleName)
        {
            var dictionryGet = new Dictionary<string, Func<string>>
            {
                {
                    Roles.Admin,
                   () =>  _unitOfWork.Administrators.GetAll().FirstOrDefault(p=>p.UserId == userId).FullName 
                },

                {
                    Roles.Student,
                  () => _unitOfWork.Students.GetAll().FirstOrDefault(p=>p.UserId == userId).FullName
                },

                {
                    Roles.Teacher,
                   () => _unitOfWork.Teachers.GetAll().FirstOrDefault(p=>p.UserId == userId).FullName
                }
            };

          return dictionryGet[roleName]();
        }

        private Notification ValidateRegistration(Dto.Input.UserDto model)
        {
            Notification notification = new Notification();

            if (model == null || string.IsNullOrEmpty(model.Email)
                                  || string.IsNullOrEmpty(model.UserName)
                                  || string.IsNullOrEmpty(model.Password)
                                  || string.IsNullOrEmpty(model.FullName)
                                  )

            {
                notification.AddError("Invalid JSON data in request body");
                return notification;
            }

            if (model.UserName.Length < int.Parse(_config["Account:UserNameRequiredMinLength"]) ||
                model.UserName.Length > int.Parse(_config["Account:UserNameRequiredMaxLength"])
                )
            {
                notification.AddError($"UserName must have between {_config["Account:UserNameRequiredMinLength"]} and {_config["Account:UserNameRequiredMaxLength"]} characters.");
                return notification;
            }

            return notification;

        }

        private Notification ValidateUpdate(Dto.Input.UserDto model)
        {
            Notification notification = new Notification();

            if (model == null || string.IsNullOrEmpty(model.Email))

            {
                notification.AddError("Invalid JSON data in request body");
                return notification;
            }


            if (_unitOfWork.Users.GetAll().Any(p => p.Id != model.Id && p.Email == model.Email))
            {
                notification.AddError($"the email {model.Email} is already taken");
                return notification;
            }

            if (_unitOfWork.Users.GetAll().Any(p => p.Id != model.Id && p.UserName == model.UserName))
            {
                notification.AddError($"the user name {model.UserName} is already taken");
                return notification;
            }

            if (model.UserName.Length < int.Parse(_config["Account:UserNameRequiredMinLength"]) ||
               model.UserName.Length > int.Parse(_config["Account:UserNameRequiredMaxLength"])
               )
            {
                notification.AddError($"UserName must have between {_config["Account:UserNameRequiredMinLength"]} and {_config["Account:UserNameRequiredMaxLength"]} characters.");
                return notification;
            }

            return notification;

        }

        public Dto.Output.UserDto Get(int id)
        {
            var roles = _unitOfWork.Roles.GetAll();

            var entity = _unitOfWork.Users.GetWithRoles(id);
            var entityDto = Mapper.Map<Dto.Output.UserDto>(entity);

            entityDto.RoleId = entity.UserRoles.FirstOrDefault().RoleId;
            entityDto.RoleName = roles.FirstOrDefault(p => p.Id == entityDto.RoleId).Name;
            entityDto.FullName = GetUserFullNameByRole(id, entityDto.RoleName);

            return entityDto;
        }

        public List<Dto.Output.UserDto> GetAll()
        {
            return Mapper.Map<List<Dto.Output.UserDto>>(_unitOfWork.Users.GetAll());
        }

        public Dto.Output.PaginationDto GetAll(int page, int pageSize, string sortBy, string sortDirection)
        {
            var entities = _unitOfWork.Users.GetAllWithRoles(page, pageSize, sortBy, sortDirection).ToList();

            var entitiesDto = Mapper.Map<List<Dto.Output.UserDto>>(entities);

            var roles = _unitOfWork.Roles.GetAll();

            foreach(var entityDto in entitiesDto)
            {
                entityDto.RoleId = entities.FirstOrDefault(p=>p.Id == entityDto.Id).UserRoles.FirstOrDefault().RoleId;
                entityDto.RoleName = roles.FirstOrDefault(p => p.Id == entityDto.RoleId).Name;
                entityDto.FullName = GetUserFullNameByRole(entityDto.Id, entityDto.RoleName);
            }

            var pagedRecord = new Dto.Output.PaginationDto
            {
                Content = entitiesDto,
                TotalRecords = _unitOfWork.Users.CountGetAll(),
                CurrentPage = page,
                PageSize = pageSize
            };

            return pagedRecord;
        }

        public int Delete(int id)
        {
            var entity = _unitOfWork.Users.Get(id);

            _unitOfWork.Users.Delete(entity);
            _unitOfWork.Complete();

            return 1;
        }

        public int Update(int id, Dto.Input.UserDto entity)
        {
            entity.Id = id;

            Notification notification = ValidateUpdate(entity);

            if (notification.HasErrors())
            {
                throw new ArgumentException(notification.ErrorMessage());
            }

            var oldEntity = _unitOfWork.Users.GetWithRoles(id);
            oldEntity.UserName = entity.UserName;
            oldEntity.NormalizedUserName = entity.UserName.ToUpper();
            oldEntity.Email = entity.Email;
            oldEntity.NormalizedEmail = entity.Email.ToUpper();
            oldEntity.Disabled = entity.Disabled;

            _unitOfWork.Users.Update(oldEntity);

           int  roleId = oldEntity.UserRoles.FirstOrDefault().RoleId;

            var roles = _unitOfWork.Roles.GetAll();

           var roleName = roles.FirstOrDefault(p => p.Id == roleId).Name;

            UpdateUserByRole(entity.FullName, roleName, id);

            _unitOfWork.Complete();

            return oldEntity?.Id ?? 0;
        }

        private void UpdateUserByRole(string fullName, string roleName, int userId)
        {
            Dictionary<string, Action> dictionryUpdate = new Dictionary<string, Action>
            {
                {
                    Roles.Admin,
                    () => {

                             var administrator =   _unitOfWork.Administrators.GetAll().SingleOrDefault(p=>p.UserId == userId);
                             administrator.FullName = fullName;
                             _unitOfWork.Administrators.Update(administrator);
                          }
                 },
                {
                    Roles.Student,
                    () => {

                             var student =   _unitOfWork.Students.GetAll().SingleOrDefault(p=>p.UserId == userId);
                             student.FullName = fullName;
                             _unitOfWork.Students.Update(student);
                          }
                },

                {
                    Roles.Teacher,
                    () => {

                             var teacher =   _unitOfWork.Teachers.GetAll().SingleOrDefault(p=>p.UserId == userId);
                             teacher.FullName = fullName;
                             _unitOfWork.Teachers.Update(teacher);
                          }
                }
            };

            dictionryUpdate[roleName]();
        }
    }
}
