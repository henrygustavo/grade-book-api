namespace GradeBook.Application.Service.Implementations
{
    using AutoMapper;
    using GradeBook.Application.Service.Interfaces;
    using GradeBook.Domain.Entity;
    using GradeBook.Domain.Repository;
    using System.Collections.Generic;
    using System.Linq;

    public class TeacherApplicationService : ITeacherApplicationService

    {
        private readonly IUnitOfWork _unitOfWork;

        public TeacherApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(Dto.Input.TeacherDto entity)
        {
            var newEntity = Mapper.Map<Teacher>(entity);

            _unitOfWork.Teachers.Add(newEntity);

            _unitOfWork.Complete();

            return newEntity.Id;
        }

        public Dto.Output.TeacherDto Get(int id)
        {
            return Mapper.Map<Dto.Output.TeacherDto>(_unitOfWork.Teachers.Get(id));
        }

        public List<Dto.Output.TeacherDto> GetAll()
        {
            return Mapper.Map<List<Dto.Output.TeacherDto>>(_unitOfWork.Teachers.GetAll());
        }

        public Dto.Output.PaginationDto GetAll(int page, int pageSize, string sortBy, string sortDirection)
        {
            var entities = _unitOfWork.Teachers.GetAll(page, pageSize, sortBy, sortDirection).ToList();

            var pagedRecord = new Dto.Output.PaginationDto
            {
                Content = Mapper.Map<List<Dto.Output.TeacherDto>>(entities),
                TotalRecords = _unitOfWork.Teachers.CountGetAll(),
                CurrentPage = page,
                PageSize = pageSize
            };

            return pagedRecord;
        }

        public int Delete(int id)
        {
            var entity = _unitOfWork.Teachers.Get(id);

            _unitOfWork.Teachers.Delete(entity);
            _unitOfWork.Complete();

            return 1;
        }

        public int Update(int id, Dto.Input.TeacherDto entity)
        {
            entity.Id = id;
            var oldEntity = _unitOfWork.Teachers.Get(id);

            Mapper.Map(entity, oldEntity);

            _unitOfWork.Teachers.Update(oldEntity);
            _unitOfWork.Complete();

            return oldEntity?.Id ?? 0;
        }
    }
}
