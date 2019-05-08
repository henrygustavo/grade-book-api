namespace GradeBook.Application.Dto.Output
{
    public class UserDto
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public int RoleId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
      
        public bool Disabled { get; set; }
    }
}
