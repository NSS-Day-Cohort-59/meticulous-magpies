using System;

namespace TabloidMVC.Models
{
    public class AdminRequest
    {
        public int RequesterUserProfileId { get; set; }
        public int TargetUserProfileId { get; set; }
        public int RequestTypeId { get; set; }
        public DateTime? CreateDateTime { get; set; }
    }
}
