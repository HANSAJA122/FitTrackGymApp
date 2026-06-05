using Microsoft.EntityFrameworkCore;
using FitTrackGymApp.Models;

namespace FitTrackGymApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<Member> Members { get; set; }
    public DbSet<MembershipPlan> MembershipPlans { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Attendance> Attendances { get; set; }

    public AppDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Ensure the path is correct for Android/MAUI
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FitTrackGym.db");
        optionsBuilder.UseSqlite($"Filename={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed basic membership plans
        modelBuilder.Entity<MembershipPlan>().HasData(
            new MembershipPlan { Id = 1, PlanName = "Monthly Plan", DurationInMonths = 1, Price = 50.00m, Description = "Access for 1 month" },
            new MembershipPlan { Id = 2, PlanName = "3 Month Plan", DurationInMonths = 3, Price = 135.00m, Description = "Access for 3 months" },
            new MembershipPlan { Id = 3, PlanName = "6 Month Plan", DurationInMonths = 6, Price = 250.00m, Description = "Access for 6 months" },
            new MembershipPlan { Id = 4, PlanName = "Annual Plan", DurationInMonths = 12, Price = 480.00m, Description = "Access for a full year" }
        );
    }
}
