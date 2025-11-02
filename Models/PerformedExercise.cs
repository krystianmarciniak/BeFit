namespace BeFit.Models
{
    public class PerformedExercise
    {
        public int Id { get; set; }

        public int TrainingSessionId { get; set; }
        public TrainingSession? TrainingSession { get; set; }

        public int ExerciseTypeId { get; set; }
        public ExerciseType? ExerciseType { get; set; }

        public int Sets { get; set; }
        public int Reps { get; set; }
        public double? Weight { get; set; }
    }
}
