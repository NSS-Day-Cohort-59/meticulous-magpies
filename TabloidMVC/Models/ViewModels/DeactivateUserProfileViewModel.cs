namespace TabloidMVC.Models.ViewModels
{
    public class DeactivateUserProfileViewModel
    {
        public UserProfile UserProfile { get; set; }
        public int DeactivationRequests { get; set; }
        public int ExistingDeactivationRequesterId { get; set; }
    }
}
