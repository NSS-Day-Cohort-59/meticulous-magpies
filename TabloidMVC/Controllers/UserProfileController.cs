using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserTypeRepository _userTypeRepository;
        public UserProfileController (IUserProfileRepository profileRepository, IUserTypeRepository userTypeRepository)
        {
            _userProfileRepository = profileRepository;
            _userTypeRepository = userTypeRepository;
        }
        // GET: UserProfileController      Shows all active accounts
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<UserProfile> userProfiles = _userProfileRepository.GetAll();

            if (userProfiles.Count < 1)
            {
                return NotFound();
            }

            return View(userProfiles);
        }

        // GET: UserProfile/Deactive    Shows all deactive accounts
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetById(id);
            List<UserType> userTypes = _userTypeRepository.GetAll().OrderBy(x => x.Name).ToList();

            if (userProfile == null || userTypes.Count < 1)
            {
                return NotFound();
            }

            EditUserProfileViewModel vm = new()
            {
                UserProfile = userProfile,
                UserTypes = userTypes
            };

            return View(vm);
        }

        // POST: UserProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditUserProfileViewModel vm)
        {
            int amountOfAdmins = _userProfileRepository.GetAdminCount();

            if (amountOfAdmins == 1 && vm.UserProfile.UserTypeId == 2)   // 1) Admin  2) Author
            {
                ModelState.AddModelError("UserProfile.UserTypeId", "Make someone else an admin before the User Profile can be changed.");
                vm.UserTypes = _userTypeRepository.GetAll().OrderBy(x => x.Name).ToList();

                return View(vm);
            }
            
            try
            {
                _userProfileRepository.Update(vm.UserProfile);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                vm.UserTypes = _userTypeRepository.GetAll().OrderBy(x => x.Name).ToList();
                return View(vm);
            }
        }

        // GET
        [Authorize(Roles = "Admin")]
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
            int amountOfAdmins = _userProfileRepository.GetAdminCount();

            if (amountOfAdmins == 1 && userProfile.UserTypeId == 1)   // 1) Admin  2) Author
            {
                ModelState.AddModelError("UserTypeId", "Make someone else an admin before the User Profile can be changed.");
                return View(userProfile);
            }

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
        [Authorize(Roles = "Admin")]
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
