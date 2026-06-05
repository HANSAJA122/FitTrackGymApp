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

    // --- Membership Plan CRUD Operations ---

    public async Task<List<MembershipPlan>> GetPlansAsync()
    {
        return await _dbContext.MembershipPlans.ToListAsync();
    }

    public async Task<int> AddMembershipPlanAsync(MembershipPlan plan)
    {
        _dbContext.MembershipPlans.Add(plan);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateMembershipPlanAsync(MembershipPlan plan)
    {
        _dbContext.MembershipPlans.Update(plan);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteMembershipPlanAsync(MembershipPlan plan)
    {
        _dbContext.MembershipPlans.Remove(plan);
        return await _dbContext.SaveChangesAsync();
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
                        (m.PhoneNumber != null && m.PhoneNumber.Contains(searchTerm)) || 
                        (m.Email != null && m.Email.ToLower().Contains(searchTerm.ToLower())))
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

    // --- Payment CRUD Operations ---

    public async Task<List<Payment>> GetPaymentsAsync()
    {
        return await _dbContext.Payments.Include(p => p.Member).ToListAsync();
    }

    public async Task<List<Payment>> SearchPaymentsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) 
            return await GetPaymentsAsync();
        
        return await _dbContext.Payments
            .Include(p => p.Member)
            .Where(p => (p.Member != null && p.Member.FullName.ToLower().Contains(searchTerm.ToLower())) || 
                        (p.Status != null && p.Status.ToLower().Contains(searchTerm.ToLower())))
            .ToListAsync();
    }

    public async Task<int> AddPaymentAsync(Payment payment)
    {
        _dbContext.Payments.Add(payment);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> UpdatePaymentAsync(Payment payment)
    {
        _dbContext.Payments.Update(payment);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> DeletePaymentAsync(Payment payment)
    {
        _dbContext.Payments.Remove(payment);
        return await _dbContext.SaveChangesAsync();
    }

    // --- Dashboard Stats ---
    
    public async Task<int> GetTotalMembersCountAsync() => await _dbContext.Members.CountAsync();
    
    public async Task<int> GetActiveMembersCountAsync() => await _dbContext.Members.CountAsync(m => m.Status == "Active");
    
    public async Task<int> GetPendingPaymentsCountAsync() => await _dbContext.Payments.CountAsync(p => p.Status == "Pending");

    public async Task<int> GetTodayCheckinsCountAsync()
    {
        var today = DateTime.Today;
        return await _dbContext.Attendances
            .CountAsync(a => a.CheckInDateTime.Date == today);
    }

    // --- Attendance CRUD Operations ---

    public async Task<List<Attendance>> GetAttendancesAsync()
    {
        return await _dbContext.Attendances
            .Include(a => a.Member)
            .OrderByDescending(a => a.CheckInDateTime)
            .ToListAsync();
    }

    public async Task<List<Attendance>> SearchAttendancesAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) 
            return await GetAttendancesAsync();
        
        return await _dbContext.Attendances
            .Include(a => a.Member)
            .Where(a => a.Member != null && a.Member.FullName.ToLower().Contains(searchTerm.ToLower()))
            .OrderByDescending(a => a.CheckInDateTime)
            .ToListAsync();
    }

    public async Task<bool> HasCheckedInTodayAsync(int memberId)
    {
        var today = DateTime.Today;
        return await _dbContext.Attendances
            .AnyAsync(a => a.MemberId == memberId && a.CheckInDateTime.Date == today);
    }

    public async Task<int> AddAttendanceAsync(Attendance attendance)
    {
        _dbContext.Attendances.Add(attendance);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteAttendanceAsync(Attendance attendance)
    {
        _dbContext.Attendances.Remove(attendance);
        return await _dbContext.SaveChangesAsync();
    }
}
