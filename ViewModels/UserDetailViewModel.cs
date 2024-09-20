namespace HikingGroupWebApp.ViewModels
{
    public class UserDetailViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? Pace { get; set; }
        public int? MeanDistance { get; set; }
        public string? City { get; set; }
        public string? Prefecture { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Location { get; set; }
    }
}
