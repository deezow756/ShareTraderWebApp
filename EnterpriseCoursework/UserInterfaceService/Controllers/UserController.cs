using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UserInterfaceService.Models;
using UserInterfaceService.Models.ViewModels;

namespace UserInterfaceService.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration config;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.httpClientFactory = httpClientFactory;
            this.config = configuration;
        }

        [HttpGet]
        [Route("User/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("User/Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return Redirect("/");
                    }

                    ModelState.AddModelError("", "Username or Password incorrect");
                }
            }               
            
            return View(model);
        }

        [HttpGet]
        [Route("User/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("User/Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var role = await roleManager.FindByNameAsync("Normal");

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role.Name);
                    await signInManager.SignInAsync(user, false);
                    return Redirect("/");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        [Route("/User/GetUser")]
        public async Task<JsonResult> GetUser()
        {
            bool signIn = signInManager.IsSignedIn(User);
            JsonResponseModel model = null;
            if (signIn)
            {
                IdentityUser user = await userManager.FindByNameAsync(User.Identity.Name);
                model = new JsonResponseModel() { status = true, msg = user.Id };
                return new JsonResult(model);
            }
            else
            {
                model = new JsonResponseModel() { status = false, msg = "User Not Sign In" };
                return new JsonResult(model);
            }
        }
    }
}
