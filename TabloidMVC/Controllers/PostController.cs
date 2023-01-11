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

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserProfileRepository _userRepository;
        private readonly ITagRepository _tagRepo;

        public PostController(IPostRepository postRepository, ITagRepository tagRepo, ICategoryRepository categoryRepository, IUserProfileRepository userRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _tagRepo = tagRepo;
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
            var post = _postRepository.Posts
                .Include(p => p.Comments)
                .FirstOrDefault(p => p.Id == postId);
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
            return View(post);
        }

        [Authorize]
        public IActionResult CreateTags(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);

            }

            PostTagViewModel tagViewModel = new PostTagViewModel()
            {
                Post = post,
                Tags = _tagRepo.GetAllTags(),
                PostTag = new PostTag()
                {
                    PostId = post.Id
                }
            };


            return View(tagViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTags(PostTag postTag, int id)
        {
            try
            {
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

        // GET: PostController/Edit/5
        [Authorize(Roles = "Admin")]
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
            var vm = new PostEditViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            vm.Post = post;
            return View(vm);

        }

        // POST: PostController/Edit/5
        [HttpPost]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: PostController/Delete/5
        [HttpPost]
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
