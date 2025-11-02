namespace BeFit.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ICollection<PerformedExercise> PerformedExercises { get; set; } = new List<PerformedExercise>();
    }
}
