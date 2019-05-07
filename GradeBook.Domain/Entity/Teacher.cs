﻿namespace GradeBook.Domain.Entity
{
  public  class Teacher
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
