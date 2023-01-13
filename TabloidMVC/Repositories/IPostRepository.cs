using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        List<Post> GetAllPublishedPosts();
        List<Post> GetAllPostsByUser(int userProfileId);
        List<Post> PostsByCategory(int catId);
        List<Post> PostsByTag(int tagId);
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
        void Add(Post post);
        void Delete(Post post);
        void UpdatePost(Post post);
        void AddPostTag(PostTag postTag);
        void DeletePostTagsonPost(int id);
    }
}