using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sloths.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Sloths.Controllers
{
    public class SlothController : Controller
    {

        private SlothContext context;
        public static PasswordHasher<User> RegisterHasher = new PasswordHasher<User>();
        public static PasswordHasher<LoginUser> LoginHasher = new PasswordHasher<LoginUser>();
        
        public SlothController(SlothContext sc)
        {
            context = sc;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(User u)
        {
            User userInDb = context.GetUserByEmail(u.Email);
            if(userInDb != null)
            {
                ModelState.AddModelError("Email", "Email already in use!");
            }
            if(ModelState.IsValid)
            {
                u.Password = RegisterHasher.HashPassword(u, u.Password);
                int UserId = context.Create(u);
                HttpContext.Session.SetInt32("UserId", UserId);
                return Redirect("/success");
            }
            return View("Index");
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginUser u)
        {
            User userInDb = context.GetUserByEmail(u.LoginEmail);
            if(userInDb == null)
            {
                ModelState.AddModelError("LoginEmail", "Unknown Email!");
            }
            if(ModelState.IsValid)
            {
                var result = LoginHasher.VerifyHashedPassword(u, userInDb.Password, u.LoginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Incorrect!");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    return Redirect("/success");
                }
            }
            return View("Index");
        }

        [Route("success")]
        [HttpGet]
        public IActionResult Success()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            }
            else
            {
                ViewBag.User = context.GetUserById((int)UserId);
                return View();
            }
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

    }
}
