using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public AccountController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Credentials credentials)
        {
            var userProfile = _userProfileRepository.GetByEmail(credentials.Email);

            if (userProfile == null)
            {
                ModelState.AddModelError("Email", "Invalid email");
                return View();
            }
            else if (!userProfile.IsActive)
            {
                ModelState.AddModelError("Email", "This account has been deactivated.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
                new Claim(ClaimTypes.Role, userProfile.UserType.Name) // Changed Role value to be whatever the Role Name is that's connected to the User in the DB  (UserType)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // This action has the anonymous User fill out a form with some basic User Data (see Register.cshtml)
        public IActionResult Register()
        {
            return View();
        }

        // This action handles the data that was given to the server using the model 
        [HttpPost]
        public async Task<IActionResult> Register(UserProfile newUserProfile)
        {
            //! First, we check if the database already has an account with this email. If it does, we inform the user.
            UserProfile existingProfile = _userProfileRepository.GetByEmail(newUserProfile.Email);

            if (existingProfile != null)
            {
                ModelState.AddModelError("Email", "An account with that email already exists.");
                return View(newUserProfile);
            }

            //! Add the default date values to the newUserProfile
            newUserProfile.CreateDateTime = DateTime.Now;
            newUserProfile.UserTypeId = 2; // "Author"

            try
            {
                _userProfileRepository.Add(newUserProfile);
            }
            catch
            {
                return View(newUserProfile);
            }

            UserProfile createdUserProfile = _userProfileRepository.GetByEmail(newUserProfile.Email);

            // These are the values tied to the cookie -- and the User object 
            List<Claim> claims = new() // Shorthand for new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, createdUserProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, createdUserProfile.Email),
                new Claim(ClaimTypes.Role, createdUserProfile.UserType.Name)
            };

            ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme); // Shorthand new()

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }
    }
}
