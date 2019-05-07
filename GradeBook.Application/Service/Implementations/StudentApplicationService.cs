namespace GradeBook.Application.Service.Implementations
{
    using AutoMapper;
    using GradeBook.Application.Service.Interfaces;
    using GradeBook.Domain.Entity;
    using GradeBook.Domain.Repository;
    using System.Collections.Generic;
    using System.Linq;

    public class StudentApplicationService : IStudentApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(Dto.Input.StudentDto entity)
        {
            var newEntity = Mapper.Map<Student>(entity);

            _unitOfWork.Students.Add(newEntity);

            _unitOfWork.Complete();

            return newEntity.Id;
        }

        public Dto.Output.StudentDto Get(int id)
        {
            return Mapper.Map<Dto.Output.StudentDto>(_unitOfWork.Students.Get(id));
        }

        public List<Dto.Output.StudentDto> GetAll()
        {
            return Mapper.Map<List<Dto.Output.StudentDto>>(_unitOfWork.Students.GetAll());
        }

        public Dto.Output.PaginationDto GetAll(int page, int pageSize, string sortBy, string sortDirection)
        {
            var entities = _unitOfWork.Students.GetAll(page, pageSize, sortBy, sortDirection).ToList();

            var pagedRecord = new Dto.Output.PaginationDto
            {
                Content = Mapper.Map<List<Dto.Output.StudentDto>>(entities),
                TotalRecords = _unitOfWork.Students.CountGetAll(),
                CurrentPage = page,
                PageSize = pageSize
            };

            return pagedRecord;
        }

        public int Delete(int id)
        {
            var entity = _unitOfWork.Students.Get(id);

            _unitOfWork.Students.Delete(entity);
            _unitOfWork.Complete();

            return 1;
        }

        public int Update(int id, Dto.Input.StudentDto entity)
        {
            entity.Id = id;
            var oldEntity = _unitOfWork.Students.Get(id);

            Mapper.Map(entity, oldEntity);

            _unitOfWork.Students.Update(oldEntity);
            _unitOfWork.Complete();

            return oldEntity?.Id ?? 0;
        }
    }
}
