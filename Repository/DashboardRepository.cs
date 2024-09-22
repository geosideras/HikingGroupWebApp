using HikingGroupWebApp.Interfaces;
using HikingGroupWebApp.Models;
using HikingGroupWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace HikingGroupWebApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) 
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Club>> GetAllUsersClubs()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userClubs = _context.Clubs.Where(r => r.AppUser.Id == curUser.ToString());
            return userClubs.ToList();
        }

        public async Task<List<HikingTrip>> GetAllUsersHikingTrips()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userHikingTrips = _context.HikingTrips.Where(r => r.AppUser.Id == curUser.ToString());
            return userHikingTrips.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetByIdNoTracking(string id)
        {
            return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved >  0 ? true : false;
        }
    }
}

