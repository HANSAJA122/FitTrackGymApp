using FitTrackGymApp.Data;
using FitTrackGymApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FitTrackGymApp.Services;

public class DatabaseService
{
    private readonly AppDbContext _dbContext;

    public DatabaseService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Initialize the database, creating it and seeding data if it doesn't exist
    public async Task InitializeDatabaseAsync()
    {
        await _dbContext.Database.EnsureCreatedAsync();
    }

    // A simple test method for retrieving the seeded plans
    public async Task<List<MembershipPlan>> GetPlansAsync()
    {
        return await _dbContext.MembershipPlans.ToListAsync();
    }
}
