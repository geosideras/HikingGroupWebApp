//using AspNetCore;
using HikingGroupWebApp.Interfaces;
using HikingGroupWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HikingGroupWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        [HttpGet("users")]
        public async Task<IActionResult> IndexAsync()
        {
            var users = await _userRepository.GetAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach(var user in users)
            {
                var userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    MeanDistance = user.MeanDistance
                };
                result.Add(userViewModel);
            }
            return View();
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUserById(id);
            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                MeanDistance = user.MeanDistance
            };
            return View(userDetailViewModel);
        }

    }
}
