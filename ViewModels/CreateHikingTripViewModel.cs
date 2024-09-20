﻿using HikingGroupWebApp.Data.Enum;
using HikingGroupWebApp.Models;

namespace HikingGroupWebApp.ViewModels
{
    public class CreateHikingTripViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public IFormFile Image { get; set; }
        public HikingTripCategory HikingTripCategory { get; set; }
        public string AppUserId { get; set; }
    }
}
