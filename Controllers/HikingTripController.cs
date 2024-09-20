using HikingGroupWebApp.Data;
using HikingGroupWebApp.Interfaces;
using HikingGroupWebApp.Models;
using HikingGroupWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;


namespace HikingGroupWebApp.Controllers
{
    public class HikingTripController : Controller
    {
        private readonly IHikingTripRepository _hikingtripRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HikingTripController(IHikingTripRepository hikingtripRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _hikingtripRepository = hikingtripRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
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

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                ModelState.AddModelError("", "Failed to edit hikingtrip");
                return View("Edit", hikingtripVM);
            }

            var userHikingTrip = await _hikingtripRepository.GetByIdAsyncNoTracking(id);

            if (userHikingTrip != null)
            {
                string imageUrl = userHikingTrip.Image; // Preserve the existing image
                try
                {
                    //Change in order to not keeping the old uploaded photo in cloudinary
                    var fi = new FileInfo(userHikingTrip.Image);
                    var publicId = Path.GetFileNameWithoutExtension(fi.Name);
                    await _photoService.DeletePhotoAsync(publicId);
                    //Original Code
                    //await _photoService.DeletePhotoAsync(userHikingTrip.Image);
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(hikingtripVM);
                }

                hikingtripVM.AppUserId = userHikingTrip.AppUserId;

                var hikingTrip = new HikingTrip
                {
                    Id = id,
                    Title = hikingtripVM.Title,
                    Description = hikingtripVM.Description,
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
