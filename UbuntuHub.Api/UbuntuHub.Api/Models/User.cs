namespace UbuntuHub.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirebaseUid { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Bio { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}