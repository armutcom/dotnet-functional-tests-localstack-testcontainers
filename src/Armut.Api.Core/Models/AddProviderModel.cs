namespace Armut.Api.Core.Models
{
    public class AddProviderModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int ServiceId { get; set; }

        public string ProfilePicture { get; set; }
    }
}