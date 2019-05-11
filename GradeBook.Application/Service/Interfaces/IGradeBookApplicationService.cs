namespace GradeBook.Application.Service.Interfaces
{
    public interface IGradeBookApplicationService : IBaseApplicationService<Dto.Input.GradeBookDto, Dto.Output.GradeBookDto>
    {
        Dto.Output.PaginationDto GetAllByRole(int page, int pageSize, string sortBy, string sortDirection, int userId);
    }
}
