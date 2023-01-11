using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostEditViewModel
    {
        public Post Post { get; set; }
        public List<Category> CategoryOptions { get; set; }
    }
}
