using HikingGroupWebApp.Interfaces;
using HikingGroupWebApp.Models;
using HikingGroupWebApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HikingGroupWebApp.Controllers
{
    public class HikingTripController : Controller
    {
        private readonly IHikingTripRepository _hikingTripRepository;
        public HikingTripController(IHikingTripRepository hikingTripRepository)
        {
            _hikingTripRepository = hikingTripRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<HikingTrip> HikingTrips = await _hikingTripRepository.GetAll();
            return View(HikingTrips);
        }
        public async Task<IActionResult> Detail(int id)
        {
            HikingTrip hikingTrip = await _hikingTripRepository.GetByIdAsync(id);
            return View(hikingTrip);

        }
        [HttpPost]
        public async Task<IActionResult> Create(HikingTrip hikingTrip)
        {
            if (!ModelState.IsValid)
            {
                return View(hikingTrip);
            }
            _hikingTripRepository.Add(hikingTrip);
            return RedirectToAction("Index");

        }

    }
}
