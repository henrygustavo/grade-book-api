namespace GradeBook.Domain.Entity
{
    public abstract class BaseUser
    {
        public virtual int Id { get; set; }
        public virtual string FullName { get; set; }

        public virtual int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
