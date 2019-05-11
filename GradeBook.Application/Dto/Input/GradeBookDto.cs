namespace GradeBook.Application.Dto.Input
{
    public class GradeBookDto
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public int StudentId { get; set; }

        public int TeacherUserId { get; set; }

        public virtual float AverageWorkScore { get; set; }

        public virtual float PartialWorkScore { get; set; }

        public virtual float FinalWorkScore { get; set; }
    }
}
