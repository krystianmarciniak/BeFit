using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeFit.Models
{
    public class TrainingSession
    {
        public int Id { get; set; }

        [Required, StringLength(128)]
        [Display(Name = "Tytuł")]
        public string Title { get; set; } = "";

        [Display(Name = "Start")]
        public DateTime? StartTime { get; set; }

        [Display(Name = "Koniec")]
        public DateTime? EndTime { get; set; }

        [Display(Name = "Data sesji")]
        public DateTime SessionDate { get; set; } = DateTime.Today;

        [Display(Name = "Czas trwania [min]")]
        [Range(0, 1440)]
        public int DurationMinutes { get; set; }

        [Display(Name = "Notatki")]
        [StringLength(1000)]
        public string? Notes { get; set; }

        // Alias dla zgodności wstecznej z kontrolerem/widokami używającymi "Date"
        [NotMapped]
        public DateTime Date
        {
            get => SessionDate;
            set => SessionDate = value;
        }

        public string? UserId { get; set; }

        public ICollection<PerformedExercise> PerformedExercises { get; set; } = new List<PerformedExercise>();
    }
}
