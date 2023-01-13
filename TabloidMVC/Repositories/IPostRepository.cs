using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        List<Post> GetAllPublishedPosts();
        Post GetPublishedPostById(int id);

        Post GetUserPostById(int id, int userProfileId);
        void Delete(Post post);
        void DeletePostTagsonPost(int id);
        void AddPostTag(PostTag postTag);
       

        List<Post> GetAllPostsByUser(int userProfileId);
        List<Post> PostsByCategory(int catId);
        void UpdatePost(Post post);

    }
}