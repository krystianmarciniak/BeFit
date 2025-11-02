namespace BeFit.Models
{
    public class TrainingSession
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? UserId { get; set; }   // IdentityUser.Id

        public ICollection<PerformedExercise> PerformedExercises { get; set; } = new List<PerformedExercise>();
    }
}
