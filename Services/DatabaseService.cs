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

    // --- Member CRUD Operations ---

    public async Task<List<Member>> GetMembersAsync()
    {
        return await _dbContext.Members.ToListAsync();
    }

    public async Task<List<Member>> SearchMembersAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) 
            return await GetMembersAsync();
        
        return await _dbContext.Members
            .Where(m => m.FullName.ToLower().Contains(searchTerm.ToLower()) || 
                        m.PhoneNumber.Contains(searchTerm) || 
                        m.Email.ToLower().Contains(searchTerm.ToLower()))
            .ToListAsync();
    }

    public async Task<int> AddMemberAsync(Member member)
    {
        _dbContext.Members.Add(member);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateMemberAsync(Member member)
    {
        _dbContext.Members.Update(member);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteMemberAsync(Member member)
    {
        _dbContext.Members.Remove(member);
        return await _dbContext.SaveChangesAsync();
    }
}
