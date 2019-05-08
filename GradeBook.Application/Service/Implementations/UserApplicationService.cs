﻿namespace GradeBook.Application.Service.Implementations
{
    using AutoMapper;
    using GradeBook.Application.Service.Interfaces;
    using GradeBook.Domain.Entity;
    using GradeBook.Domain.Repository;
    using System.Collections.Generic;
    using System.Linq;

    public class UserApplicationService: IUserApplicationService

    {
        private readonly IUnitOfWork _unitOfWork;

        public UserApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(Dto.Input.UserDto entity)
        {
            var newEntity = Mapper.Map<User>(entity);

            _unitOfWork.Users.Add(newEntity);

            _unitOfWork.Complete();

            return newEntity.Id;
        }

        public Dto.Output.UserDto Get(int id)
        {
            var roles = _unitOfWork.Roles.GetAll();

            var entity = _unitOfWork.Users.Get(id);
            var entityDto = Mapper.Map<Dto.Output.UserDto>(entity);

            entityDto.RoleId = entity.UserRoles.FirstOrDefault().RoleId;
            entityDto.RoleName = roles.FirstOrDefault(p => p.Id == entityDto.RoleId).Name;

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
            var oldEntity = _unitOfWork.Users.Get(id);

            Mapper.Map(entity, oldEntity);

            _unitOfWork.Users.Update(oldEntity);
            _unitOfWork.Complete();

            return oldEntity?.Id ?? 0;
        }
    }
}
