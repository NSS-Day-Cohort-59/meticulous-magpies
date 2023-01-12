using System;

namespace TabloidMVC.Models.ViewModels
{
    public class Subscription
    {
        public int Id { get; set; }
        public int SubscriberUserProfileId { get; set; }
        public int ProviderUserProfileId { get; set; }
        public DateTime BeginDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
