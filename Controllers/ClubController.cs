using HikingGroupWebApp.Data;
using HikingGroupWebApp.Interfaces;
using HikingGroupWebApp.Models;
using HikingGroupWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;


namespace HikingGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager; 
        public ClubController(IClubRepository clubRepository, IPhotoService photoService, 
            IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager; 
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }
        public async Task<IActionResult> Details(int id)
        {
            Club club = await _clubRepository.GetByIdAsync(id);
            return club == null ? NotFound() : View(club);
        }
        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel { AppUserId = curUserId };
            return View(createClubViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    ClubCategory = clubVM.ClubCategory,
                    AppUserId = clubVM.AppUserId,
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        Prefecture = clubVM.Address.Prefecture
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
            
            return View(clubVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if(club == null) return View("Error");
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory,
                AppUserId = club.AppUserId
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            // Remove the validation for Image field if no new image is uploaded
            if (clubVM.Image == null)
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
                    ModelState.AddModelError("", "Failed to edit club");
                    return View("Edit", clubVM);
                }
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Failed to edit club");
                    return View("Edit", clubVM);
                }

            }

            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);

            if (userClub != null)
            {
                string imageUrl = userClub.Image; // Preserve the existing image

                if (clubVM.Image != null) // If a new image is uploaded
                {
                    try
                    {
                        // Delete the old photo from Cloudinary
                        var fi = new FileInfo(userClub.Image);
                        var publicId = Path.GetFileNameWithoutExtension(fi.Name);
                        await _photoService.DeletePhotoAsync(publicId);

                        // Add the new photo
                        var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);
                        imageUrl = photoResult.Url.ToString(); // Update with the new image URL
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Could not delete or upload photo");
                        return View(clubVM);
                    }
                }

                clubVM.AppUserId = userClub.AppUserId;

                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = imageUrl,
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address,
                    AppUserId = clubVM.AppUserId,
                    ClubCategory = clubVM.ClubCategory
                };

            _clubRepository.Update(club);

            return RedirectToAction("Index");
            }
            else
            {
                return View(clubVM);
            }
        }


        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");
            return View(clubDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");

            _clubRepository.Delete(clubDetails);
            return RedirectToAction("Index");
        }
    }
}
