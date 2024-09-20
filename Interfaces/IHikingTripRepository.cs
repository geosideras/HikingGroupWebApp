using HikingGroupWebApp.Models;

namespace HikingGroupWebApp.Interfaces
{
    public interface IHikingTripRepository
    {
        Task<IEnumerable<HikingTrip>> GetAll();
        Task<HikingTrip> GetByIdAsync(int id);
        Task<IEnumerable<HikingTrip>> GetAllHikingTripsByCity(string city);
        bool Add(HikingTrip hikingTrip);
        bool Update(HikingTrip hikingTrip);
        bool Delete(HikingTrip hikingTrip);
        bool Save();
    }
}
