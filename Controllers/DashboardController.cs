using HikingGroupWebApp.Data;
using HikingGroupWebApp.Interfaces;
using HikingGroupWebApp.Models;
using HikingGroupWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HikingGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }
        public async Task<IActionResult> Index()
        {
            var userHikingTrips = await _dashboardRepository.GetAllUsersHikingTrips();
            var userClubs = await _dashboardRepository.GetAllUsersClubs();
            var dashboardViewModel = new DashboardViewModel
            {
                HikingTrips = userHikingTrips,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }
    }
}
