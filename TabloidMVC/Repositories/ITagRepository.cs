using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetAllTags();
        Tag GetTagById(int id);
        List<int> GetTagsByPostId(int id);


        void AddTag(Tag tag);
        void DeleteTag(Tag tag);
        void UpdateTag(Tag tag);
    }
}
