using HikingGroupWebApp.Models;

namespace HikingGroupWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<HikingTrip>> GetAllUsersHikingTrips();
        Task<List<Club>> GetAllUsersClubs();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetByIdNoTracking(string id);
        bool Update(AppUser user);
        bool Save();
    }
}
