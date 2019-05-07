namespace GradeBook.Domain.Entity
{
    public class GradeBook
    {
        public virtual int Id { get; set; }

        public virtual Course Course { get; set; }

        public virtual Student Student { get; set; }

        public virtual Teacher Teacher { get; set; }

        public virtual float AverageWorkScore { get; set; }

        public virtual float PartialWorkScore { get; set; }

        public virtual float FinalWorkScore { get; set; }

        public virtual float FinalScore { get; set; }

    }
}
