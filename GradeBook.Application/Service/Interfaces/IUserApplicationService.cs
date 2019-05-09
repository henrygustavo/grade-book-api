namespace GradeBook.Application.Service.Interfaces
{
    using System.Threading.Tasks;

    public interface IUserApplicationService : IBaseApplicationService<Dto.Input.UserDto, Dto.Output.UserDto>
    {
        Task<string> PerformRegistration(Dto.Input.UserDto model);
    }
}
