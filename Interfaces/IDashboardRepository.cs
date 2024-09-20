using HikingGroupWebApp.Models;

namespace HikingGroupWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<HikingTrip>> GetAllUsersHikingTrips();
        Task<List<Club>> GetAllUsersClubs();
    }
}
