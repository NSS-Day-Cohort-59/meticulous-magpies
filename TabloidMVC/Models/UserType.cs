using System.ComponentModel;

namespace TabloidMVC.Models
{
    public class UserType
    {
        public int Id { get; set; }
        [DisplayName("Role")]
        public string Name { get; set; }
    }
}