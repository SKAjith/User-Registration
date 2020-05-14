using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserRegistration.Models;
using UserRegistration.Data;
using System.Security.Claims;
using System.Text;

namespace UserRegistration.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<VerificationUser> userManager;
        private readonly SignInManager<VerificationUser> signInManager;

        public AccountController(UserManager<VerificationUser> userManager, SignInManager<VerificationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new VerificationUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    
                    await signInManager.SignInAsync(user, isPersistent: false);
                    user.VerificationCode = RandomString(10, true);
                    await userManager.UpdateAsync(user);
                    return RedirectToAction("index", "home");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                VerificationUser verificationUser = await userManager.GetUserAsync(User);
                if (verificationUser.VerificationCode == model.VerificationCode)
                {
                    verificationUser.EmailConfirmed = true;
                }
                else
                {
                    verificationUser.EmailConfirmed = false;
                }
                await userManager.UpdateAsync(verificationUser);

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
                if (result.Succeeded)
                {
                    //var finalresult = await signInManager.TwoFactorAuthenticatorSignInAsync(model.VerificationCode, false,false)
                    //if(verificationUser.IsUpdated == true)
                    //{
                    //    return RedirectToAction("UserInfoReadOnly", "Account");
                    //}
                    return RedirectToAction("UserDetails", "Account");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult UserDetails()
        {
            return View();
        }

      
        [HttpPost]
        public async Task<IActionResult> UserDetails(UserDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //var userName = User.FindFirstValue(ClaimTypes.Name);
                VerificationUser verificationUser = await userManager.GetUserAsync(User);
                verificationUser.FirstName = model.FirstName;
                verificationUser.LastName = model.LastName;
                verificationUser.AddressLine1 = model.AddressLine1;
                verificationUser.AddressLine2 = model.AddressLine2;
                verificationUser.City = model.City;
                verificationUser.State = model.State;
                verificationUser.IsPromotionsEnable = model.IsPromotionsEnable;
                verificationUser.IsUpdated = true;

                var result = await userManager.UpdateAsync(verificationUser);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Update Failed");
                }

                
            }
            return View(model);
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }


}
