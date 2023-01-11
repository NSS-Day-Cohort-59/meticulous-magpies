using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        List<Comment> GetAllComments();
        Comment GetCommentById(int id);
        void CreateComment(Comment comment, int postId);
        void UpdateComment(Comment comment);
        void DeleteComment(int id);
        List<Comment> GetAllCommentsByPost(int postId);


    }
}