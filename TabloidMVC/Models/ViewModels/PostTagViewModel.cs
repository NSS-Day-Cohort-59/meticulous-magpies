using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace TabloidMVC.Models.ViewModels
{
    public class PostTagViewModel
    {
        public Post Post { get; set; }
        public PostTag PostTag { get; set; }
        public List<Tag> Tags { get; set; }
        public List<SelectListItem> Options { get; set; }
        public void OnGet()
        {
            Options = Tags.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList();
        }
    }
}
