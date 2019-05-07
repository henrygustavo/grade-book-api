namespace GradeBook.Application.Service.Interfaces
{
    using GradeBook.Application.Dto.Output;
    using System.Collections.Generic;

    public interface IBaseApplicationService<TEntityInput, TEntityOutPut>
    {
        TEntityOutPut Get(int id);

        PaginationDto GetAll(int page, int pageSize, string sortBy, string sortDirection);

        List<TEntityOutPut> GetAll();

        int Add(TEntityInput entity);

        int Update(int id, TEntityInput entity);

        int Delete(int id);

    }
}
