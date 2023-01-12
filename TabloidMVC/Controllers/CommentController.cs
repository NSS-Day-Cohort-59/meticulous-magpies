using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TabloidMVC.Repositories;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System;
using TabloidMVC.Models;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public CommentController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }

        // GET: CommentController
        public ActionResult Index()
        {
            var comments = _commentRepository.GetAllComments();
            return View(comments);
        }
        [Authorize]
        public IActionResult MyPostComments(int postId)
        {
            var comments = _commentRepository.GetAllCommentsByPost(postId);
            ViewData["PostId"] = postId;
            return View(comments);
        }


        [Authorize]
        public IActionResult Details(int id)
        {
            var comment = _commentRepository.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }


        [Authorize]
        public IActionResult Create()
        {
            var comment = new Comment();
            return View(comment);
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment, int postId)
        {
            comment.CreateDateTime = DateTime.Now;
            comment.PostId = postId;
            comment.UserProfileId = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            _commentRepository.CreateComment(comment, postId);

            return RedirectToAction("PostComments", new { id = postId });
        }



        // GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            var comment = _commentRepository.GetCommentById(id);
            return View(comment);
        }


        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Comment comment)
        {
            var commentToEdit = _commentRepository.GetCommentById(id);
            commentToEdit.Subject = comment.Subject;
            commentToEdit.Content = comment.Content;
            commentToEdit.PublishDateTime = comment.PublishDateTime;

            _commentRepository.UpdateComment(commentToEdit);

            return RedirectToAction("Index");
        }

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            var comment = _commentRepository.GetCommentById(id);
            return View(comment);
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _commentRepository.DeleteComment(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}