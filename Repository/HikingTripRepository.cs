using HikingGroupWebApp.Data;
using HikingGroupWebApp.Interfaces;
using HikingGroupWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HikingGroupWebApp.Repository
{
    public class HikingTripRepository : IHikingTripRepository
    {
        private readonly ApplicationDbContext _context;
        public HikingTripRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public bool Add(HikingTrip hikingTrip)
        {
            _context.Add(hikingTrip);
            return Save();
        }

        public bool Delete(HikingTrip hikingTrip)
        {
            _context.Remove(hikingTrip);
            return Save();
        }

        public async Task<IEnumerable<HikingTrip>> GetAll()
        {
            return await _context.HikingTrips.ToListAsync();
        }

        public async Task<IEnumerable<HikingTrip>> GetAllHikingTripsByCity(string city)
        {
            return await _context.HikingTrips.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<HikingTrip> GetByIdAsync(int id)
        {
            return await _context.HikingTrips.Include(i => i.Address).FirstOrDefaultAsync(i => i.Id == id);
            
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(HikingTrip hikingTrip)
        {
            _context.Update(hikingTrip);
            return Save();
        }
    }
}
