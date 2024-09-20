using HikingGroupWebApp.Models;

namespace HikingGroupWebApp.ViewModels
{
    public class DashboardViewModel
    {
        public List<HikingTrip> HikingTrips { get; set; }
        public List<Club> Clubs { get; set; }
    }
}