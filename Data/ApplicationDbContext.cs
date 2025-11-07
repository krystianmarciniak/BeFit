using BeFit.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<BeFit.Models.ExerciseType> ExerciseTypes { get; set; } = default!;

    public DbSet<TrainingSession> TrainingSessions => Set<TrainingSession>();
    public DbSet<PerformedExercise> PerformedExercises => Set<PerformedExercise>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PerformedExercise>()
            .HasOne(pe => pe.TrainingSession)
            .WithMany(ts => ts.PerformedExercises)
            .HasForeignKey(pe => pe.TrainingSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PerformedExercise>()
            .HasOne(pe => pe.ExerciseType)
            .WithMany(et => et.PerformedExercises)
            .HasForeignKey(pe => pe.ExerciseTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
