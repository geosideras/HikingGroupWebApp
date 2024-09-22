using CloudinaryDotNet.Actions;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;
        public DashboardController(IDashboardRepository dashboardRepository, 
            IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }
        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editVM, ImageUploadResult photoResult)
        {
            user.Id = editVM.Id;
            user.Pace = editVM.Pace;
            user.MeanDistance = editVM.MeanDistance;
            user.ProfileImageUrl = photoResult.Url.ToString();
            user.City = editVM.City;
            user.Prefecture = editVM.Prefecture;
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
        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(curUserId);
            if (user == null) return View("Error");
            var editUserViewModel = new EditUserDashboardViewModel()
            {
                Id = curUserId,
                Pace = user.Pace,
                MeanDistance = user.MeanDistance,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                Prefecture = user.Prefecture
            };
            return View(editUserViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile!");
                return View("EditUserProfile", editVM);
            }

            AppUser user = await _dashboardRepository.GetByIdNoTracking(editVM.Id);

            if (user.ProfileImageUrl == null || user.ProfileImageUrl == "")
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                //Optimistic Concurency - "Tracking error"
                MapUserEdit(user, editVM, photoResult);

                _dashboardRepository.Update(user);
                return RedirectToAction("Index");

            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user, editVM, photoResult);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");

            }
        }
    }
}
