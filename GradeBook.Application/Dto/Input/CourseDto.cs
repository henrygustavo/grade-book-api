namespace GradeBook.Application.Dto.Input
{
   public class CourseDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public int AverageWorkPercentage { get; set; }

        public int PartialWorkPercentage { get; set; }

        public int FinalWorkPercentage { get; set; }
    }
}
