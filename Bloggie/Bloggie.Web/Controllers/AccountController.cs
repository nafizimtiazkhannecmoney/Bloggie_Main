﻿using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel) 
        {
            var identityUser = new IdentityUser
            {
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

            if (identityResult.Succeeded)
            {
                // Assing this user to the user role
                var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");
             
                if (roleIdentityResult.Succeeded)
                {
                    // Show success message notification
                    return RedirectToAction("Index", "Home");
                }
            }

            //show error message notification
            return View(); 
        }

        [HttpGet]
        public IActionResult Login() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);
            if (signInResult.Succeeded && signInResult != null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Show error message notification
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        { 
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
