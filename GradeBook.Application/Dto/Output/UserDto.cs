﻿namespace GradeBook.Application.Dto.Output
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
      
        public bool Disabled { get; set; }
    }
}
