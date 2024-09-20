using System.ComponentModel.DataAnnotations;

namespace HikingGroupWebApp.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Prefecture { get; set; }
    }
}
