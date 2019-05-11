namespace GradeBook.Application.Service.Implementations
{
    using AutoMapper;
    using GradeBook.Application.Service.Interfaces;
    using GradeBook.Domain.Entity;
    using GradeBook.Domain.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Notification;

    public class GradeBookApplicationService : IGradeBookApplicationService

    {
        private readonly IUnitOfWork _unitOfWork;

        public GradeBookApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(Dto.Input.GradeBookDto entity)
        {
            Notification notification = Validate(entity);

            if (notification.HasErrors())
            {
                throw new ArgumentException(notification.ErrorMessage());
            }

            var newEntity = Mapper.Map<GradeBook>(entity);

            newEntity.FinalScore = CalculateFinalScore(entity);

            _unitOfWork.GradeBooks.Add(newEntity);

            _unitOfWork.Complete();

            return newEntity.Id;
        }

        private Notification Validate(Dto.Input.GradeBookDto entity)
        {
            Notification notification = new Notification();

            if (entity == null)
            {
                notification.AddError("Invalid JSON data in request body");
                return notification;
            }

            if (_unitOfWork.Courses.Get(entity.CourseId) == null)
            {
                notification.AddError($"Course does not exist");
                return notification;
            }

            if (_unitOfWork.Users.Get(entity.TeacherId) == null)
            {
                notification.AddError($"Teacher does not exist");
                return notification;
            }

            if (_unitOfWork.Users.Get(entity.StudentId) == null)
            {
                notification.AddError($"Course does not exist");
                return notification;
            }

            if (_unitOfWork.GradeBooks.GetAll().Any(p=>p.Course.Id == entity.CourseId && p.Student.Id  == entity.StudentId))
            {
                notification.AddError($"The Student already has a Score on this course");
                return notification;
            }

            if (entity.AverageWorkScore >= 0 && entity.AverageWorkScore<= 20)
            {
                notification.AddError("Average Work Score must be between 0-20");
                return notification;
            }

            if (entity.PartialWorkScore >= 0 && entity.PartialWorkScore <= 20)
            {
                notification.AddError("Partial Work Score must be between 0-20");
                return notification;
            }

            if (entity.FinalWorkScore >= 0 && entity.FinalWorkScore <= 20)
            {
                notification.AddError("Final Work Score must be must be between 0-20");
                return notification;
            }

            return notification;
        }

        public Dto.Output.GradeBookDto Get(int id)
        {
            return Mapper.Map<Dto.Output.GradeBookDto>(_unitOfWork.GradeBooks.Get(id));
        }

        public Dto.Output.PaginationDto GetAll(int page, int pageSize, string sortBy, string sortDirection)
        {
            var entities = _unitOfWork.GradeBooks.GetAll(page, pageSize, sortBy, sortDirection).ToList();

            var pagedRecord = new Dto.Output.PaginationDto
            {
                Content = Mapper.Map<List<Dto.Output.GradeBookDto>>(entities),
                TotalRecords = _unitOfWork.GradeBooks.CountGetAll(),
                CurrentPage = page,
                PageSize = pageSize
            };

            return pagedRecord;
        }

        public Dto.Output.PaginationDto GetAllByRole(int page, int pageSize, string sortBy, string sortDirection, int userId)
        {
            int roleId = _unitOfWork.Users.GetWithRoles(userId).UserRoles.FirstOrDefault().RoleId;

            string roleName = _unitOfWork.Roles.Get(roleId).Name;

            var entities = _unitOfWork.GradeBooks.GetAllByRole(page, pageSize, sortBy, sortDirection, roleName, userId).ToList();

            var pagedRecord = new Dto.Output.PaginationDto
            {
                Content = Mapper.Map<List<Dto.Output.GradeBookDto>>(entities),
                TotalRecords = _unitOfWork.GradeBooks.CountGetAllByRole(roleName, userId),
                CurrentPage = page,
                PageSize = pageSize
            };

            return pagedRecord;

        }

        public List<Dto.Output.GradeBookDto> GetAll()
        {
            return Mapper.Map<List<Dto.Output.GradeBookDto>>(_unitOfWork.GradeBooks.GetAll());
        }

        public int Delete(int id)
        {
            var entity = _unitOfWork.GradeBooks.Get(id);

            _unitOfWork.GradeBooks.Delete(entity);
            _unitOfWork.Complete();

            return 1;
        }

        public int Update(int id, Dto.Input.GradeBookDto entity)
        {
            entity.Id = id;
            var oldEntity = _unitOfWork.GradeBooks.Get(id);

            Notification notification = Validate(entity);

            if (notification.HasErrors())
            {
                throw new ArgumentException(notification.ErrorMessage());
            }

            oldEntity.FinalScore = CalculateFinalScore(entity);

            Mapper.Map(entity, oldEntity);

            _unitOfWork.GradeBooks.Update(oldEntity);
            _unitOfWork.Complete();

            return oldEntity?.Id ?? 0;
        }

        public float CalculateFinalScore(Dto.Input.GradeBookDto entity)
        {
           Course course =  _unitOfWork.Courses.Get(entity.CourseId);

            return (entity.FinalWorkScore * course.FinalWorkPercentage +
                    entity.PartialWorkScore * course.PartialWorkPercentage +
                    entity.AverageWorkScore * course.AverageWorkPercentage) / 100;

        }
    }
}
