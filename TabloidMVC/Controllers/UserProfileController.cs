using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TabloidMVC.Models;
using TabloidMVC.Models.Enums;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using TabloidMVC.Utils;

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
        [Authorize]
        public ActionResult Details(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetById(id);

            if (userProfile == null || (!User.IsInRole("Admin") && userProfile.Id != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))))
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // GET: UserProfileController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetById(id);
            List<UserType> userTypes = _userTypeRepository.GetAll().OrderBy(x => x.Name).ToList();

            if (userProfile == null || (!User.IsInRole("Admin") && userProfile.Id != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))) || userTypes.Count < 1)
            {
                return NotFound();
            }

            if (userProfile.ImageLocation == GravatarUtil.GetImageUrl(userProfile.Email))
            {
                userProfile.ImageLocation = null;
            }

            EditUserProfileViewModel vm = new()
            {
                UserProfile = userProfile,
                UserTypes = userTypes,
                DemotionRequests = _userProfileRepository.RequestsByUserId(id, (int)AdminRequestTypeEnum.Demote)
            };

            //! Check if there is already a demotionRequest, and add the requester's id if there is.
            if (vm.DemotionRequests == 1)
            {
                vm.ExistingDemotionRequesterId = _userProfileRepository.GetRequesterId(id, (int)AdminRequestTypeEnum.Demote);
            }

            return View(vm);
        }

        // POST: UserProfileController/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditUserProfileViewModel vm)
        {
            int amountOfAdmins = _userProfileRepository.GetAdminCount();
            UserProfile currentProfile = _userProfileRepository.GetById(id);
            UserProfile existingProfile = _userProfileRepository.GetByEmail(vm.UserProfile.Email);

            //! Check if they are trying to demote the last admin
            if (amountOfAdmins == 1 && (vm.UserProfile.UserTypeId == (int)UserProfileTypeEnum.Author && currentProfile.UserTypeId == (int)UserProfileTypeEnum.Admin))
            {
                ModelState.AddModelError("UserProfile.UserTypeId", "Make someone else an admin before the User Profile can be changed.");
                vm.UserTypes = _userTypeRepository.GetAll().OrderBy(x => x.Name).ToList();

                return View(vm);
            }
            
            //! Check if they are trying to get an email that another user already has that email
            if (currentProfile.Email != vm.UserProfile.Email && existingProfile != null)
            {
                ModelState.AddModelError("UserProfile.Email", "An account with that email already exists.");
                vm.UserTypes = _userTypeRepository.GetAll().OrderBy(x => x.Name).ToList();

                return View(vm);
            }

            try
            {
                //! Construct a new demotion request if existing one is not from current admin && trying to demote an admin
                if (vm.UserProfile.UserTypeId == (int)UserProfileTypeEnum.Author && vm.UserProfile.UserTypeId != currentProfile.UserTypeId && int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) != vm.ExistingDemotionRequesterId)
                {
                    AdminRequest newRequest = new()
                    {
                        RequesterUserProfileId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                        TargetUserProfileId = currentProfile.Id,
                        RequestTypeId = (int)AdminRequestTypeEnum.Demote
                    };

                    _userProfileRepository.RequestChange(newRequest);

                    //! If it was 1 before this current request*
                    if (vm.DemotionRequests == 1)
                    {
                        _userProfileRepository.RetireRequest(currentProfile.Id, (int)AdminRequestTypeEnum.Demote);
                    }
                    else
                    {
                        vm.UserProfile.UserTypeId = 1;
                    }
                }
                else if (int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) == vm.ExistingDemotionRequesterId && vm.UserProfile.UserTypeId == (int)UserProfileTypeEnum.Admin) //! If the admin edits and saves their role as admin
                {
                    _userProfileRepository.CancelRequest(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), vm.UserProfile.Id, (int)AdminRequestTypeEnum.Demote);
                }
                else if (vm.UserProfile.UserTypeId == (int)UserProfileTypeEnum.Author && currentProfile.UserTypeId == (int)UserProfileTypeEnum.Admin) //! If the admin is trying to demote them again
                {
                    ModelState.AddModelError("UserProfile.UserTypeId", "You already put in a demotion request for this user.");
                    vm.UserProfile.UserTypeId = 2;
                    vm.UserTypes = _userTypeRepository.GetAll().OrderBy(x => x.Name).ToList();

                    return View(vm);
                }

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

            DeactivateUserProfileViewModel vm = new()
            {
                UserProfile = userProfile,
                DeactivationRequests = _userProfileRepository.RequestsByUserId(id, (int)AdminRequestTypeEnum.Deactivate)
            };

            //! Check if there is already a demotionRequest, and add the requester's id if there is.
            if (vm.DeactivationRequests == 1)
            {
                vm.ExistingDeactivationRequesterId = _userProfileRepository.GetRequesterId(id, (int)AdminRequestTypeEnum.Deactivate);
            }

            return View(vm);
        }

        //SET 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(int id, DeactivateUserProfileViewModel vm)
        {
            int amountOfAdmins = _userProfileRepository.GetAdminCount();

            if (amountOfAdmins == 1)
            {
                ModelState.AddModelError("UserProfile.UserTypeId", "Make someone else an admin before the User Profile can be changed.");
                return View(vm);
            }
            else if (int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) == vm.UserProfile.Id)
            {
                ModelState.AddModelError("UserProfile.UserTypeId", "You cannot deactivate your own account.");
                return View(vm);
            }

            try
            {
                if (vm.UserProfile.UserTypeId == (int)UserProfileTypeEnum.Admin)
                {
                    //! First, make the request
                    AdminRequest newRequest = new()
                    {
                        RequesterUserProfileId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                        TargetUserProfileId = id,
                        RequestTypeId = (int)AdminRequestTypeEnum.Deactivate
                    };
                    _userProfileRepository.RequestChange(newRequest);

                    //! Check if there was already an existing request before making this new one (which would make it a total of 2)
                    if (vm.DeactivationRequests == 1)
                    {
                        _userProfileRepository.Deactivate(vm.UserProfile);

                        _userProfileRepository.RetireRequest(id, (int)AdminRequestTypeEnum.Deactivate);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelDeactivate(int id, DeactivateUserProfileViewModel vm)
        {
            try
            {
                _userProfileRepository.CancelRequest(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), vm.UserProfile.Id, (int)AdminRequestTypeEnum.Deactivate);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(vm);
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
