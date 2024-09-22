using HikingGroupWebApp.Models;

namespace HikingGroupWebApp.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Club> Clubs { get; set; }
        public string City { get; set; }
        public string Prefecture { get; set; }
    }
}
