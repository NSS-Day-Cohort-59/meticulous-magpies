using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.Extensions.Hosting;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserProfileRepository _userRepository;
        private readonly ITagRepository _tagRepo;
        private readonly ICommentRepository _commentRepository;

        public PostController(IPostRepository postRepository, ITagRepository tagRepo, ICategoryRepository categoryRepository, IUserProfileRepository userRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _tagRepo = tagRepo;
            _commentRepository = commentRepository;
        }

        [Authorize]
        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            var users = _userRepository.GetAll();
            var categories = _categoryRepository.GetAll();
            var vu = new PostByUserViewModel();
            vu.Post = posts;
            vu.UserProfiles = users;
            vu.Categories = categories;
            return View(vu);
        }

        [Authorize]
        public IActionResult MyPosts()
        {
            int authorId = GetCurrentUserProfileId();
            var posts = _postRepository.GetAllPostsByUser(authorId);
            return View(posts);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            var selectedTags = _tagRepo.GetTagsOnPost(id);
            var postComments = _commentRepository.GetAllCommentsByPost(id);
            PostDetailsViewModel postDetailsViewModel = new PostDetailsViewModel()
            {
                Post = post,
                Tags = selectedTags,
                Comments = postComments
            };


            return View(postDetailsViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateTags(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);

            }
            var selectedTags = _tagRepo.GetTagsByPostId(id);

            PostTagViewModel tagViewModel = new PostTagViewModel()
            {
                Post = post,
                Tags = _tagRepo.GetAllTags(),

                PostTag = new PostTag()
                {
                    PostId = post.Id,
                    TagIds = selectedTags
                }
            };


            return View(tagViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTags(PostTag postTag, int id)
        {
            try
            {
                _postRepository.DeletePostTagsonPost(id);
                postTag.PostId = id;
                _postRepository.AddPostTag(postTag);
                return RedirectToAction("Details", new { id = postTag.PostId });
            }
            catch (Exception ex)
            {

                return View(postTag);
            }
        }

        [Authorize]
        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        [Authorize]
        public IActionResult CreateComment(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            Comment comment = new Comment()
            {
                PostId = post.Id,
                UserProfileId = GetCurrentUserProfileId()
            };
            return View(comment);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateComment(Comment comment, int id)
        {
            try
            {
                var post = _postRepository.GetPublishedPostById(id);
                comment.CreateDateTime = DateAndTime.Now;
                comment.PostId = post.Id;
                comment.UserProfileId = GetCurrentUserProfileId();

                _commentRepository.Add(comment);
                

                return RedirectToAction("Details", new { id = comment.PostId });
            }
            catch
            {
                return View(comment);
            }
        }

        // GET: PostController/EditComment/5
        [Authorize]
        public IActionResult EditComment(int id)
        {
            int userId = GetCurrentUserProfileId();
            var comment = _commentRepository.GetCommentById(id);

            if (comment == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin") && comment.UserProfileId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return NotFound();
            }

            return View(comment);

        }


        // POST: PostController/EditComment/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult EditComment(Comment comment)
        {
            try
            {
                _commentRepository.UpdateComment(comment);
                return RedirectToAction("Details", new { id = comment.PostId });
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }


        // GET: PostController/Edit/5
        [Authorize]
        public IActionResult Edit(int id)
        {
            int userId = GetCurrentUserProfileId();
            var post = _postRepository.GetPublishedPostById(id);

            if (post == null)
            {

                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }

            if (!User.IsInRole("Admin") && post.UserProfileId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return NotFound();
            }

            var vm = new PostEditViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            vm.Post = post;
            return View(vm);

        }

        // POST: PostController/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PostEditViewModel vm)
        {
            try
            {
                _postRepository.UpdatePost(vm.Post);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(vm.Post);
            }
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
        // GET: PostController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);

            if (post == null || (post.UserProfileId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) && !User.IsInRole("Admin")))
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.Delete(post);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(post);
            }

        }

        // GET: PostController/DeleteComment/5
        [Authorize]
        public ActionResult DeleteComment(int id)
        {
            Comment comment = _commentRepository.GetCommentById(id);

            if (comment == null || (comment.UserProfileId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))))
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: PostController/DeleteComment/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteComment(Comment comment)
        {
            int detailId = comment.PostId;
            try
            {
                _commentRepository.DeleteComment(comment.Id);

                return RedirectToAction("Details", new { id = detailId });
            }
            catch
            {
                return View(comment);
            }

        }

        [Authorize]
        public IActionResult UsersPosts(IFormCollection thing)
        {
            var posts = _postRepository.GetAllPostsByUser(int.Parse(thing["UserProfiles"]));
            return View(posts);
        }

        [Authorize]
        public IActionResult CategoryPosts(IFormCollection thing)
        {
            var posts = _postRepository.PostsByCategory(int.Parse(thing["Categories"]));
            return View(posts);
        }
    }
}
