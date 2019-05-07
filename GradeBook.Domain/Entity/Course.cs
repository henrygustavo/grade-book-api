namespace GradeBook.Domain.Entity
{
    public class Course
    {
        public virtual int Id { get; set; }

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual int AverageWorkPercentage { get; set; }

        public virtual int PartialWorkPercentage { get; set; }

        public virtual int FinalWorkPercentage { get; set; }
    }
}
