namespace GradeBook.Application.Service.Implementations
{
    using AutoMapper;
    using GradeBook.Application.Service.Interfaces;
    using GradeBook.Domain.Entity;
    using GradeBook.Domain.Repository;
    using System.Collections.Generic;
    using System.Linq;
    using Notification;
    using System;

    public class CourseApplicationService : ICourseApplicationService

    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(Dto.Input.CourseDto entity)
        {
            Notification notification = Validate(entity);

            if (notification.HasErrors())
            {
                throw new ArgumentException(notification.ErrorMessage());
            }

            var newEntity = Mapper.Map<Course>(entity);

            _unitOfWork.Courses.Add(newEntity);

            _unitOfWork.Complete();

            return newEntity.Id;
        }

        private Notification Validate(Dto.Input.CourseDto entity)
        {
            Notification notification = new Notification();

            if (entity == null)
            {
                notification.AddError("Invalid JSON data in request body");
                return notification;
            }

            if(_unitOfWork.Courses.GetAll().Any(p=>p.Id!=entity.Id 
            && p.Code == entity.Code))
            {
                notification.AddError($"Code {entity.Code} already exists");
                return notification;
            }

            if (_unitOfWork.Courses.GetAll().Any(p => p.Id != entity.Id
             && p.Name == entity.Name))
            {
                notification.AddError($"Name {entity.Name} already exists");
                return notification;
            }

            if (string.IsNullOrEmpty(entity.Name))
            {
                notification.AddError("Name should have a value");
                return notification;
            }

            if (string.IsNullOrEmpty(entity.Code))
            {
                notification.AddError("Code should have a value");
                return notification;
            }

            if (entity.AverageWorkPercentage <1)
            {
                notification.AddError("Average Work % must be greater than 0");
                return notification;
            }

            if (entity.PartialWorkPercentage < 1)
            {
                notification.AddError("Partial Work % must be greater than 0");
                return notification;
            }

            if (entity.FinalWorkPercentage < 1)
            {
                notification.AddError("Final Work % must be greater than 0");
                return notification;
            }

            int totalPercentage = entity.AverageWorkPercentage +
                entity.PartialWorkPercentage +
                entity.FinalWorkPercentage;

            if (totalPercentage != 100)
            {
                notification.AddError($"The Sum of % should be 100% already exists");
                return notification;

            }

            return notification;
        }

        public List<Dto.Output.CourseDto> GetAll()
        {
            return Mapper.Map<List<Dto.Output.CourseDto>>(_unitOfWork.Courses.GetAll());
        }

        public Dto.Output.CourseDto Get(int id)
        {
            return Mapper.Map<Dto.Output.CourseDto>(_unitOfWork.Courses.Get(id));
        }

        public Dto.Output.PaginationDto GetAll(int page, int pageSize, string sortBy, string sortDirection)
        {
            var entities = _unitOfWork.Courses.GetAll(page, pageSize, sortBy, sortDirection).ToList();

            var pagedRecord = new Dto.Output.PaginationDto
            {
                Content = Mapper.Map<List<Dto.Output.CourseDto>>(entities),
                TotalRecords = _unitOfWork.Courses.CountGetAll(),
                CurrentPage = page,
                PageSize = pageSize
            };

            return pagedRecord;
        }

        public int Delete(int id)
        {
            var entity = _unitOfWork.Courses.Get(id);

            _unitOfWork.Courses.Delete(entity);
            _unitOfWork.Complete();

            return 1;
        }

        public int Update(int id, Dto.Input.CourseDto entity)
        {
            entity.Id = id;
            var oldEntity = _unitOfWork.Courses.Get(id);

            Notification notification = Validate(entity);

            if (notification.HasErrors())
            {
                throw new ArgumentException(notification.ErrorMessage());
            }

            Mapper.Map(entity, oldEntity);

            _unitOfWork.Courses.Update(oldEntity);
            _unitOfWork.Complete();

            return oldEntity?.Id ?? 0;
        }
    }
}
