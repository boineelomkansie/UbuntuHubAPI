namespace UbuntuHub.Api.Models
{
    public class UserSyncRequest
    {
        public string FirebaseUid { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}