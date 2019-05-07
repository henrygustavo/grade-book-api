namespace GradeBook.Application.Service.Implementations
{
    using AutoMapper;
    using GradeBook.Application.Service.Interfaces;
    using GradeBook.Domain.Entity;
    using GradeBook.Domain.Repository;
    using System.Collections.Generic;
    using System.Linq;

    public class GradeBookApplicationService : IGradeBookApplicationService

    {
        private readonly IUnitOfWork _unitOfWork;

        public GradeBookApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(Dto.Input.GradeBookDto entity)
        {
            var newEntity = Mapper.Map<GradeBook>(entity);

            _unitOfWork.GradeBooks.Add(newEntity);

            _unitOfWork.Complete();

            return newEntity.Id;
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

            Mapper.Map(entity, oldEntity);

            _unitOfWork.GradeBooks.Update(oldEntity);
            _unitOfWork.Complete();

            return oldEntity?.Id ?? 0;
        }
    }
}
