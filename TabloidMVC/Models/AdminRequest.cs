using System;

namespace TabloidMVC.Models
{
    public class AdminRequest
    {
        public int RequesterUserProfileId { get; set; }
        public int TargetUserProfileId { get; set; }
        public bool Demote { get; set; }
        public bool Deactivate { get; set; }
        public DateTime? CreateDateTime { get; set; }
    }
}
