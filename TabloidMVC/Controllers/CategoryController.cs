using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        // GET: CategoryController
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            List<Category> categories = _categoryRepository.GetAll();

            if (categories.Count < 1)
            {
                return NotFound();
            }

            return View(categories);
        }

        // GET: CategoryController/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            try
            {
                _categoryRepository.Add(category);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            Category category = _categoryRepository.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                _categoryRepository.Update(category);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(category);
            }
        }

        // GET: CategoryController/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            Category category = _categoryRepository.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Category category)
        {
            try
            {
                _categoryRepository.Delete(category);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(category);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
