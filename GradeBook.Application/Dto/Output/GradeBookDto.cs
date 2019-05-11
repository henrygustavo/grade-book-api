namespace GradeBook.Application.Dto.Output
{
    public class GradeBookDto
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int TeacherId { get; set; }

        public int StudentId { get; set; }

        public string CourseName { get; set; }
        public string StudentName { get; set; }

        public string TeacherName { get; set; }

        public  float AverageWorkScore { get; set; }

        public  float PartialWorkScore { get; set; }

        public  float FinalWorkScore { get; set; }

        public  float FinalScore { get; set; }
    }
}
