using HikingGroupWebApp.Data;
using HikingGroupWebApp.Interfaces;
using HikingGroupWebApp.Models;
using HikingGroupWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;


namespace HikingGroupWebApp.Controllers
{
    public class HikingTripController : Controller
    {
        private readonly IHikingTripRepository _hikingtripRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        public HikingTripController(IHikingTripRepository hikingtripRepository, IPhotoService photoService, 
            IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _hikingtripRepository = hikingtripRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<HikingTrip> hikingtrips = await _hikingtripRepository.GetAll();
            return View(hikingtrips);
        }
        public async Task<IActionResult> Details(int id)
        {
            HikingTrip hikingtrip = await _hikingtripRepository.GetByIdAsync(id);
            return hikingtrip == null ? NotFound() : View(hikingtrip);
        }
        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createHikingTripViewModel = new CreateHikingTripViewModel { AppUserId = curUserId };
            return View(createHikingTripViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateHikingTripViewModel hikingtripVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(hikingtripVM.Image);
                var hikingtrip = new HikingTrip
                {
                    Title = hikingtripVM.Title,
                    Description = hikingtripVM.Description,
                    Image = result.Url.ToString(),
                    HikingTripCategory = hikingtripVM.HikingTripCategory,
                    AppUserId = hikingtripVM.AppUserId,
                    Address = new Address
                    {
                        Street = hikingtripVM.Address.Street,
                        City = hikingtripVM.Address.City,
                        Prefecture = hikingtripVM.Address.Prefecture
                    }
                };
                _hikingtripRepository.Add(hikingtrip);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(hikingtripVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var hikingtrip = await _hikingtripRepository.GetByIdAsync(id);
            if (hikingtrip == null) return View("Error");
            var hikingtripVM = new EditHikingTripViewModel
            {
                Title = hikingtrip.Title,
                Description = hikingtrip.Description,
                AddressId = hikingtrip.AddressId,
                Address = hikingtrip.Address,
                URL = hikingtrip.Image,
                HikingTripCategory = hikingtrip.HikingTripCategory,
                AppUserId = hikingtrip.AppUserId
            };
            return View(hikingtripVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditHikingTripViewModel hikingtripVM)
        {
            // Remove the validation for Image field if no new image is uploaded
            if (hikingtripVM.Image == null)
            {
                ModelState.Remove("Image");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(currentUser, "admin");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                    if (error.ErrorMessage == "The AppUserId field is required." && isAdmin == true)
                    {
                        // Optionally remove this specific error if you don't want it in ModelState
                        ModelState.Remove("AppUserId"); // This will stop the error from appearing in validation summary
                        break; // Exit the loop as we found the error and handled it
                    }
                    ModelState.AddModelError("", "Failed to edit hikingtrip");
                    return View("Edit", hikingtripVM);
                }
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Failed to edit club");
                    return View("Edit", hikingtripVM);
                }
            }

            var userHikingTrip = await _hikingtripRepository.GetByIdAsyncNoTracking(id);

            if (userHikingTrip != null)
            {
                string imageUrl = userHikingTrip.Image; // Preserve the existing image
                if (userHikingTrip.Image != null) // If a new image is uploaded
                {
                    try
                    {
                        // Delete the old photo from Cloudinary
                        var fi = new FileInfo(userHikingTrip.Image);
                        var publicId = Path.GetFileNameWithoutExtension(fi.Name);
                        await _photoService.DeletePhotoAsync(publicId);

                        // Add the new photo
                        var photoResult = await _photoService.AddPhotoAsync(hikingtripVM.Image);
                        imageUrl = photoResult.Url.ToString(); // Update with the new image URL
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Could not delete or upload photo");
                        return View(hikingtripVM);
                    }
                }

                hikingtripVM.AppUserId = userHikingTrip.AppUserId;

                var hikingTrip = new HikingTrip
                {
                    Id = id,
                    Title = hikingtripVM.Title,
                    Description = hikingtripVM.Description,
                    HikingTripCategory = hikingtripVM.HikingTripCategory,
                    Image = imageUrl,
                    AddressId = hikingtripVM.AddressId,
                    Address = hikingtripVM.Address,
                    AppUserId = hikingtripVM.AppUserId
                };

                _hikingtripRepository.Update(hikingTrip);

                return RedirectToAction("Index");
            }
            else
            {
                return View(hikingtripVM);
            }
        }


        public async Task<IActionResult> Delete(int id)
        {
            var hikingtripDetails = await _hikingtripRepository.GetByIdAsync(id);
            if (hikingtripDetails == null) return View("Error");
            return View(hikingtripDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteHikingTrip(int id)
        {
            var hikingtripDetails = await _hikingtripRepository.GetByIdAsync(id);
            if (hikingtripDetails == null) return View("Error");

            _hikingtripRepository.Delete(hikingtripDetails);
            return RedirectToAction("Index");
        }
    }
}
