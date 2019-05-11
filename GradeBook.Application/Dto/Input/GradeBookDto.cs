namespace GradeBook.Application.Dto.Input
{
    public class GradeBookDto
    {
        public int Id { get; set; }

        public int CourseId { get; }
        public int TeacherId { get; }
        public int StudentId { get; }
        public virtual float AverageWorkScore { get; set; }

        public virtual float PartialWorkScore { get; set; }

        public virtual float FinalWorkScore { get; set; }
    }
}
