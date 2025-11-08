using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeFit.Models
{
    public class PerformedExercise
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Sesja jest wymagana.")]
        [Display(Name = "Sesja")]
        public int TrainingSessionId { get; set; }

        [Required(ErrorMessage = "Rodzaj ćwiczenia jest wymagany.")]
        [Display(Name = "Ćwiczenie")]
        public int ExerciseTypeId { get; set; }

        [Range(1, 50, ErrorMessage = "Serie: 1–50.")]
        [Display(Name = "Serie")]
        public int Sets { get; set; }

        [Range(1, 200, ErrorMessage = "Powtórzenia: 1–200.")]
        [Display(Name = "Powtórzenia")]
        public int Reps { get; set; }

        [Range(0, 2000, ErrorMessage = "Ciężar w kg: 0–2000.")]
        [Display(Name = "Waga (kg)")]
        public double? WeightKg { get; set; }

        [ForeignKey(nameof(TrainingSessionId))]
        public TrainingSession? TrainingSession { get; set; }

        [ForeignKey(nameof(ExerciseTypeId))]
        public ExerciseType? ExerciseType { get; set; }
    }
}
