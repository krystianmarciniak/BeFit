using System.Collections.Generic;
using BeFit.Data;


namespace BeFit.Models;

public class ExerciseType
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // (opcjonalnie, jeśli trzymasz właściciela wpisu)
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    // ⬇⬇⬇ NAWIGACJA, której brakowało
    public ICollection<PerformedExercise> PerformedExercises { get; set; }
        = new List<PerformedExercise>();
}
