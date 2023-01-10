using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;
        public UserProfileController (IUserProfileRepository profileRepository)
        {
            _userProfileRepository = profileRepository;
        }
        // GET: UserProfileController
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            List<UserProfile> userProfiles = _userProfileRepository.GetAll();

            if (userProfiles.Count < 1)
            {
                return NotFound();
            }

            return View(userProfiles);
        }

        // GET: UserProfile/Deactive
        [Authorize(Roles = "admin")]
        public ActionResult Deactive()
        {
            List<UserProfile> userProfiles = _userProfileRepository.GetAll();

            if (userProfiles.Count < 1)
            {
                return NotFound();
            }

            return View(userProfiles);
        }

        // GET: UserProfileController/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetById(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // GET: UserProfileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserProfileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET
        [Authorize(Roles = "admin")]
        public ActionResult Deactivate(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetById(id);
            
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(int id, UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.Deactivate(userProfile);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(userProfile);
            }
        }

        // GET
        [Authorize(Roles = "admin")]
        public ActionResult Activate(int id) 
        {
            UserProfile userProfile = _userProfileRepository.GetById(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(int id, UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.Activate(userProfile);

                return RedirectToAction(nameof(Index));
            } catch
            {
                return View(userProfile);
            }
        }
    }
}
