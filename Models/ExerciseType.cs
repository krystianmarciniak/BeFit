using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BeFit.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace BeFit.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa ćwiczenia jest wymagana.")]
        [StringLength(64, ErrorMessage = "Nazwa może mieć maksymalnie 64 znaki.")]
        [Display(Name = "Nazwa ćwiczenia")]
        public string Name { get; set; } = string.Empty;

        [StringLength(256, ErrorMessage = "Opis może mieć maksymalnie 256 znaków.")]
        [Display(Name = "Opis (opcjonalny)")]
        public string? Description { get; set; }

        public string? UserId { get; set; }

        [Display(Name = "Użytkownik")]
        public ApplicationUser? User { get; set; }

        public ICollection<PerformedExercise> PerformedExercises { get; set; }
            = new List<PerformedExercise>();
    }
}
