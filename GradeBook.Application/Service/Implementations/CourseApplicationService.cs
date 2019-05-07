namespace GradeBook.Application.Service.Implementations
{
    using AutoMapper;
    using GradeBook.Application.Service.Interfaces;
    using GradeBook.Domain.Entity;
    using GradeBook.Domain.Repository;
    using System.Collections.Generic;
    using System.Linq;

    public class CourseApplicationService : ICourseApplicationService

    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(Dto.Input.CourseDto entity)
        {
            var newEntity = Mapper.Map<Course>(entity);

            _unitOfWork.Courses.Add(newEntity);

            _unitOfWork.Complete();

            return newEntity.Id;
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

            Mapper.Map(entity, oldEntity);

            _unitOfWork.Courses.Update(oldEntity);
            _unitOfWork.Complete();

            return oldEntity?.Id ?? 0;
        }
    }
}
